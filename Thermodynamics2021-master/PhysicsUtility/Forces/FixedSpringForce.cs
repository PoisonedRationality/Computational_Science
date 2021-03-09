using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DongUtility;

namespace PhysicsUtility
{
    /// <summary>
    /// A spring force for which one end is fixed at a given position
    /// </summary>
    public class FixedSpringForce : SpringForce
    {
        private Vector position;

        public FixedSpringForce(Projectile projectile, double springConstant, Vector position, double unstretchedLength = 0) :
            base (projectile, springConstant, unstretchedLength)
        {
            this.position = position;
        }

        protected override Vector SpringPosition()
        {
            return position;
        }
    }
}
