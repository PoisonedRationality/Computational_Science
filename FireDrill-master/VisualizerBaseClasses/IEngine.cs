using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizerBaseClasses
{
    /// <summary>
    /// Provides an interface for a kinematics engine or world class to work with the visualizer.
    /// This class should keep track of projectiles and forces and do all updates when told to.
    /// </summary>
    public interface IEngine<TVisualizer, TCommand> where TCommand : ICommand<TVisualizer>
    {
        /// <summary>
        /// Moves the time in the engine to newTime and updates all projectiles accordingly.
        /// </summary>
        /// <param name="newTime">The new time the engine will move to</param>
        /// <returns>The visualization commands that result from this operation</returns>
        CommandSet<TVisualizer> Tick(double newTime);

        /// <summary>
        /// Commands to set up the initial visualization (before anything starts)
        /// </summary>
        /// <returns></returns>
        CommandSet<TVisualizer> Initialization();

        /// <summary>
        /// A simple property to check whether the simulation should continue
        /// </summary>
        bool Continue { get; }

        double Time { get; }
    }
}
