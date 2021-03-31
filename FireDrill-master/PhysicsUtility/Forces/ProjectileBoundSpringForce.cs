using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DongUtility;

namespace PhysicsUtility
{
    /// <summary>
    /// A spring force for which the other end of the spring is connected to another projectile
    /// Note that this needs to be set on both projectiles for Newton's third law to work properly
    /// </summary>
    public class ProjectileBoundSpringForce : SpringForce
    {
        private Projectile other;

        public ProjectileBoundSpringForce(Projectile projectile1, Projectile projectile2, double springConstant,
            double unstretchedLength = 0) :
            base (projectile1, springConstant, unstretchedLength)
        {
            other = projectile2;
        }

        protected override Vector SpringPosition()
        {
            return other.Position;
        }
    }
}
