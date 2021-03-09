using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DongUtility;

namespace Thermodynamics
{
    /// <summary>
    /// A random generator that generates particles with a flat distribution between two endpoints
    /// </summary>
    public class FlatGenerator : RandomGenerator
    {
        private readonly double min;
        private readonly double max;

        /// <param name="min">The minimum possible speed</param>
        /// <param name="max">The maximum possible speed</param>
        public FlatGenerator(ParticleContainer cont, double min, double max) :
            base(cont)
        {
            this.min = min;
            this.max = max;
        }

        override protected double GetSpeed(ParticleInfo info)
        {
            return RandomGen.NextDouble(min, max);
        }
    }
}
