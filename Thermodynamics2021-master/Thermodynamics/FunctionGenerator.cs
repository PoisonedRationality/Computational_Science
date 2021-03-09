using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thermodynamics
{
    abstract public class FunctionGenerator : RandomGenerator
    {
        private double minVal;
        private double maxVal;
        private double increment = double.NaN;
        private double normalization;
        private readonly int nDivisions;
        private readonly double threshold;



        public FunctionGenerator(ParticleContainer cont, int nDivisions = 10000, int projectedCalls = 10000) :
            base(cont)
        {
            threshold = 1.0 / projectedCalls / 10; // One tenth of the reciprocal of the number of calls
            this.nDivisions = nDivisions;
        }

        protected void Setup()
        {
            var range = GetScale(threshold);
            minVal = Math.Pow(10, range.Item1);
            maxVal = Math.Pow(10, range.Item2);
            increment = (maxVal - minVal) / nDivisions;
            normalization = Normalize();
        }

        // These are the minimum and maximum exponents for doubles
        private const int minExponent = -324;
        private const int maxExponent = 308;

        private Tuple<int, int> GetScale(double threshold)
        {
            // Test all exponents
            var exponentMap = new Dictionary<int, double>();

            for (int exponent = minExponent; exponent <= maxExponent; ++exponent)
            {
                double value = Math.Pow(10, exponent);
                exponentMap.Add(exponent, Function(value));
            }

            double maxVal = exponentMap.Values.Max();
            int minExp = int.MaxValue;
            int maxExp = int.MinValue;
            double maxFunc = double.MinValue;

            foreach (var entry in exponentMap)
            {
                double scale = entry.Value / maxVal;
                if (scale > threshold)
                {
                    if (minExp == int.MaxValue)
                        minExp = entry.Key;
                    if (scale > maxFunc)
                    {
                        maxFunc = scale;
                        maxExp = entry.Key;
                    }
                }
            }

            return new Tuple<int, int>(minExp - 1, maxExp + 1);
        }

        private double Normalize()
        {
            double total = 0;

            for (double current = minVal; current < maxVal; current += increment)
            {
                total += increment * Function(current);
            }

            return 1 / total;
        }

        protected override double GetSpeed(ParticleInfo info)
        {
            double ran = RandomGen.NextDouble();
            double speed = minVal;
            double cumulative = 0;
            while (cumulative < ran)
            {
                speed += increment;
                cumulative += increment * normalization * Function(speed);
            }

            return speed;
        }

        abstract protected double Function(double speed);
    }
}
