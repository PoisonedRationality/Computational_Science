using System;
using System.Numerics;

namespace ProjectileMotion
{
    class Program
    {
       static void Main(string[] args)
        {
            LevelOne();
            //LevelTwo();
            //LevelThree();
            
        }

        static void LevelOne() 
        {
            //theta = 45 deg
            float theta = (float)Math.PI/ 4;
            float time = 0;

            //initialize velocity vector
            float speed = 5;
            Vector3 velocity = new Vector3((float)Math.Cos(theta)*speed, 0, (float)Math.Sin(theta)*speed);

            //initialize acceleration vector (only in z axis, everything else is just 0 so it doesn't matter)
            Vector3 acceleration = new Vector3(0, 0, -9.8F);
            float m_accel = acceleration.Length();
            //initialize position variable
            Vector3 position = new Vector3(0, 0, 0);
            float distance = 0;
            //initialize deltatime variable
            float dt = 0.01F;
            Console.WriteLine("Time (s)" + "\t" + "x" +"\t" + "y"+ "\t" + "z" + "\t" + "Distance" + "\t" + "vx" + "\t" + "vy" + "\t" + "vz" + "\t"
                + "Speed" + "\t" + "ax" + "\t" + "ay" + "\t" + "az" + "\t" + "m_accel");
            while (position.Z>=0) 
            {
                Console.WriteLine(time.ToString("0.00") + "\t" + position.X.ToString("0.00") + "\t" + position.Y.ToString("0.00") +
                    "\t" + position.Z.ToString("0.00") + "\t" + distance.ToString("0.00") + "\t" + velocity.X.ToString("0.00") + "\t" + velocity.Y.ToString("0.00") +
                    "\t" + velocity.Z.ToString("0.00") + "\t" + speed.ToString("0.00") + "\t" + acceleration.X.ToString("0.00") + "\t" + acceleration.Y.ToString("0.00")
                    + "\t" + acceleration.Z.ToString("0.00") + "\t" + m_accel.ToString("0.00"));
                time += dt;
                
                velocity += acceleration * dt;
                position += velocity*dt;

                distance = position.Length();
                speed = velocity.Length();
            }
            Console.WriteLine("Done.");
        }
        
        static void LevelTwo()
        {

        }

        static void LevelThree()
        {

        }
    }
}
