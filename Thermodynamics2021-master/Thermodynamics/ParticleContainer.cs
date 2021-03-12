using DongUtility;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Thermodynamics
{
    /// <summary>
    /// A container class for holding gas particles in a cubical container
    /// </summary>
    public class ParticleContainer
    {
        /// <summary>
        /// The size of the container
        /// Each component of the vector is the size in that dimension
        /// </summary>
        public Vector Size { get; set; }
        /// <summary>
        /// All the particles in the container
        /// </summary>
        public List<Particle> Particles { get; } = new List<Particle>();

        /// <summary>
        /// A dictionary to keep track of all the particle types
        /// </summary>
        public ParticleDictionary Dictionary { get; } = new ParticleDictionary();

        /// <summary>
        /// Access to the random generator
        /// </summary>
        static protected Random Random { get { return RandomGenerator.RandomGen; } }

        public double TotalForce { get; set; } = 0;

        public double initialPressure { get; set; }

        public double Pressure => TotalForce / (Size.X * Size.X * 6);

        public double Time { get; set; } = 0;

        public ParticleContainer(double xSize, double ySize, double zSize)
        {
            Size = new Vector(xSize, ySize, zSize);
        }

        public ParticleContainer(double size) :
            this(size, size, size)
        { }

        public static double Count = 0;

        public void RegisterParticleType(string name, double mass, Color color, Shapes.Shapes3D shape = Shapes.Shapes3D.Sphere)
        {
            Dictionary.AddParticle(new ParticleInfo(name, mass, color, shape));
        }

        public List<Particle> ParticlesToAdd { get; } = new List<Particle>();
        public List<Particle> ParticlesToRemove { get; } = new List<Particle>();

        public void AddParticle(Particle part)
        {
            ParticlesToAdd.Add(part);
        }

        public void RemoveParticle(Particle part)
        {
            ParticlesToRemove.Add(part);
        }

        /// <summary>
        /// Adds a particle directly
        /// Not for general use; hence, it is protected
        /// </summary>
        protected virtual void AddParticleDirectly(Particle part)
        {
            Particles.Add(part);
        }

        /// <summary>
        /// Removes a particle directly
        /// Not for general use; hence, it is protected
        /// </summary>
        /// <param name="part"></param>
        protected virtual void RemoveParticleDirectly(Particle part)
        {
            Particles.Remove(part);
        }

        public double GetTemperature()
        {
            double temperature;
            double totalKE = 0;
            foreach (Particle particle in this.Particles)
            {
                totalKE += particle.KineticEnergy;
            }
            //T = (2/3)*KE/k_b
            temperature = (2 * totalKE / this.Particles.Count) / (3 * Constants.BoltzmannConstant);
            
            return temperature;
        }

        /// <summary>
        /// Gets the total number of particles of a given name
        /// </summary>
        public int GetNParticles(string name)
        {
            int total = 0;

            foreach (var part in Particles)
            {
                if (part.Info.Name == name)
                {
                    ++total;
                }
            }

            return total;
        }

        /// <summary>
        /// Adds a number of particles at random
        /// </summary>
        /// <param name="generator">The random generator to use</param>
        /// <param name="name">The name of the particle type</param>
        /// <param name="number">The number of particles to add</param>
        public void AddRandomParticles(RandomGenerator generator, string name, int number)
        {
            for (int i = 0; i < number; ++i)
            {
                AddParticleDirectly(generator.GetRandomParticle(name));
            }
        }

        /// <summary>
        /// Updates all particles for a given time increment
        /// </summary>
        public virtual void Update(double deltaTime)
        {
            TotalForce = 0;
            ParticlesToAdd.Clear();
            ParticlesToRemove.Clear();

            Setup();

            foreach (Particle part in Particles)
            {
                part.Update(deltaTime);
                CheckParticle(part);
                ParticleUpdate(part);
            }

            if (Count == 0)
            {
                initialPressure = Pressure;
                Count++;
            }
            else
            {
                Vector vec = Size * 1e-3;
                while (Math.Abs(initialPressure - Pressure) > 0.01*initialPressure)
                {
                    if (initialPressure > Pressure)
                    {
                        Size -= vec;
                    }
                    else
                    {
                        Size += vec;
                    }
                }

            }

            ParticlesToAdd.ForEach((x) => AddParticleDirectly(x));
            ParticlesToRemove.ForEach((x) => RemoveParticleDirectly(x));

            Time += deltaTime;

        }

        protected virtual void ParticleUpdate(Particle part)
        { }
        

        protected virtual void Setup()
        { }

        /// <summary>
        /// A function that extracts a specific property of a particle
        /// </summary>
        public delegate double ParticleFunction(Particle part);

        /// <summary>
        /// Get a list of values for a given property of all particles
        /// Useful for histograms
        /// </summary>
        public List<double> GetParticlePropertyList(ParticleFunction func)
        {
            var response = new List<double>();

            foreach (var part in Particles)
            {
                response.Add(func(part));
            }

            return response;
        }

        /// <summary>
        /// Make sure the particle lies within the bounds of the box
        /// Reflect it back if it is not
        /// </summary>
        protected virtual void CheckParticle(Particle particle)
        {
            Vector newVec = particle.Position;
            double dt = 0.01;
            Vector newMomentum = particle.Momentum;
            Vector lastMomentum;
            if (particle.Position.X < 0 || particle.Position.X > Size.X)
            {
                particle.Velocity = new Vector(-particle.Velocity.X, particle.Velocity.Y, particle.Velocity.Z);
               
                if (particle.Position.X < 0)
                {
                    newVec.X = 0;
                }
                else if (particle.Position.X > Size.X)
                {
                    newVec.X = Size.X;
                }
            }
            if (particle.Position.Y < 0 || particle.Position.Y > Size.Y)
            {
                particle.Velocity = new Vector(particle.Velocity.X, -particle.Velocity.Y, particle.Velocity.Z);
                if (particle.Position.Y < 0)
                {
                    newVec.Y = 0;
                }
                else if (particle.Position.Y > Size.Y)
                {
                    newVec.Y = Size.Y;
                }
            }
            if (particle.Position.Z < 0 || particle.Position.Z > Size.Z)
            {
                particle.Velocity = new Vector(particle.Velocity.X, particle.Velocity.Y, -particle.Velocity.Z);
                if (particle.Position.Z < 0)
                {
                    newVec.Z = 0;
                }
                else if (particle.Position.Z > Size.Z)
                {
                    newVec.Z = Size.Z;
                }
            }

            lastMomentum = newMomentum;
            newMomentum = particle.Momentum;

            particle.Velocity += particle.Velocity.UnitVector();
            TotalForce += (newMomentum - lastMomentum).Magnitude / dt;
            
            particle.Position = newVec;
        }
    }
}
