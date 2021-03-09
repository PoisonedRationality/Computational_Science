using DongUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thermodynamics;

namespace Thermodynamics
{
    public class Reaction
    {

        private readonly IDictionary<string, int> inputs = new Dictionary<string, int>();
        private readonly IDictionary<string, int> outputs = new Dictionary<string, int>();
        private readonly double kineticBarrier;

        private string catalyst = null;
        private double catalystBarrier = 1;

        private static readonly Random random = new Random();

        public Reaction(String reaction, double kineticBarrier = 1)
        {
            this.kineticBarrier = kineticBarrier;

            string[] tokens = reaction.Split(" ".ToCharArray());

            bool reactants = true;
            foreach (string theString in tokens)
            {
                string str = theString;
                str = str.Trim();
                if (str.Equals("->"))
                {
                    reactants = false;
                    continue;
                }
                else if (str.Equals("+"))
                {
                    continue;
                }

                int index = 0;
                while (char.IsDigit(str[index]))
                {
                    ++index;
                }

                string chemical;
                int number = 1;
                if (index != 0)
                {
                    string numString = str.Substring(0, index);
                    number = int.Parse(numString);
                    chemical = str.Substring(index);
                }
                else
                {
                    chemical = str;
                }

                if (reactants)
                {
                    AddInput(chemical, number);
                }
                else
                {
                    AddOutput(chemical, number);
                }
            }
        }

        public void AddInput(string input, int number)
        {
            inputs.Add(input, number);
        }

        public void AddOutput(string output, int number)
        {
            outputs.Add(output, number);
        }

        public void AddCatalyst(string catalyst, double barrier)
        {
            this.catalyst = catalyst;
            catalystBarrier = barrier;
        }

        public bool ItWorks(Particle mainParticle, IList<Particle> potentialReactants)
        {
            if (!inputs.ContainsKey(mainParticle.Info.Name))
            {
                return false;
            }

            IDictionary<string, int> choices = new Dictionary<string, int>();
            bool hasCatalyst = false;

            choices.Add(mainParticle.Info.Name, 1);

            foreach (Particle part in potentialReactants)
            {
                if (choices.ContainsKey(part.Info.Name))
                {
                    choices[part.Info.Name]++;
                }
                else
                {
                    choices.Add(part.Info.Name, 1);
                }
                if (part.Info.Name == catalyst)
                {
                    hasCatalyst = true;
                }
            }

            foreach (var entry in inputs)
            {
                if (!choices.ContainsKey(entry.Key) || choices[entry.Key] < entry.Value)
                {
                    return false;
                }
            }

            return PassEnergyBarrier(GetReactants(mainParticle, potentialReactants), hasCatalyst);
        }

        private bool PassEnergyBarrier(IList<Particle> particles, bool catalyst)
        {
            // interpret kinetic barrier as a reaction probability
            return random.NextDouble() < (catalyst ? catalystBarrier : kineticBarrier);
        }

        public Vector DoReaction(Particle mainParticle, IList<Particle> particles, ReactingParticleContainer container)
        {
            IList<Particle> reactants = GetReactants(mainParticle, particles);

            double totalEnergy = 0;

            foreach (Particle part in reactants)
            {
                double v2 = part.Velocity.MagnitudeSquared;
                double ke = .5 * part.Info.Mass * v2;
                totalEnergy += ke;

                container.RemoveParticle(part);
            }

            foreach (var entry in outputs)
            {
                for (int i = 0; i < entry.Value; ++i)
                {
                    double energy = random.NextDouble() * totalEnergy;
                    double mass = container.Dictionary.Map[entry.Key].Mass;
                    double speed = Math.Sqrt(2 * energy / mass);
                    Vector velocity = Vector.RandomDirection(speed, random);
                    totalEnergy -= energy;

                    container.AddParticle(container.Dictionary.MakeParticle(particles[0].Position, velocity, entry.Key));
                }
            }

            return particles[0].Position;
        }


        private IList<Particle> GetReactants(Particle mainParticle, IList<Particle> potentialReactants)
        {
            IList<Particle> output = new List<Particle> { mainParticle };

            foreach (Particle part in potentialReactants)
            {
                if (inputs.ContainsKey(part.Info.Name) && Count(output, part) < inputs[part.Info.Name])
                {
                    output.Add(part);
                }
            }

            return output;
        }

        private int Count(IList<Particle> list, Particle particle)
        {
            int output = 0;
            foreach (Particle part in list)
            {
                if (part.Info.Name.Equals(particle.Info.Name))
                {
                    ++output;
                }
            }
            return output;
        }

    }

}
