using System;
using System.Numerics;

namespace ProjectileMotion
{
    class Program
    {
       static void Main(string[] args)
        {
            //LevelOne();
            //LevelTwo();
            LevelThree();
            
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
            float dt = 0.001F;
            Console.WriteLine("Time (s)" + "\t" + "x" +"\t" + "y"+ "\t" + "z" + "\t" + "Distance" + "\t" + "vx" + "\t" + "vy" + "\t" + "vz" + "\t"
                + "Speed" + "\t" + "ax" + "\t" + "ay" + "\t" + "az" + "\t" + "m_accel");
            while (position.Z>=0) 
            {
                Console.WriteLine(time.ToString("0.000") + "\t" + position.X.ToString("0.000") + "\t" + position.Y.ToString("0.000") +
                    "\t" + position.Z.ToString("0.000") + "\t" + distance.ToString("0.000") + "\t" + velocity.X.ToString("0.000") + "\t" + velocity.Y.ToString("0.000") +
                    "\t" + velocity.Z.ToString("0.000") + "\t" + speed.ToString("0.000") + "\t" + acceleration.X.ToString("0.000") + "\t" + acceleration.Y.ToString("0.000")
                    + "\t" + acceleration.Z.ToString("0.000") + "\t" + m_accel.ToString("0.000"));
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
            //theta = 45 deg
            float theta = (float)Math.PI / 4;
            float time = 0;

            //initialize velocity vector
            float speed = 5;
            Vector3 velocity = new Vector3((float)Math.Cos(theta) * speed, 0, (float)Math.Sin(theta) * speed);

            //accel = net force/mass
            //force of air resistance = C*v^2
            float c = 0.5F;
            float mass = 4;
            Vector3 air_accel = Vector3.Multiply(-c*velocity.Length()*velocity.Length()/mass, Vector3.Normalize(velocity));
            Vector3 g_accel = new Vector3(0, 0, -9.8F);
            Vector3 acceleration = Vector3.Add(air_accel, g_accel);
            
            float m_accel = acceleration.Length();
            //initialize position variable
            Vector3 position = new Vector3(0, 0, 0);
            float distance = 0;
            //initialize deltatime variable
            float dt = 0.001F;
            Console.WriteLine("Time (s)" + "\t" + "x" + "\t" + "y" + "\t" + "z" + "\t" + "Distance" + "\t" + "vx" + "\t" + "vy" + "\t" + "vz" + "\t"
                + "Speed" + "\t" + "ax" + "\t" + "ay" + "\t" + "az" + "\t" + "m_accel");
            while (position.Z >= 0)
            { 
                Console.WriteLine(time.ToString("0.000") + "\t" + position.X.ToString("0.000") + "\t" + position.Y.ToString("0.000") +
                    "\t" + position.Z.ToString("0.000") + "\t" + distance.ToString("0.000") + "\t" + velocity.X.ToString("0.000") + "\t" + velocity.Y.ToString("0.000") +
                    "\t" + velocity.Z.ToString("0.000") + "\t" + speed.ToString("0.000") + "\t" + acceleration.X.ToString("0.000") + "\t" + acceleration.Y.ToString("0.000")
                    + "\t" + acceleration.Z.ToString("0.000") + "\t" + m_accel.ToString("0.000")); 
                time += dt;

                velocity += acceleration * dt;
                position += velocity * dt;

                air_accel = Vector3.Multiply(-c * speed * speed / mass, Vector3.Normalize(velocity));
                acceleration = air_accel+g_accel;

                distance = position.Length();
                speed = velocity.Length();
                m_accel = acceleration.Length();
            }
            Console.WriteLine("Done.");
        }

        static void LevelThree()
        {

            float time = 0;

            //initialize velocity vector
            Vector3 velocity = new Vector3(5, -1, 3);
            float speed = velocity.Length();
            //accel = net force/mass
            //force of air resistance = C*v^2
            float c = 0.5F;
            float mass = 4;
            float spring_const = 8;
            float spring_len = 2;
            //initialize position variable
            Vector3 position = new Vector3(-1, 1, -1);
            float distance = position.Length();
            //initialize deltatime variable
            float dt = 0.01F;

            Vector3 spring_accel = Vector3.Multiply(-spring_const / mass, (distance - spring_len) * Vector3.Normalize(position));
            Vector3 air_accel = Vector3.Multiply(-c * speed * speed / mass, Vector3.Normalize(velocity));
            Vector3 g_accel = new Vector3(0, 0, -9.8F);
            Vector3 acceleration = air_accel+g_accel+spring_accel;
            float m_accel = acceleration.Length();
            Console.WriteLine("Time (s)" + "\t" + "x" + "\t" + "y" + "\t" + "z" + "\t" + "Distance" + "\t" + "vx" + "\t" + "vy" + "\t" + "vz" + "\t"
                + "Speed" + "\t" + "ax" + "\t" + "ay" + "\t" + "az" + "\t" + "m_accel");
            while (time<=23)
            {
                Console.WriteLine(time.ToString("0.00") + "\t" + position.X.ToString("0.000") + "\t" + position.Y.ToString("0.000") +
                    "\t" + position.Z.ToString("0.000") + "\t" + distance.ToString("0.000") + "\t" + velocity.X.ToString("0.000") + "\t" + velocity.Y.ToString("0.000") +
                    "\t" + velocity.Z.ToString("0.000") + "\t" + speed.ToString("0.000") + "\t" + acceleration.X.ToString("0.000") + "\t" + acceleration.Y.ToString("0.000")
                    + "\t" + acceleration.Z.ToString("0.000") + "\t" + m_accel.ToString("0.000"));
                
                time += dt;

                velocity += acceleration * dt;
                position += velocity * dt;

                air_accel = Vector3.Multiply(-c * speed * speed / mass, Vector3.Normalize(velocity));
                spring_accel = Vector3.Multiply(-spring_const / mass, (distance-spring_len) * Vector3.Normalize(position));
                acceleration = air_accel + g_accel+spring_accel;

                distance = position.Length();
                speed = velocity.Length();
                m_accel = acceleration.Length();
            }
            Console.WriteLine("Done.");
        }
    }
}
