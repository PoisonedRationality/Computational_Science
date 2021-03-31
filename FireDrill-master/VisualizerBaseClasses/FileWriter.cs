using GraphData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace VisualizerBaseClasses
{
    public class FileWriter<TVisualizer, TCommand, TEngine>
        where TCommand : ICommand<TVisualizer>
        where TEngine : IEngine<TVisualizer, TCommand>
    {
        public FileWriter(TEngine engine)
        {
            this.engine = engine;
        }

        private TEngine engine;
        public GraphDataManager Manager { get; } = new GraphDataManager();

        public void Run(string filename, double timeStep, double maxTime = double.MaxValue, double messageEvery = double.MaxValue)
        {
            using var bw = new BinaryWriter(File.OpenWrite(filename));

            var initialSet = engine.Initialization();
            initialSet.WriteToFile(bw);
            Manager.WriteGraphHeader(bw);

            double nextMessageTime = messageEvery;
            while (engine.Continue && engine.Time < maxTime)
            {
                if (engine.Time > nextMessageTime)
                {
                    Console.WriteLine("Reached time " + engine.Time);
                    nextMessageTime += messageEvery;
                }
                double newTime = engine.Time + timeStep;
                var commands = engine.Tick(newTime);
                var data = Manager.GetData();
                commands.WriteToFile(bw);
                data.WriteData(bw);
                bw.Write(newTime);
                // A bit on whether to continue
                bw.Write(engine.Continue);
            }
        }
    }
}
