namespace MathLibrary.Operations;

public interface IOperation
{
    Matrix<T> Perform<T>((Matrix<T> MatrixOne, Matrix<T> MatrixTwo) matrices);
}