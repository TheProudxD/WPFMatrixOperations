using System.Collections.Generic;

namespace WPFMatrixOperations
{
    public class SumOperation : IOperation
    {
        Matrix<T> IOperation.Perform<T>((int X, int Y) size, List<Matrix<T>> matrices)
        {
            Matrix<T> resultMatrix = new(new T[size.X, size.Y]);
            matrices.ForEach(x => resultMatrix += x);
            return resultMatrix;
        }
    }
}