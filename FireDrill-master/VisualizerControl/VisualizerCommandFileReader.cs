using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VisualizerBaseClasses;

namespace VisualizerControl
{
    public class VisualizerCommandFileReader : ICommandFileReader<Visualizer>
    {
        public ICommand<Visualizer> ReadCommand(BinaryReader br)
        {
            return VisualizerCommand.ReadFromFile(br);
        }
    }
}
