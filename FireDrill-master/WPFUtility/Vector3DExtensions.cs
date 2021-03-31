using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace WPFUtility
{
    static public class Vector3DExtensions
    {
        /// <summary>
        /// The azimuthal angle is the angle from the x axis in the x-y plane (phi in physics, theta in math)
        /// </summary>
        static public double Azimuthal(this Vector3D vector)
        {
            double answer = Math.Atan2(vector.Y, vector.X);
            if (answer < 0)
                answer += 2 * Math.PI;
            return answer;
        }

        static public double Polar(this Vector3D vector)
        {
            return Math.Acos(vector.Z / vector.Length);
        }

        static public Vector3D SphericalCoordinates(double radius, double azimuthal, double polar)
        {
            double x = radius * Math.Cos(azimuthal) * Math.Sin(polar);
            double y = radius * Math.Sin(azimuthal) * Math.Sin(polar);
            double z = radius * Math.Cos(polar);

            return new Vector3D(x, y, z);
        }

        static public Vector3D Midpoint(Vector3D one, Vector3D two)
        {
            return (one + two) / 2;
        }

        static public bool IsValid(this Vector3D vector)
        {
            return DongUtility.UtilityFunctions.IsValid(vector.X)
                && DongUtility.UtilityFunctions.IsValid(vector.Y)
                && DongUtility.UtilityFunctions.IsValid(vector.Z);
        }
    }
}
