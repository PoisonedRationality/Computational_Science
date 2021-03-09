using DongUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thermodynamics
{
    public class InteractingParticleContainer : ReactingParticleContainer
    {
        private readonly double reactionRad;

        public InteractingParticleContainer(double side, double gridSize, double reactionRad) :
            base(side, side, side, reactionRad, NDiv(side, gridSize))
        {
            this.reactionRad = reactionRad;
        }
        
        static private int NDiv(double size, double gridSize)
        {
            return (int)Math.Round(size / gridSize);
        }

        private List<Particle> alreadyInteracted = new List<Particle>();

        protected override void Setup()
        {
            alreadyInteracted.Clear();
        }

        protected override void ParticleUpdate(Particle part)
        {
            foreach (var other in GetNearbyParticles(part, reactionRad))
            {
                if (alreadyInteracted.Contains(part))
                    continue;
                Collide(part, other);
            }

            alreadyInteracted.Add(part);

            base.ParticleUpdate(part);
        }

        private void Collide(Particle p1, Particle p2)
        {
            Vector direction = (p1.Position - p2.Position).UnitVector();
            Vector difference = p2.Momentum - p1.Momentum;
            double deltaPMagnitude = Vector.Dot(difference, direction);
            Vector deltaP = direction * deltaPMagnitude;

            p1.Momentum += deltaP;
            p2.Momentum -= deltaP;
        }
    }
}
