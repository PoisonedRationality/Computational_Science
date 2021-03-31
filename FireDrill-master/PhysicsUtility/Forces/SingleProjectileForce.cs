using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DongUtility;

namespace PhysicsUtility
{
    /// <summary>
    /// A force which affects only a single projectile
    /// </summary>
    abstract public class SingleProjectileForce : Force
    {
        protected Projectile Particle { get; }

        public SingleProjectileForce(Projectile particle)
        {
            Particle = particle;
        }

        /// <summary>
        /// Returns the actual force on the particle
        /// </summary>
        abstract protected Vector GetForce();

        public override void AddForce()
        {
            Particle.AddForce(GetForce());
        }
    }
}
