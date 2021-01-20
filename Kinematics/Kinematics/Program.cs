using System;

namespace Kinematics
{
    class Program
    {
        static void Main(string[] args)
        {
            //v = dx/dt  --->  dx = v*dt
            //dt = 0.1


            //initialize velocity variable to 5 m/s
            //using decimal instead of double to get rid of annoying floating point errors
            decimal velocity = 5;
            //initialize position variable
            decimal x = 0;
            Console.WriteLine("Time (s)" + "\tPosition (m)" + "\tVelocity (m/s)");
            for (decimal time = 0; time <= 100; time += (decimal)0.1) 
            {

                Console.WriteLine(time + "\t" + x + "\t" + velocity);

                x = x + velocity * (decimal)0.1;

            }
            Console.WriteLine("Done");


        }
    }
}
