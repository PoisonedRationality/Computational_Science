using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsUtility
{
    /// <summary>
    /// A class to see if the engine should stop
    /// </summary>
    abstract public class StopCondition
    {
        /// <summary>
        /// A function that detects whether the engine should continue
        /// </summary>
        /// <returns>True if the engine should continue running</returns>
        abstract public bool ShouldContinue(KinematicsEngine engine);
    }
}
