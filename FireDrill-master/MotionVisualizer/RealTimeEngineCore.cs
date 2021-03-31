using GraphControl;
using GraphData;
using System;
using System.Collections.Generic;
using System.Text;
using VisualizerBaseClasses;

namespace MotionVisualizer
{
    internal class RealTimeEngineCore<TVisualizer, TCommand> : EngineCore<TVisualizer, TCommand>
        where TCommand : ICommand<TVisualizer>
    {
        private IEngine<TVisualizer, TCommand> engine;
        private GraphDataManager manager;
        private RealTimeGraphDataInterface graphInterface;

        public RealTimeEngineCore(IEngine<TVisualizer, TCommand> engine, GraphDataManager manager)
        {
            this.engine = engine;
            this.manager = manager;
        }

        public override bool Continue => engine.Continue;

        public override IGraphDataInterface Initialize(TVisualizer visualizer)
        {
            engine.Initialization().ProcessAll(visualizer);
            graphInterface = new RealTimeGraphDataInterface(manager);
            return graphInterface;
        }

        public override PackagedCommands<TVisualizer> NextCommand(double newTime)
        {
            var commands = engine.Tick(newTime);
            var data = graphInterface.GetData();
            return new PackagedCommands<TVisualizer>(commands, data, newTime);
        }
    }
}
