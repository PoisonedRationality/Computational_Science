using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thermodynamics;
using DongUtility;

namespace Thermodynamics
{
    public class ReactingParticleContainer : ParticleContainerGrid
    {
        protected double CollisionRadius { get; private set; }

        public ReactingParticleContainer(double xSize, double ySize, double zSize, double collisionRadius, double gridSize) :
            base(xSize, ySize, zSize, GetGridSize(xSize, gridSize), GetGridSize(ySize, gridSize), GetGridSize(zSize, gridSize))
        {
            CollisionRadius = collisionRadius;
        }

        public ReactingParticleContainer(double side, double collisionRadius) :
            base(side, collisionRadius)
        {
            CollisionRadius = collisionRadius;
        }

        static private int GetGridSize(double size, double rad)
        {
            return (int)Math.Ceiling(size / rad);
        }

        public double GetTemperature()
        {
            double sumKE = 0;
            foreach (var part in Particles)
            {
                double speed = part.Velocity.Magnitude;
                sumKE += .5 * part.Info.Mass * speed * speed;
            }

            return sumKE / Particles.Count * 2.0 / 3.0 / Constants.BoltzmannConstant;
        }

        public void AddParticle(string name, Vector position, Vector velocity)
        {
            ParticlesToAdd.Add(Dictionary.MakeParticle(position, velocity, name));
        }

        public void AddRandomParticles(RandomGenerator generator, int number, String name)
        {
            for (int i = 0; i < number; ++i)
            {
                AddParticleDirectly(generator.GetRandomParticle(name));
            }
        }

        private void CheckCollisions(Particle particle)
        {
            if (ParticlesToRemove.Contains(particle))
            {
                return;
            }
            var particles = GetNearbyParticles(particle, CollisionRadius);
            var particleList = new List<Particle>();
            foreach (var part in particles)
            {
                if (!ParticlesToRemove.Contains(part))
                    particleList.Add(part);
            }

            if (particleList.Count > 1)
            {
                React(particle, particleList);
            }
        }

        protected virtual void React(Particle particle, List<Particle> nearby)
        {
            // Here the particle is the primary particle, and nearby is a list of all
            // nearby particles.  This determines what happens when you react two particles

        }

        public bool CheckForCollisions { get; set; } = true;

        //protected override void Setup()
        //{
        //    UpdateParticleList();
        //}

        protected override void ParticleUpdate(Particle part)
        {
            CheckCollisions(part);
        }
    }
}
