using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DongUtility;

namespace Thermodynamics
{
    /// <summary>
    /// A class holding general info about a particular type of particle
    /// </summary>
    public class ParticleInfo
    {
        /// <param name="name">The name of the particle type</param>
        /// <param name="mass">The mass of the particle, in kg</param>
        /// <param name="color">The color of the particle for display</param>
        public ParticleInfo(string name, double mass, Color color, Shapes.Shapes3D shape = Shapes.Shapes3D.Sphere)
        {
            Name = name;
            Mass = mass;
            Color = color;
            Shape = shape;
        }

        public string Name { get; set; }
        public double Mass { get; set; }
        public Color Color { get; set; }
        public Shapes.Shapes3D Shape { get; set; }
    }
}
