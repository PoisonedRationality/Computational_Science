using System;

namespace DongUtility
{
    /// <summary>
    /// A class representing a (persistent) rotation that can be applied to matrices
    /// </summary>
    public class Rotation
    {
        /// <summary>
        /// The underlying representation of the rotation is a matrix
        /// </summary>
        private Matrix rotation;

        public Rotation()
        {
            rotation = new Matrix(3, 3);
            // Begin with the identity matrix
            rotation[0, 0] = 1;
            rotation[1, 1] = 1;
            rotation[2, 2] = 1;
        }

        static public Rotation Identity => new Rotation();

        public Matrix Matrix => rotation;

        /// <summary>
        /// Constructs a rotation from an existing 3 x 3 matrix
        /// </summary>
        public Rotation(Matrix matrix)
        {
            if (matrix.NRows != 3 || matrix.NColumns != 3)
                throw new ArgumentException("Invalid matrix size passed to Rotation constructor");

            rotation = matrix;
        }

        /// <summary>
        /// Applies the rotation to a vector
        /// </summary>
        public Vector ApplyRotation(Vector input)
        {
            return rotation * input;
        }

        /// <summary>
        /// Inverts the rotation.
        /// The inverse of a rotation matrix is the same as its transpose
        /// </summary>
        public Rotation Inverse()
        {
            return new Rotation(rotation.Transpose());
        }

        /// <summary>
        /// Rotates about the x axis in a counter-clockwise direction
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        public void RotateXAxis(double angle)
        {
            rotation = RotationMatrix(new Vector(1, 0, 0), angle) * rotation;
        }

        /// <summary>
        /// Rotates about the y axis in a counter-clockwise direction
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        public void RotateYAxis(double angle)
        {
            rotation = RotationMatrix(new Vector(0, 1, 0), angle) * rotation;
        }

        /// <summary>
        /// Rotates about the z axis in a counter-clockwise direction
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        public void RotateZAxis(double angle)
        {
            rotation = RotationMatrix(new Vector(0, 0, 1), angle) * rotation;
        }

        /// <summary>
        /// Rotates about an arbitrary axis in a counter-clockwise direction
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        public void RotateArbitraryAxis(Vector axis, double angle)
        {
            rotation = RotationMatrix(axis, angle) * rotation;
        }

        /// <summary>
        /// Constructs the rotation matrix for an angle counter-clockwise (right-handed) about an arbitrary axis
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        static public Matrix RotationMatrix(Vector axis, double angle)
        {
            axis = axis.UnitVector();
            var matrix = new Matrix(3, 3);
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            double oneMinusCos = 1 - cos;
            matrix[0, 0] = cos + UtilityFunctions.Square(axis.X) * oneMinusCos;
            matrix[0, 1] = axis.X * axis.Y * oneMinusCos - axis.Z * sin;
            matrix[0, 2] = axis.X * axis.Z * oneMinusCos + axis.Y * sin;
            matrix[1, 0] = axis.Y * axis.X * oneMinusCos + axis.Z * sin;
            matrix[1, 1] = cos + UtilityFunctions.Square(axis.Y) * oneMinusCos;
            matrix[1, 2] = axis.Y * axis.Z * oneMinusCos - axis.X * sin;
            matrix[2, 0] = axis.Z * axis.X * oneMinusCos - axis.Y * sin;
            matrix[2, 1] = axis.Z * axis.Y * oneMinusCos + axis.X * sin;
            matrix[2, 2] = cos + UtilityFunctions.Square(axis.Z) * oneMinusCos;
            return matrix;
        }
    }
}
