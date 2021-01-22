using System;

namespace Kinematics
{
    class Program
    {
        static void Main(string[] args)
        {
            Challenge();


        }

        private static void LevelOne()
        {
            //v = dx/dt  --->  dx = v*dt
            
            //dt = 0.1


            //initialize velocity variable to 5 m/s
            //using decimal instead of double to get rid of annoying floating point errors
            decimal velocity = 5;
            //initialize position variable
            decimal position = 0;



            Console.WriteLine("Time (s)" + "\tPosition (m)" + "\tVelocity (m/s)");

            for (decimal time = 0; time <= 100; time += (decimal)0.1)
            {
                Console.WriteLine(time + "\t\t" + position + "\t\t" + velocity);


                position = position + velocity * (decimal)0.1;




            }
            Console.WriteLine("Done");

        }
        private static void LevelTwo()
        {
            //v = dx/dt  --->  dx = v*dt
            //a = dv/dt = d^2x/dt^2
            //dt = 0.1


            //initialize velocity variable to 5 m/s
            //using decimal instead of double to get rid of annoying floating point errors
            decimal velocity = 5;
            //initialize position variable
            decimal position = 0;

            //initialize acceleration as 9.8
            decimal acceleration = (decimal)9.8;


            Console.WriteLine("Time (s)" + "\tPosition (m)" + "\tVelocity (m/s)" + "\tAcceleration");

            for (decimal time = 0; time <= 100; time += (decimal)0.1)
            {
                Console.WriteLine(time + "\t" + position + "\t" + velocity + "\t" + acceleration);
                velocity = velocity + acceleration * (decimal)0.1;

                position = position + velocity * (decimal)0.1;




            }
            Console.WriteLine("Done");
        }

        private static void LevelThree()
        {
            //v = dx/dt  --->  dx = v*dt
            //a = dv/dt = d^2x/dt^2
            //dt = 0.1


            //initialize velocity variable to 5 m/s
            //using decimal instead of double to get rid of annoying floating point errors
            decimal velocity = 5;
            //initialize position variable
            decimal position = 0;
            //initialize mass variable
            decimal mass = 4;
            //need to calculate F_net
            //F_g = mass*9.8
            decimal f_g = mass * (decimal)9.8;
            //F_air = 0.5*v^2
            decimal f_air = (decimal)(0.5) * velocity * velocity;
            //F_net = f_g - f_air
            decimal f_net = f_g - f_air;
            //initialize acceleration as 9.8
            decimal acceleration = f_net/mass;


            Console.WriteLine("Time (s)" + "\tPosition (m)" + "\tVelocity (m/s)" + "\tAcceleration");

            for (decimal time = 0; time <= 100; time += (decimal)0.1)
            {
                Console.WriteLine(time + "," + position + "," + velocity + "," + acceleration);
                velocity = velocity + acceleration * (decimal)0.1;

                position = position + velocity * (decimal)0.1;

                
                f_air = (decimal)(0.5) * velocity * velocity;
                
                f_net = f_g - f_air;

                acceleration = f_net / mass;


            }
            Console.WriteLine("Done");

        }

        

        private static void Challenge()
        {
            //v = dx/dt  --->  dx = v*dt
            //a = dv/dt = d^2x/dt^2
            //dt = 0.1


            //initialize velocity variable to 5 m/s
            //using decimal instead of double to get rid of annoying floating point errors
            decimal velocity = 5;
            //initialize position variable
            decimal position = -1;
            //initialize mass variable
            decimal mass = 4;
            //need to calculate F_net
            //F_g = mass*9.8
            decimal f_g = mass * (decimal)9.8;
            //F_air = 0.5*v^2
            decimal f_air = (decimal)(0.5) * velocity * velocity;
            //F_spring = -8*(position relative to spring) = -8*(position - 2)
            decimal f_spring = -8 * (position - 2);
            //F_net = sum of all forces
            decimal f_net = f_g - f_air + f_spring;
            //initialize acceleration as 9.8
            decimal acceleration = f_net / mass;


            Console.WriteLine("Time (s)" + "\tPosition (m)" + "\tVelocity (m/s)" + "\tAcceleration");

            for (decimal time = 0; time <= 100; time += (decimal)0.1)
            {
                Console.WriteLine(time + "," + position + "," + velocity + "," + acceleration);
                velocity = velocity + acceleration * (decimal)0.1;

                position = position + velocity * (decimal)0.1;

                if (velocity > 0)
                {
                    f_air = (decimal)(0.5) * velocity * velocity;
                }
                else
                {
                    f_air = (decimal)(-0.5) * velocity * velocity;
                }

                f_spring = -8 * (position - 2);

                f_net = f_g - f_air + f_spring;

                acceleration = f_net / mass;


            }
            Console.WriteLine("Done");


        }
    }


}
