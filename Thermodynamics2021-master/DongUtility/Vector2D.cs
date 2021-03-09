using System;

namespace DongUtility
{
    /// <summary>
    /// A simple three-dimensional vector of doubles
    /// </summary>
    public struct Vector2D
    {

        /// <summary>
        /// Cartesian variables are the default accessors
        /// </summary>
        public double X { get; set; }
        public double Y { get; set; }

        /// <summary>
        /// Cartesian constructor
        /// </summary>
        public Vector2D(double x, double y)
        {
            if (!UtilityFunctions.IsValid(x) || !UtilityFunctions.IsValid(y))
            {
                throw new ArgumentException("Attempted to pass infinity or not-a-number to Vector2D class!");
            }
            X = x;
            Y = y;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="rhs">The vector to copy from</param>
        public Vector2D(Vector2D rhs) :
            this(rhs.X, rhs.Y)
        {
        }

        public override bool Equals(object obj)
        {
            Vector2D vec = (Vector2D)obj;
            return X == vec.X && Y == vec.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        override public string ToString()
        {
            return X + "\t" + Y + '\t' + Magnitude;
        }

        static public Vector2D operator +(Vector2D one, Vector2D two)
        {
            return new Vector2D(one.X + two.X, one.Y + two.Y);
        }

        static public Vector2D operator -(Vector2D vec)
        {
            return vec * -1;
        }

        static public Vector2D operator -(Vector2D one, Vector2D two)
        {
            return one + (-two);
        }

        static public Vector2D operator *(Vector2D vec, double scale)
        {
            return new Vector2D(vec.X * scale, vec.Y * scale);
        }

        static public Vector2D operator *(double scale, Vector2D vec)
        {
            return vec * scale;
        }

        static public Vector2D operator /(Vector2D vec, double divisor)
        {
            if (divisor == 0)
            {
                throw new DivideByZeroException();
            }
            else
            {
                return vec * (1 / divisor);
            }
        }

        static public bool operator ==(Vector2D one, Vector2D two)
        {
            return one.X == two.X && one.Y == two.Y;
        }

        static public bool operator !=(Vector2D one, Vector2D two)
        {
            return !(one == two);
        }

        public bool IsNull => X == 0 && Y == 0;

        /// <summary>
        /// The magnitude of the vector squared, which is much faster to calculate.
        /// </summary>
        public double MagnitudeSquared 
        {
            get => X * X + Y * Y;
        }

        /// <summary>
        /// The magnitude of the vector
        /// </summary>
        public double Magnitude
        {
            get => Math.Sqrt(MagnitudeSquared);
        }

        /// <summary>
        /// The azimuthal angle is the angle from the x axis in the x-y plane (phi in physics, theta in math)
        /// </summary>
        public double Azimuthal
        {
            get
            {
                double answer = Math.Atan2(Y, X);
                if (answer < 0)
                    answer += 2 * Math.PI;
                return answer;
            }
        }

        /// <summary>
        /// Reflects the vector across a given axis
        /// </summary>
        /// <param name="origin">The origin of the axis about which the vector is to be reflected</param>
        /// <param name="axis">The end point of the axis of reflection that goes through the origin</param>
        /// <returns>A vector that has been reflected about the given axis</returns>
        public Vector2D ReflectParticle(Vector2D origin, Vector2D axis)
        {
            Vector2D unitN = axis.UnitVector();

            // From Wolfram MathWorld, "Reflection":
            return -this + 2 * origin + 2 * unitN * Dot(this - origin, unitN);
        }
        
        /// <summary>
        /// A normalized version of a given vector
        /// </summary>
        /// <returns>A unit vector that points in the same direction as the given vector</returns>
        public Vector2D UnitVector()
        {
            double mag2 = MagnitudeSquared;
            if (mag2 == 0)
            {
                return NullVector();
            }
            else
            {
                return this / Math.Sqrt(mag2);
            }
        }
        
        /// <summary>
        /// Gives the dot product (Euclidean scalar product) of two vectors
        /// </summary>
        public static double Dot(Vector2D v1, Vector2D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        /// <summary>
        /// Gives the distance between two spatial vectors
        /// </summary>
        public static double Distance(Vector2D v1, Vector2D v2)
        {
            return Math.Sqrt(Distance2(v1, v2));
        }

        /// <summary>
        /// Gives the distance squared between two spatial vectors
        /// </summary>
        public static double Distance2(Vector2D v1, Vector2D v2)
        {
            return (v2 - v1).MagnitudeSquared;
        }

        /// <summary>
        /// The angle between two vectors.
        /// </summary>
        public static double AngleBetween(Vector2D v1, Vector2D v2)
        {
            return Math.Acos(Vector2D.Dot(v1, v2) / (v1.Magnitude * v2.Magnitude));
        }

        /// <summary>
        /// A convenient null vector
        /// </summary>
        /// <returns>A vector of magnitude zero</returns>
        public static Vector2D NullVector()
        {
            return new Vector2D(0, 0);
        }

        /// <summary>
        /// Takes the place of a spherical constructor
        /// </summary>
        /// <param name="r">The radius or magnitude of the vector</param>
        /// <param name="phi">The azimuthal angle, in radians, using the physics convention (this would be theta in math)
        /// It goes from 0 to 2 pi and gives the angle in x-y plane.</param>
        /// <param name="theta">The polar angle, in radians, using the physics convention (this would be phi in math)
        /// It goes from 0 to pi and gives the angle from the positive z axis.</param>
        /// <returns>A vector matching these three parameters</returns>
        public static Vector2D PolarVector(double r, double phi)
        {
            double x = r * Math.Cos(phi);
            double y = r * Math.Sin(phi);

            return new Vector2D(x, y);
        }

        /// <summary>
        /// Creates a vector of a given magnitude which points in a random direction, evenly distributed across the surface of a sphere
        /// </summary>
        /// <param name="magnitude">The magnitude of the random vector</param>
        /// <param name="generator">The Random object used to generate the random vector.  This is defined externally so that users can control the random behavior.</param>
        /// <returns>A vector of the given magnitude that points in a randomly chosen direction</returns>
        public static Vector2D RandomDirection(double magnitude, Random generator)
        {
            double phi = generator.NextDouble() * 2 * Math.PI;

            return Vector2D.PolarVector(magnitude, phi);
        }

        /// <summary>
        /// Returns the projection of this vector onto another vector
        /// </summary>
        public Vector2D ProjectOnto(Vector2D otherVec)
        {
            Vector2D unitVec = otherVec.UnitVector();
            return Vector2D.Dot(this, unitVec) * unitVec;
        }

        /// <summary>
        /// Returns whether two vectors point in the same general direction; that is,
        /// if the projection of one onto the other is in the same direction as the original.
        /// Perpendicular vectors return false, as do null vectors.
        /// </summary>
        static public bool SameDirection(Vector2D vec1, Vector2D vec2)
        {
            double dotProduct = Vector2D.Dot(vec1, vec2);
            return dotProduct > 0;
        }

        /// <summary>
        /// Finds the intersection between the line (not line segment!) defined by p1 and p2 and the line defined by p3 and p4
        /// </summary>
        static public Vector2D Intersection(Vector2D p1, Vector2D p2, Vector2D p3, Vector2D p4)
        {
            // Check for parallel lines
            double slope1 = (p2.Y - p1.Y) / (p2.X - p1.X);
            double slope2 = (p4.Y - p3.Y) / (p4.X - p3.X);

            if (slope1 == slope2)
                throw new ArgumentException("Lines are parallel!");

            // Using simple math with equation of a line
            double intercept1 = -slope1 * p1.X + p1.Y;
            double intercept2 = -slope2 * p3.X + p3.Y;

            double xCoord = (intercept2 - intercept1) / (slope1 - slope2);
            double yCoord = slope1 * xCoord + intercept1;
            return new Vector2D(xCoord, yCoord);
        }
    }
}
