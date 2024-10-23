using System.Collections.Generic;

namespace WPFMatrixOperations
{
    public interface IOperation
    {
        Matrix<T> Perform<T>((int X, int Y) size, List<Matrix<T>> matrices);
    }
}