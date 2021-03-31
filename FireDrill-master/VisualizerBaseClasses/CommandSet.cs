using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizerBaseClasses
{
    /// <summary>
    /// A collection of VisualizerCommands representing one "turn" for the visualizer
    /// </summary>
    public class CommandSet<TVisualizer>
    {
        public List<ICommand<TVisualizer>> Commands { get; } = new List<ICommand<TVisualizer>>();

        public CommandSet()
        { }

        /// <summary>
        /// Adds a new command to the CommandSet
        /// </summary>
        public void AddCommand(ICommand<TVisualizer> command) => Commands.Add(command);

        /// <summary>
        /// Executes all the commands in the set, in order
        /// </summary>
        /// <param name="visualizer">The Visualizer that is receiving the commands</param>
        public void ProcessAll(TVisualizer visualizer)
        {
            foreach (var command in Commands)
            {
                command.Do(visualizer);
            }
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(Commands.Count);
            foreach (var command in Commands)
            {
                command.WriteToFile(bw);
            }
        }

        public CommandSet(BinaryReader br, ICommandFileReader<TVisualizer> factory)
        {

            int nCommands = br.ReadInt32();
            for (int i = 0; i < nCommands; ++i)
            {
                var newCommand = factory.ReadCommand(br);
                Commands.Add(newCommand);
            }
        }

        static public CommandSet<TVisualizer> operator +(CommandSet<TVisualizer> one,
            CommandSet<TVisualizer> two)
        {
            var response = new CommandSet<TVisualizer>();
            response.Commands.AddRange(one.Commands);
            response.Commands.AddRange(two.Commands);
            return response;
        }
    }
}
