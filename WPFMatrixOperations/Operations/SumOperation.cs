namespace WPFMatrixOperations;

public class SumOperation : IOperation
{
    Matrix<T> IOperation.Perform<T>((Matrix<T> MatrixOne, Matrix<T> MatrixTwo) matrices) => matrices.MatrixOne + matrices.MatrixTwo;
}