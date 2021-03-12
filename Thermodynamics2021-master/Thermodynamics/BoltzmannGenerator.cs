using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DongUtility;

namespace Thermodynamics
{
    public class BoltzmannGenerator : RandomGenerator
    {
        private readonly double minSpeed;
        private readonly double maxSpeed;

        public BoltzmannGenerator(ParticleContainer cont, double minSpeed, double maxSpeed) :
            base(cont)
        {
            this.minSpeed = minSpeed;
            this.maxSpeed = maxSpeed;
        }

        protected override double GetSpeed(ParticleInfo info)
        {
            double counter = 0;
            double mass = info.Mass;
            double temperature = 293.15;
            double result = 0;
            double velo;
            while (counter < 1)
            {
                velo = RandomGen.NextDouble(minSpeed, maxSpeed);
                double equation = BoltzmannFunction(velo, mass, temperature);

                double rand = RandomGen.NextDouble();

                if (rand < equation)
                {
                    result = velo;
                    counter++;
                }
            }

            return result;
        }

        private static double BoltzmannFunction(double velo, double mass, double temp)
        {
            double result = 4 * Math.PI * (Math.Pow(mass / (2 * Math.PI * Constants.BoltzmannConstant * temp), 1.5)) * Math.Pow(velo, 2) * Math.Exp(-1 * (mass * velo * velo / (2 * Constants.BoltzmannConstant * temp)));
            return result;

        }
    }
}