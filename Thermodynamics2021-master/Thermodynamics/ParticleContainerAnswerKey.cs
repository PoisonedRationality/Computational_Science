using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thermodynamics;

namespace Thermodynamics
{
    public class ParticleContainerAnswerKey : ReactingParticleContainer
    {
        private IList<Reaction> reactions = new List<Reaction>();

        public ParticleContainerAnswerKey(double side, double reactionRadius) :
            base(side, reactionRadius)
        { }

        public void AddReaction(Reaction react)
        {
            reactions.Add(react);
        }

        protected override void React(Particle mainParticle, List<Particle> potentialReactants)
        {
            IList<Reaction> working = new List<Reaction>();

            foreach (Reaction react in reactions)
            {
                if (react.ItWorks(mainParticle, potentialReactants))
                {
                    working.Add(react);
                }
            }

            if (working.Count == 0)
            {
                return;
            }

            int index = Random.Next(working.Count);
            working[index].DoReaction(mainParticle, potentialReactants, this);
        }
    }
}
