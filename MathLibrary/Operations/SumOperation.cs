using System.Windows;

namespace MathLibrary.Operations;

public class SumOperation : IOperation
{
    Matrix<T>? IOperation.Perform<T>((Matrix<T> MatrixOne, Matrix<T> MatrixTwo) matrices)
    {
        int firstMatrixRowCount = matrices.MatrixOne.Rows;
        int firstMatrixColumnCount = matrices.MatrixOne.Columns;
        int secondMatrixRowCount = matrices.MatrixTwo.Rows;
        int secondMatrixColumnCount = matrices.MatrixTwo.Columns;

        if (firstMatrixRowCount != secondMatrixRowCount || firstMatrixColumnCount != secondMatrixColumnCount)
        {
            MessageBox.Show("Размерность матриц должна быть одинаковой", "Внимание!", MessageBoxButton.OK,
                MessageBoxImage.Error);

            return null;
        }

        return matrices.MatrixOne + matrices.MatrixTwo;
    }
}