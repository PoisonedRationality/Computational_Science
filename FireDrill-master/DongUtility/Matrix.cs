using System;

namespace DongUtility
{
    /// <summary>
    /// A matrix class
    /// </summary>
    public struct Matrix
    {
        /// <summary>
        /// The actual values in the matrix
        /// </summary>
        private double[,] values;

        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        public Matrix(int rows, int columns)
        {
            if (rows < 1 || columns < 1)
                throw new ArgumentException("Cannot make a matrix with nonpositive number of rows or columns!");
            values = new double[rows, columns];
        }

        /// <summary>
        /// This constructs a matrix from a (column) vector, making a 3 x 1 matrix
        /// </summary>
        public Matrix(Vector vec) :
            this(3, 1)
        {
            values[0, 0] = vec.X;
            values[1, 0] = vec.Y;
            values[2, 0] = vec.Z;
        }

        /// <summary>
        /// Accessors. Row and column numbers are zero-indexed
        /// </summary>
        public double this[int row, int column]
        {
            get
            {
                return values[row, column];
            }
            set
            {
                values[row, column] = value;
            }
        }

        /// <summary>
        /// Number of rows
        /// </summary>
        public int NRows => values.GetLength(0);
        /// <summary>
        /// Number of columns
        /// </summary>
        public int NColumns => values.GetLength(1);

        static public Matrix operator +(Matrix lhs, Matrix rhs)
        {
            if (SameSize(lhs, rhs))
            {
                Matrix response = new Matrix(lhs.NRows, lhs.NColumns);
                for (int irow = 0; irow < response.NRows; ++irow)
                    for (int icolumn = 0; icolumn < response.NColumns; ++icolumn)
                    {
                        response[irow, icolumn] = lhs[irow, icolumn] + rhs[irow, icolumn];
                    }
                return response;
            }
            else
            {
                throw new ArgumentException("Matrices of different size cannot be added!");
            }
        }

        static public Matrix operator *(double lhs, Matrix rhs)
        {
            Matrix response = new Matrix(rhs.NRows, rhs.NColumns);
            for (int irow = 0; irow < response.NRows; ++irow)
                for (int icolumn = 0; icolumn < response.NColumns; ++icolumn)
                {
                    response[irow, icolumn] = lhs * rhs[irow, icolumn];
                }
            return response;
        }

        static public Matrix operator -(Matrix target)
        {
            return -1 * target;
        }

        static public Matrix operator -(Matrix lhs, Matrix rhs)
        {
            return lhs - rhs;
        }

        static public Matrix operator *(Matrix lhs, Matrix rhs)
        {
            if (CanMultiply(lhs, rhs))
            {
                Matrix response = new Matrix(lhs.NRows, rhs.NColumns);
                for (int irow = 0; irow < response.NRows; ++irow)
                    for (int icolumn = 0; icolumn < response.NColumns; ++icolumn)
                    {
                        double value = 0;
                        for (int i = 0; i < lhs.NColumns; ++i)
                        {
                            value += lhs[irow, i] * rhs[i, icolumn];
                        }
                        response[irow, icolumn] = value;
                    }
                return response;
            }
            else
            {
                throw new ArgumentException("Sizes invalid for matrix multiplication!");
            }
        }

        static public Vector operator *(Matrix lhs, Vector rhs)
        {
            var result = lhs * new Matrix(rhs);
            return new Vector(result[0, 0], result[1, 0], result[2, 0]);
        }

        public double Trace
        {
            get
            {
                if (!IsSquare)
                    throw new InvalidOperationException("Cannot calculate the trace of a non-square matrix");

                double response = 0;
                for (int i = 0; i < NRows; ++i)
                {
                    response += values[i, i];
                }
                return response;
            }
        }

        public double Determinant
        {
            get
            {
                if (!IsSquare)
                    throw new InvalidOperationException("Cannot calculate the determinant of a non-square matrix");

                if (NRows == 1)
                {
                    return values[0, 0];
                }
                else
                {
                    double determinant = 0;
                    for (int i = 0; i < NColumns; ++i)
                    {
                        determinant += values[0, i] * Cofactor(0, i);
                    }
                    return determinant;
                }
            }
        }

        public Matrix Transpose()
        {
            var matrix = new Matrix(NColumns, NRows);

            for (int irow = 0; irow < NRows; ++irow)
                for (int icolumn = 0; icolumn < NColumns; ++icolumn)
                {
                    matrix[icolumn, irow] = values[irow, icolumn];
                }
            return matrix;
        }

        /// <summary>
        /// Returns the cofactor for a given matrix position
        /// </summary>
        public double Cofactor(int row, int column)
        {
            if (!IsSquare)
                throw new InvalidOperationException("Cannot calculate the cofactor of a non-square matrix");

            int sign = (row + column) % 2 == 0 ? 1 : -1;

            return sign * Minor(row, column).Determinant;
        }

        /// <summary>
        /// Returns the comatrix (matrix of cofactors)
        /// </summary>
        public Matrix Comatrix()
        {
            if (!IsSquare)
                throw new InvalidOperationException("Cannot calculate the comatrix of a non-square matrix");

            var matrix = new Matrix(NRows, NColumns);

            for (int irow = 0; irow < NRows; ++irow)
                for (int icolumn = 0; icolumn < NColumns; ++icolumn)
                {
                    matrix[irow, icolumn] = Cofactor(irow, icolumn);
                }

            return matrix;
        }

        public Matrix Inverse()
        {
            if (!IsSquare)
                throw new InvalidOperationException("Cannot calculate the inverse of a non-square matrix");

            double determinant = Determinant;
            if (determinant == 0)
                throw new InvalidOperationException("Matrix is not invertible!");

            return (1 / determinant) * Comatrix().Transpose();
        }

        /// <summary>
        /// Returns the minor, the matrix obtained by removing a specific row and column
        /// </summary>
        public Matrix Minor(int removeRow, int removeColumn)
        {
            if (removeRow >= NRows || removeColumn >= NColumns || removeRow < 0 || removeColumn < 0)
                throw new ArgumentException("Invalid values passed to Submatrix()!");

            var matrix = new Matrix(NRows - 1, NColumns - 1);
            int currentRow = 0;

            for (int irow = 0; irow < NRows; ++irow)
            {
                int currentColumn = 0;
                for (int icolumn = 0; icolumn < NColumns; ++icolumn)
                {
                    if (icolumn == removeColumn || irow == removeRow)
                        continue;

                    matrix[currentRow, currentColumn] = values[irow, icolumn];

                    ++currentColumn;
                }
                if (irow != removeRow)
                    ++currentRow;
            }
            return matrix;
        }

        /// <summary>
        /// Checks to see if two matrices are the same size
        /// </summary>
        static public bool SameSize(Matrix lhs, Matrix rhs)
        {
            return lhs.NRows == rhs.NRows && lhs.NColumns == rhs.NColumns;
        }

        /// <summary>
        /// Checks to see if two matrices are of appropriate sizes to multiply together
        /// </summary>
        static public bool CanMultiply(Matrix lhs, Matrix rhs)
        {
            return lhs.NColumns == rhs.NRows;
        }

        public bool IsSquare => NRows == NColumns;

        public override string ToString()
        {
            string response = "";
            for (int irow = 0; irow < NRows; ++irow)
            {
                response += "[ ";
                for (int icolumn = 0; icolumn < NColumns; ++icolumn)
                {
                    response += values[irow, icolumn];
                    response += "\t";
                }
                response += "]\n";
            }
            return response;
        }
    }
}
