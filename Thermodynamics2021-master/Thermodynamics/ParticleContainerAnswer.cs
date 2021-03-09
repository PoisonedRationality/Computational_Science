using DongUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thermodynamics
{
    public class ParticleContainerAnswer : ParticleContainer
    {
        public ParticleContainerAnswer(double size) :
            base(size, size, size)
        { }

        public double Temperature
        {
            get
            {
                double average = Particles.Average((x) => x.KineticEnergy);
                return 2.0 / 3 / Constants.BoltzmannConstant * average;
            }
        }

        public double Volume => Size.X * Size.Y * Size.Z;

        private double deltaTime;

        public double Pressure { get; private set; }

        private double initialPressure = -1;

        /// <summary>
        /// Amount to increase temperature by, as a fraction of the velocity of the particle hitting the wall
        /// Setting this to 1 means no change (the default)
        /// </summary>
        public double IncreaseTemperature { get; set; } = 1;

        public bool ConstantPressure { get; set; } = false;
        public double VolumeAdjustmentFactor { get; set; } = 1;

        public override void Update(double deltaTime)
        {
            Pressure = 0;
            this.deltaTime = deltaTime;
            base.Update(deltaTime);
            if (initialPressure == -1)
                initialPressure = Pressure;
            else
                AdjustForPressure();
        }

        private void AdjustForPressure()
        {
            if (Pressure > initialPressure)
                Size *= VolumeAdjustmentFactor;
            else
                Size /= VolumeAdjustmentFactor;
        }

        protected override void CheckParticle(Particle particle)
        {
            Vector oldVelocity = particle.Velocity;
            Vector newVec = particle.Position;
            if (particle.Position.X < 0 || particle.Position.X > Size.X)
            {
                particle.Velocity = new Vector(-particle.Velocity.X * IncreaseTemperature, particle.Velocity.Y, particle.Velocity.Z);
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
                particle.Velocity = new Vector(particle.Velocity.X, -particle.Velocity.Y * IncreaseTemperature, particle.Velocity.Z);
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
                particle.Velocity = new Vector(particle.Velocity.X, particle.Velocity.Y, -particle.Velocity.Z * IncreaseTemperature);
                if (particle.Position.Z < 0)
                {
                    newVec.Z = 0;
                }
                else if (particle.Position.Z > Size.Z)
                {
                    newVec.Z = Size.Z;
                }
            }
            particle.Position = newVec;

            Vector difference = particle.Velocity - oldVelocity;
            Vector deltaP = particle.Info.Mass * difference;
            Vector force = deltaP / deltaTime;

            double forceMag = force.Magnitude;
            double area = Size.X * Size.Y; // Assumes it is a cube, guaranteed by constructor
            double pressure = forceMag / area;
            Pressure += pressure;
        }
    }
}
