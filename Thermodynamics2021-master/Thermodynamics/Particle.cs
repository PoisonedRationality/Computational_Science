using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DongUtility;

namespace Thermodynamics
{
    /// <summary>
    /// A single particle in the simulation
    /// </summary>
    public class Particle
    {
        public Vector Position { get; set; }
        public Vector Velocity { get; set; }
        /// <summary>
        /// Information about the particle type that is the same for all particles of that type
        /// </summary>
        public ParticleInfo Info { get; set; }

        public Vector Momentum
        {
            get
            {
                return Info.Mass * Velocity;
            }
            set
            {
                Velocity = value / Info.Mass;
            }
        }

        public double KineticEnergy
        {
            get
            {
                return .5 * Info.Mass * Velocity.MagnitudeSquared;
            }
            set
            {
                Velocity = Velocity.UnitVector() * Math.Sqrt(2 * value / Info.Mass);
            }
        }

        public Particle(Vector position, Vector velocity, ParticleInfo info)
        {
            Position = position;
            Velocity = velocity;
            Info = info;
        }

        /// <summary>
        /// Change the particle position for a given time step
        /// </summary>
        virtual public void Update(double timeIncrement)
        {
            Position += Velocity * timeIncrement;
        }

    }
}
