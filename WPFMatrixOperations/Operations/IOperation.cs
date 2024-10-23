﻿using System.Collections.Generic;

namespace WPFMatrixOperations
{
    public interface IOperation
    {
        Matrix<T> Perform<T>((Matrix<T> MatrixOne, Matrix<T> MatrixTwo) matrices);
    }
}