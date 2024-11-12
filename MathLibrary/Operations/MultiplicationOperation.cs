using System.Windows;

namespace MathLibrary.Operations;

public class MultiplicationOperation : IOperation
{
    Matrix<T>? IOperation.Perform<T>((Matrix<T> MatrixOne, Matrix<T> MatrixTwo) matrices)
    {
        if (matrices.MatrixOne.Columns != matrices.MatrixTwo.Rows)
        {
            MessageBox.Show("Количество строк во второй матрице должно быть равно количеству столбцов во первой",
                "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);

            return null;
        }

        return matrices.MatrixOne * matrices.MatrixTwo;
    }
}