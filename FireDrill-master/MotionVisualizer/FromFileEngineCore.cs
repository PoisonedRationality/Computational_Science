using GraphData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VisualizerBaseClasses;

namespace MotionVisualizer
{
    internal class FromFileEngineCore<TVisualizer, TCommand> : EngineCore<TVisualizer, TCommand>
        where TCommand : ICommand<TVisualizer>
    {
        private BinaryReader br;
        private FileGraphDataInterface graphInterface;
        private ICommandFileReader<TVisualizer> factory;
        public FromFileEngineCore(string filename, ICommandFileReader<TVisualizer> factory)
        {
            br = new BinaryReader(File.OpenRead(filename));
            this.factory = factory;
        }

        private bool shouldContinue = true;
        public override bool Continue => shouldContinue;

        public override IGraphDataInterface Initialize(TVisualizer visualizer)
        {
            var initialSet = new CommandSet<TVisualizer>(br, factory);
            initialSet.ProcessAll(visualizer);

            graphInterface = new FileGraphDataInterface(br);
            return graphInterface;
        }

        public override PackagedCommands<TVisualizer> NextCommand(double newTime)
        {
            if (DongUtility.FileUtilities.IsEndOfFile(br))
            {
                return null;
            }
            var commands = new CommandSet<TVisualizer>(br, factory);
            var graphData = graphInterface.GetData();
            double time = br.ReadDouble();
            shouldContinue = br.ReadBoolean();
            return new PackagedCommands<TVisualizer>(commands, graphData, time);
        }
    }
}
