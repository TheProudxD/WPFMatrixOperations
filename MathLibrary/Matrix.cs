namespace MathLibrary;

public class Matrix<T>
{
    private T[,]? Array { get; }

    public int Rows => Array?.GetLength(0) ?? 0;

    public int Columns => Array?.GetLength(1) ?? 0;

    public Matrix(T[,]? array) => Array = array;

    public T[,] GetMatrix() => Array ?? new T[Rows, Columns];

    public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b)
    {
        if (a.GetMatrix().Length == 0 || b.GetMatrix().Length == 0)
        {
            throw new ArgumentException("The matrix's array is empty.");
        }

        int firstMatrixRowCount = a.Rows;
        int firstMatrixColumnCount = a.Columns;
        int secondMatrixRowCount = b.Rows;
        int secondMatrixColumnCount = b.Columns;

        if (firstMatrixRowCount != secondMatrixRowCount || firstMatrixColumnCount != secondMatrixColumnCount)
        {
            throw new ArgumentException(
                "The number of rows in the first matrix must be equal to the number of columns in the second one.");
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
        if (a.GetMatrix().Length == 0 || b.GetMatrix().Length == 0)
        {
            throw new ArgumentException("The matrix's array is empty.");
        }

        int firstMatrixRowCount = a.Rows;
        int firstMatrixColumnCount = a.Columns;
        int secondMatrixRowCount = b.Rows;
        int secondMatrixColumnCount = b.Columns;

        if (firstMatrixColumnCount != secondMatrixRowCount)
        {
            throw new ArgumentException(
                "The number of rows in the first matrix must be equal to the number of columns in the second one.");
        }

        var result = new T[Math.Min(firstMatrixRowCount, secondMatrixRowCount),
            Math.Min(firstMatrixColumnCount, secondMatrixColumnCount)];

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