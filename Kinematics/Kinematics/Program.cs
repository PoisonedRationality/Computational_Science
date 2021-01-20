using System;

namespace Kinematics
{
    class Program
    {
        static void Main(string[] args)
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
    }


}
