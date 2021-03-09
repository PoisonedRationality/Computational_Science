using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DongUtility;

namespace Thermodynamics
{
    /// <summary>
    /// A class to keep track of all the different particle types
    /// </summary>
    public class ParticleDictionary
    {
        /// <summary>
        /// A map from names to ParticleInfo objects, for convenience
        /// </summary>
        public IDictionary<string, ParticleInfo> Map { get; } = new Dictionary<string, ParticleInfo>();

        /// <summary>
        /// Adds a particle type to the dictionary
        /// </summary>
        public void AddParticle(ParticleInfo info)
        {
            Map.Add(info.Name, info);
        }

        /// <summary>
        /// Create a new particle by name
        /// </summary>
        /// <param name="name">The name of the particle type to be created, as a string</param>
        public Particle MakeParticle(Vector position, Vector velocity, string name)
        {
            ParticleInfo info = Map[name];

            return new Particle(position, velocity, info);
        }
    }
}
