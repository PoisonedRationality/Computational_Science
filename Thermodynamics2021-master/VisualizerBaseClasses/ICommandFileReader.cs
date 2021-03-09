using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VisualizerBaseClasses
{
    public interface ICommandFileReader<TVisualizer>
    {
        public ICommand<TVisualizer> ReadCommand(BinaryReader br);
    }
}
