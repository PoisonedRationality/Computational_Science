using System;

namespace PhysicsUtility
{
    /// <summary>
    /// A stopping force that just looks for z > 0
    /// </summary>
    public class GroundStoppingForce : StoppingForce
    {

        public GroundStoppingForce(KinematicsEngine engine) : base(engine)
        {
        }

        protected override bool ConditionMet(Projectile projectile)
        {
            return projectile.Position.Z <= 0 && projectile.Velocity.Z <= 0;
        }
    }
}