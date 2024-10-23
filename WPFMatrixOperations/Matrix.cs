using System;

namespace WPFMatrixOperations
{
    public class Matrix<T>
    {
        private readonly T[,] array;
        public T[,] Array => array;

        public Matrix(T[,] array)
        {
            this.array = array;
        }

        public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b)
        {
            var firstDimensionSize = b.array.GetLength(0);
            var secondDimensionSize = b.array.GetLength(1);
            var totalSum = new T[firstDimensionSize, secondDimensionSize];

            for (int i = 0; i < firstDimensionSize; i++)
            {
                for (int j = 0; j < secondDimensionSize; j++)
                {
                    totalSum[i, j] = (T)((a.array[i, j] as dynamic) + (b.array[i, j] as dynamic));
                }
            }
            Matrix<T> matrix = new Matrix<T>(totalSum);
            return matrix;
        }

        public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b)
        {
            var firstMatrixRowCount = a.array.GetLength(0);
            var firstMatrixColumnCount = a.array.GetLength(1); 
            
            var secondMatrixRowCount = b.array.GetLength(0);
            var secondMatrixColumnCount = b.array.GetLength(1);

            if (firstMatrixRowCount!=secondMatrixColumnCount)
            {
                throw new Exception("The number of rows in the first matrix must be equal to the number of columns in the second one.");
            }

            var result = new T[secondMatrixRowCount, secondMatrixColumnCount];

            for (int i = 0; i < firstMatrixRowCount; i++)
            {
                for (int k = 0; k < secondMatrixColumnCount; k++)
                {                        
                    T resultColumn = default;

                    for (int j = 0; j < firstMatrixColumnCount; j++)
                    {
                        resultColumn = (T)((resultColumn as dynamic) + (a.array[i, j] as dynamic) * (b.array[j, k] as dynamic));
                    }
                    result[i, k] = resultColumn;
                }
            }
            Matrix<T> matrix = new Matrix<T>(result);
            return matrix;
        }

        public T this[int index1, int index2]
        {
            get => array[index1, index2];
            set => array[index1, index2] = value;
        }
    }
}
