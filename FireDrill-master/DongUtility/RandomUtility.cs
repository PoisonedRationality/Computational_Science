using System;

namespace DongUtility
{
    public static class RandomUtility
    {
        /// <summary>
        /// Throws a random number with a Gaussian distribution.
        /// Box-Muller transform, copied from stackoverflow.com/questions/218060/random-gaussian-variables
        /// </summary>
        /// <param name="mean">The mean of the distribution</param>
        /// <param name="sd">The standard deviation of the distribution</param>
        static public double NextGaussian(this Random random, double mean, double sd)
        {
            double u1 = random.NextDouble();
            double u2 = random.NextDouble();
            double ranNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + sd * ranNormal;
        }

        static public double NextDouble(this Random random, double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }

        static public bool NextBool(this Random random)
        {
            return random.Next(2) == 0;
        }

        /// <summary>
        /// Returns a bool that will be true a given fraction of the time,
        /// as in a binomial distribution
        /// </summary>
        /// <param name="random"></param>
        /// <param name="fraction"></param>
        /// <returns></returns>
        static public bool NextBool(this Random random, double fraction)
        {
            return random.NextDouble() < fraction;
        }
    }
}
