using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DongUtility;

namespace Thermodynamics
{
    /// <summary>
    /// A generator used to create random particles in a specified way
    /// </summary>
    abstract public class RandomGenerator
    {
        private ParticleContainer cont;

        /// <param name="cont">The particle container that will hold the generated particles</param>
        public RandomGenerator(ParticleContainer cont)
        {
            this.cont = cont;
        }

        /// <summary>
        /// The static random number generator, internal so it can be synchronized across all classes
        /// </summary>
        static internal protected Random RandomGen { get; } = new Random();

        /// <summary>
        /// Choose a random position inside the container
        /// </summary>
        static protected Vector RandomPosition(ParticleContainer grid)
        {
            double x = RandomGen.NextDouble() * grid.Size.X;
            double y = RandomGen.NextDouble() * grid.Size.Y;
            double z = RandomGen.NextDouble() * grid.Size.Z;

            return new Vector(x, y, z);
        }

        /// <summary>
        /// Choose a speed at random for a new particle
        /// </summary>
        abstract protected double GetSpeed(ParticleInfo info);

        /// <summary>
        /// Creates a new random particle of a given type 
        /// </summary>
        /// <param name="name">The name, as a string, of the particle type</param>
        public Particle GetRandomParticle(string name)
        {
            var particleType = cont.Dictionary.Map[name];
            var speed = GetSpeed(particleType);
            Vector velocity = Vector.RandomDirection(speed, RandomGen);
            Vector position = RandomPosition(cont);

            return cont.Dictionary.MakeParticle(position, velocity, name);
        }
    }
}
