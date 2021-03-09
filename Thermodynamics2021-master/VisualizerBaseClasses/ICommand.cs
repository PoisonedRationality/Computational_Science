using System;
using System.IO;

namespace VisualizerBaseClasses
{
    public interface ICommand<TVisualizer>
    {
        /// <summary>
        /// This executes the command
        /// </summary>
        /// <param name="viz">The visualizer that receives the command</param>
        public void Do(TVisualizer viz);

        public void WriteToFile(BinaryWriter bw);
    }
}
