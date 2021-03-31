using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsUtility
{
    /// <summary>
    /// A condition that stops the simulation if any projectile is on the ground or below
    /// </summary>
    public class HitGroundStopCondition : StopCondition
    {
        public override bool ShouldContinue(KinematicsEngine engine)
        {
            foreach (var proj in engine.Projectiles)
            {
                if (proj.Position.Z < 0 || (proj.Position.Z == 0 && engine.Time > 0))
                    return false;
            }
            return true;
        }
    }
}
