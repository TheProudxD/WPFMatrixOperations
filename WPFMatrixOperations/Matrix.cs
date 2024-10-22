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
            var N = b.array.GetLength(0);
            var totalSum = new T[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    totalSum[i, j] = (T)((a.array[i, j] as dynamic) + b.array[i, j]);
                }
            }
            Matrix<T> matrix = new Matrix<T>(totalSum);
            return matrix;
        }

        public T this[int index1, int index2]
        {
            get => array[index1, index2];
            set => array[index1, index2] = value;
        }
    }
}
