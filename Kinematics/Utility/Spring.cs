using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class Spring
    {
        public Spring(double len, double k, Projectile proj)
        {
            Length = len;

            K = k;

            Proj = proj;
        }

        public double Length { get; set; }

        public double K { get; set; }

        public Projectile Proj { get; set; }

    }
}
