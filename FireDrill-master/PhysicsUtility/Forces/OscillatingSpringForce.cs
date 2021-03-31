using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DongUtility;

namespace PhysicsUtility
{
    /// <summary>
    /// A spring force for which the end is oscillating sinusoidally
    /// </summary>
    public class OscillatingSpringForce : SpringForce
    {
        private Vector origin;
        private Vector amplitude;
        private readonly double frequency; 
        private KinematicsEngine engine;

        public OscillatingSpringForce(Projectile projectile, double springConstant, Vector origin, double unstretchedLength, Vector amplitude, double frequency,
            KinematicsEngine engine) :
            base (projectile, springConstant, unstretchedLength)
        {
            this.origin = origin;
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.engine = engine;
        }

        protected override Vector SpringPosition()
        {
            return origin + Math.Sin(2 * Math.PI * frequency * engine.Time) * amplitude;
        }
    }
}
