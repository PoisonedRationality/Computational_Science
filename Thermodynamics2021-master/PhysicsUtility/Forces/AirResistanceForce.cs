using System.Collections.Generic;
using DongUtility;

namespace PhysicsUtility
{
    /// <summary>
    /// A force for a simple C*v^2 dependence for air resistance
    /// </summary>
    public class AirResistanceForce : GlobalForce
    {
        private readonly double coefficient;

        /// <param name="coefficient">The coefficient of air resistance, including drag coefficient, density of air, and cross-sectional area</param>
        public AirResistanceForce(KinematicsEngine engine, double coefficient) :
            base(engine)
        {
            this.coefficient = coefficient;
        }

        override protected Vector GetForce(Projectile proj)
        {
            return proj.Velocity.UnitVector() * (-coefficient * proj.Velocity.MagnitudeSquared);
        }

    }
}
