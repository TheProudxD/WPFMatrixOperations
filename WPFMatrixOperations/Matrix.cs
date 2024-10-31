using System;

namespace WPFMatrixOperations
{
    public class Matrix<T>
    {
        public T[,] Array { get; }

        public (int X, int Y) Size { get; set; }

        public Matrix(T[,] array) => Array = array;

        public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b)
        {
            int firstDimensionSize = b.Array.GetLength(0);
            int secondDimensionSize = b.Array.GetLength(1);
            var totalSum = new T[firstDimensionSize, secondDimensionSize];

            for (int i = 0; i < firstDimensionSize; i++)
            {
                for (int j = 0; j < secondDimensionSize; j++)
                {
                    totalSum[i, j] = (T)((a.Array[i, j] as dynamic) + (b.Array[i, j] as dynamic));
                }
            }

            Matrix<T> matrix = new Matrix<T>(totalSum);
            return matrix;
        }

        public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b)
        {
            int firstMatrixRowCount = a.Array.GetLength(0);
            int firstMatrixColumnCount = a.Array.GetLength(1);

            int secondMatrixRowCount = b.Array.GetLength(0);
            int secondMatrixColumnCount = b.Array.GetLength(1);

            if (firstMatrixRowCount != secondMatrixColumnCount)
            {
                throw new Exception(
                    "The number of rows in the first matrix must be equal to the number of columns in the second one.");
            }

            var result = new T[secondMatrixRowCount, secondMatrixColumnCount];

            for (int i = 0; i < firstMatrixRowCount; i++)
            {
                for (int k = 0; k < secondMatrixColumnCount; k++)
                {
                    T resultColumn = default!;

                    for (int j = 0; j < firstMatrixColumnCount; j++)
                    {
                        resultColumn = (T)((resultColumn as dynamic) +
                                           (a[i, j] as dynamic) * (b[j, k] as dynamic));
                    }

                    result[i, k] = resultColumn!;
                }
            }

            Matrix<T> matrix = new Matrix<T>(result);
            return matrix;
        }

        public T this[int index1, int index2]
        {
            get => Array[index1, index2];
            set => Array[index1, index2] = value;
        }
    }
}