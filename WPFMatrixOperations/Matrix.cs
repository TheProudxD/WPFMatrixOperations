using System;
using System.Windows;

namespace WPFMatrixOperations
{
    public class Matrix<T>
    {
        public T[,] Array { get; }

        public (int X, int Y) Size { get; set; }

        public Matrix(T[,] array) => Array = array;

        public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b)
        {
            int firstMatrixRowCount = a.Array.GetLength(0);
            int firstMatrixColumnCount = a.Array.GetLength(1);

            int secondMatrixRowCount = b.Array.GetLength(0);
            int secondMatrixColumnCount = b.Array.GetLength(1);

            if (firstMatrixRowCount != secondMatrixRowCount || firstMatrixColumnCount != secondMatrixColumnCount)
            {
                MessageBox.Show("Размерность матриц должна быть одинаковой", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
                //throw new Exception("The number of rows in the first matrix must be equal to the number of columns in the second one.");
            }

            var totalSum = new T[secondMatrixRowCount, secondMatrixColumnCount];

            for (int i = 0; i < secondMatrixRowCount; i++)
            {
                for (int j = 0; j < secondMatrixColumnCount; j++)
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

            if (firstMatrixColumnCount != secondMatrixRowCount)
            {
                MessageBox.Show("Количество строк во второй матрице должно быть равно количеству столбцов во первой", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
                //throw new Exception("The number of rows in the first matrix must be equal to the number of columns in the second one.");
            }

            var result = new T[Math.Min(firstMatrixRowCount, secondMatrixRowCount), Math.Min(firstMatrixColumnCount, secondMatrixColumnCount)];

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