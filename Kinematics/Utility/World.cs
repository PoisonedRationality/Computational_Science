using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class World
    {

        public World(double time, double c, double g_accel, double ground, List<Projectile> list)
        {
            Time = time;
            
            Projectiles = list;

            Challenge = false;

        }

        public World(List<Projectile> list)
        {
            Time = 0;


            Projectiles = list;

            Challenge = false;
        }

        public World(List<Projectile> list, bool challenge)
        {
            Time = 0;


            Projectiles = list;

            Challenge = challenge;
        }


        public double Time { get; set; }

        public const double G = 6.67430e-11;

        public bool Challenge { get; set; }


        public List<Projectile> Projectiles { get; set; }

        public void ApplyForcesToProjectiles()
        {
            var moon = Projectiles[0];
            var f_grav = Vector.Zero;
            foreach (Projectile earth in Projectiles)
            {
                if (earth == Projectiles[0])
                {
                    continue;
                }
                var force_magnitude = (G * moon.Mass * earth.Mass / Math.Pow((moon.Position - earth.Position).Magnitude, 2));
                var direction = Vector.Normalize(earth.Position - moon.Position);
                Vector force = force_magnitude * direction;
                f_grav += force;
            }


            Projectiles[0].ApplyForce(f_grav);


        }

        public bool Tick(double dt)
        {
            bool running = true;
            Time += dt;
            ApplyForcesToProjectiles();

            foreach (Projectile proj in Projectiles)
            {
                proj.Move(dt);
            }

            if (Challenge)
            {
                double earthDepth = Projectiles[1].Position.Z * 2;
                int numPoints = (int)(10e6 / Math.Pow(Projectiles[0].Position.Z - earthDepth, 2));
                double earthMass = Projectiles[1].Mass*numPoints;
                double earthRadius = 6.371e6;
                double sideLength = earthRadius * 2;
                var temp = new List<Projectile>();
                temp.Add(Projectiles[0]);
                double spacing = sideLength / numPoints;

                for (int x = 0; x < numPoints; x++)
                {
                    for (int y = 0; y < numPoints; y++)
                    {
                        var pos = new Vector(sideLength/2 - spacing * .5 - spacing * x, sideLength/2 - spacing * .5 - spacing * y, earthDepth / 2);
                        var point = new Projectile(pos, Vector.Zero, Vector.Zero, 0);
                        temp.Add(point);
                    }
                }

                var pointMass = earthMass / temp.Count;
                foreach (Projectile point in temp)
                {
                    point.Mass = pointMass;
                }

                Projectiles = temp;

            }

            return running;
            
        }
    }
}
