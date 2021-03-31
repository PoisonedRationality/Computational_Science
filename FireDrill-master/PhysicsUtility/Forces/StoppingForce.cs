using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DongUtility;

namespace PhysicsUtility
{
    /// <summary>
    /// A force which stops a projectile entirely
    /// </summary>
    abstract public class StoppingForce : GlobalForce
    {
        private Vector finalPosition = Vector.NullVector();
        private KinematicsEngine engine;

        private Vector forceLastTime = Vector.NullVector();
        private double formerTime = 0;

        public StoppingForce(KinematicsEngine engine) :
            base(engine)
        {
            this.engine = engine;
        }

        /// <summary>
        /// The condition that must be met for the force to "turn on"
        /// Otherwise, nothing happens
        /// </summary>
        abstract protected bool ConditionMet(Projectile projectile);

        override protected Vector GetForce(Projectile projectile)
        {
            Vector response = Vector.NullVector();

            if (engine.Time > 0 && ConditionMet(projectile))
            {
                double timeIncrement = engine.Time - formerTime;

                // Adjust for velocity
                Vector counterAcc = projectile.Velocity / (-timeIncrement);
                Vector counterForce1 = counterAcc * projectile.Mass;

                // Adjust for other forces
                Vector counterForce2 = forceLastTime - projectile.Acceleration * projectile.Mass;

                response = counterForce1 + counterForce2;
            }

            formerTime = engine.Time;
            forceLastTime = response;
            return response;
        }
    }
}
