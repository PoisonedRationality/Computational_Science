using GraphData;
using System;
using System.Collections.Generic;
using System.Text;
using VisualizerBaseClasses;

namespace MotionVisualizer
{
    /// <summary>
    /// Includes all the information needed by the visualizer piece that the engine will pass it
    /// </summary>
    internal class PackagedCommands<TVisualizer>
    {

        public PackagedCommands(CommandSet<TVisualizer> commands, GraphDataPacket data, double time)
        {
            Commands = commands;
            Data = data;
            Time = time;
        }

        /// <summary>
        /// The command set
        /// </summary>
        public CommandSet<TVisualizer> Commands { get; set; }
        /// <summary>
        /// Data, for graphing
        /// </summary>
        public GraphDataPacket Data { get; set; }
        /// <summary>
        /// The time of the commands
        /// </summary>
        public double Time { get; set; }
    }
}
