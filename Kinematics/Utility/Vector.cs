using System;

namespace Utility
{
    /// <summary>
    /// A basic 3D vector
    /// </summary>
    public struct Vector
    {
 

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; set; }

        public double Y
        { get; set; }

        public double Z
        { get; set; }
        
        public double Magnitude => Math.Sqrt(X * X + Y * Y + Z * Z);

        static public Vector Normalize(Vector vec)
        {
            return vec / vec.Magnitude;
        }

        static public double Dot(Vector one, Vector two)
        {
            return (one.X * two.X + one.Y * two.Y + one.Z * two.Z);
        }

        static public Vector Cross(Vector one, Vector two)
        {
            var vec = new Vector
            {
                X = one.Y * two.Z - one.Z * two.Y,
                Y = one.Z * two.X - one.X * two.Z,
                Z = one.X * two.Y - one.Y * two.X
            };
            return vec;
        }

        public override string ToString()
        {
            return X + ", " + Y + ", " + Z;
        }

        static public Vector operator + (Vector one, Vector two)
        {
            var vec = new Vector
            {
                X = one.X + two.X,
                Y = one.Y + two.Y,
                Z = one.Z + two.Z
            };
            return vec;
        }

        static public Vector operator - (Vector one, Vector two)
        {
            return one + (-1 * two);
        }

        static public Vector operator * (double scalar, Vector two)
        {
            var vec = new Vector
            {
                X = scalar * two.X,
                Y = scalar * two.Y,
                Z = scalar * two.Z
            };
            return vec;
        }


        static public Vector operator * (Vector two, double scalar)
        {
            var vec = new Vector
            {
                X = scalar * two.X,
                Y = scalar * two.Y,
                Z = scalar * two.Z
            };
            return vec;
        }

        static public Vector operator / (Vector one, double scalar)
        {
            var vec = new Vector
            {
                X = one.X / scalar,
                Y = one.Y / scalar,
                Z = one.Z / scalar
            };
            return vec;
        }


    }


}
