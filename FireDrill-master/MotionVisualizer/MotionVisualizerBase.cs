using GraphControl;
using GraphData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using VisualizerBaseClasses;

namespace MotionVisualizer
{
    public class MotionVisualizerBase<TVisualizer, TCommand> : Window
        where TVisualizer : new()
        where TCommand : ICommand<TVisualizer>
    {
        public GraphDataManager Manager { get; } = new GraphDataManager();
        private IGraphDataInterface GraphManagerInterface;
        private GraphManager manager;


        private EngineCore<TVisualizer, TCommand> core;
        public TVisualizer Visualizer { get; set; }
        public CompositeGraph Graphs { get; } = new CompositeGraph();

        public MotionVisualizerBase(IEngine<TVisualizer, TCommand> engine, TVisualizer visualizer)
        {
            core = new RealTimeEngineCore<TVisualizer, TCommand>(engine, Manager);
            Visualizer = visualizer;
            FinishInitialization();
        }

        public MotionVisualizerBase(string filename, ICommandFileReader<TVisualizer> factory, TVisualizer visualizer)
        {
            core = new FromFileEngineCore<TVisualizer, TCommand>(filename, factory);
            Visualizer = visualizer;
            FinishInitialization();
        }

        private void FinishInitialization()
        {
            //this.render += LinkManager;
            ContentRendered += LinkManager;
            //ReadyToInitialize += LinkManager;
        }

        public event EventHandler FinishedInitialization;

        public void LinkManager(object sender, EventArgs e)
        {
            GraphManagerInterface = core.Initialize(Visualizer);
            manager = new GraphManager(GraphManagerInterface, Graphs);

            manager.Initialize();
            OnFinishedInitialization(EventArgs.Empty);
            AlreadyFinishedInitialization = true;
        }

        public bool AlreadyFinishedInitialization { get; private set; } = false;

        protected virtual void OnFinishedInitialization(EventArgs e)
        {
            FinishedInitialization?.Invoke(this, e);
        }

        virtual public void UpdateTime(double time)
        { }

        // To keep time
        private readonly Stopwatch timer = new Stopwatch();

        // For multithreading communications
        private readonly BufferBlock<PackagedCommands<TVisualizer>> turnBuffer
            = new BufferBlock<PackagedCommands<TVisualizer>>();

        /// <summary>
        /// Gets and sets whether the simulation is running
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return timer.IsRunning;
            }
            set
            {
                if (value)
                    timer.Start();
                else
                    timer.Stop();
            }
        }

        /// <summary>
        /// The maximum buffer size - otherwise you get memory overruns
        /// </summary>
        virtual public int MaxBufferSize { get; set; } = 500;
        virtual public int SizeToResume { get; set; } = 300;

        virtual public double DisplayTime { get; private set; } = 0;

        virtual public double TimeIncrement { get; set; } = 0;

        private double time = 0;

        public void SetIntitialTime(double timeValue)
        {
            time = timeValue;
        }

        /// <summary>
        /// Start or continue the engine running
        /// </summary>
        private void RunEngine()
        {
            while (IsRunning)
            {
                time += TimeIncrement;

                var package = core.NextCommand(time);

                // Stop in an empty loop until the turnBuffer has dropped down in size.
                // Otherwise you get memory overruns
                if (turnBuffer.Count > MaxBufferSize)
                {
                    while (turnBuffer.Count > SizeToResume)
                    {
                        // This will break out if things are paused
                        // Otherwise it runs forever
                        if (!IsRunning)
                            return;
                    }
                }
                // Send to the buffer
                if (package != null)
                {
                    turnBuffer.Post(package);
                }

                if (!core.Continue)
                {
                    turnBuffer.Complete();
                    break;
                }
            }
        }

        /// <summary>
        /// This is the time I need to flush the visual memory
        /// </summary>
        virtual public double FlushTime { get; set; } = 1;

        virtual public double TimeScale { get; set; } = 1;

        virtual public bool SlowDraw { get; set; } = false;

        virtual public bool FastDraw { get; set; } = false;

        /// <summary>
        /// Updates visualization
        /// </summary>
        private async Task UpdateVisualAsync()
        {
            double timeOfLastDraw = 0;
            while (await turnBuffer.OutputAvailableAsync() && IsRunning)
            {
                var turn = turnBuffer.Receive();
                // Process commands
                turn.Commands.ProcessAll(Visualizer);
                // Redraw screen
                //InvalidateVisual();

                //Update time display
                DisplayTime = turn.Time;
                UpdateTime(DisplayTime);

                // Update graphs
                Graphs.Update(turn.Data);

                if (!FastDraw)
                {
                    //Check if delay is needed
                    var timeDiff = DisplayTime / TimeScale - timer.Elapsed.TotalSeconds;
                    if (timeDiff > 0)
                    {
                        // This delays it so the clocks will line up
                        int delay = (int)(timeDiff * 1000); // Convert to milliseconds
                        await Task.Delay(delay);
                    }
                }

                double timeSinceLastDraw = timer.Elapsed.TotalSeconds - timeOfLastDraw;
                if (timeSinceLastDraw > FlushTime || SlowDraw)
                {
                    InvalidateVisual();
                    WaitForDrawing();
                    timeOfLastDraw = timer.Elapsed.TotalSeconds;
                }
            }
        }

        /// <summary>
        /// Waits until all drawing is done - needed to keep the visuals displaying sometimes
        /// </summary>
        private void WaitForDrawing()
        {
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
        }

        private Task engineTask;
        private Task visualizerTask;

        /// <summary>
        /// Starts all tasks
        /// </summary>
        public void StartAll()
        {
            visualizerTask = UpdateVisualAsync();
            engineTask = Task.Run(() => RunEngine());
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsRunning = false;
        }

    }
}
