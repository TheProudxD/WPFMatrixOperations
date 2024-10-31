using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Controls;

namespace WPFMatrixOperations
{
    public class MatricesController<T>
        where T : struct
    {
        private readonly Dictionary<DataGrid, Matrix<T>> _matrixTable = new();

        private IOperation _operation = null!;

        private void AddMatrixDataGrid(DataGrid matrixDataGrid, T[,] array)
        {
            if (_matrixTable.TryAdd(matrixDataGrid, new Matrix<T>(array)) == false)
            {
                throw new Exception("Duplicate Matrix data grid");
            }
        }

        private T[,] CreateDataArray(DataGrid dataGrid, bool randomize, int firstSize, int secondSize, int maxValue = 10)
        {
            Random random = new();
            T[,] array = new T[firstSize, secondSize];

            for (int i = 0; i < firstSize; i++)
            {
                for (int j = 0; j < secondSize; j++)
                {
                    T value;

                    if (typeof(T) == typeof(int))
                    {
                        value = (T)(object)random.Next(maxValue);
                    }
                    else if (typeof(T) == typeof(double))
                    {
                        value = (T)(object)(random.NextDouble() * maxValue);
                    }
                    else if (typeof(T) == typeof(float))
                    {
                        value = (T)(object)(random.NextSingle() * maxValue);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Unsupported data type");
                    }

                    array[i, j] = randomize ? value : default(T);
                }
            }

            return array;
        }

        public DataView ConvertArrayToDataTable(T[,] array)
        {
            if (array == null)
                return null;
            DataTable dataTable = new();

            for (int i = 0; i < array.GetLength(1); i++)
            {
                dataTable.Columns.Add("Column " + (i + 1));
            }

            for (int i = 0; i < array.GetLength(0); i++)
            {
                DataRow row = dataTable.NewRow();

                for (int j = 0; j < array.GetLength(1); j++)
                {
                    row[j] = array[i, j];
                }

                dataTable.Rows.Add(row);
            }

            return dataTable.DefaultView;
        }

        public T[,] GetOperationResult()
        {
            List<Matrix<T>> matrices = _matrixTable.Values.ToList();

            if (_operation == null)
                throw new Exception("No operation setup");

            Matrix<T> operationResult = _operation.Perform((matrices[0], matrices[1]));
            return operationResult?.Array;
        }

        public DataView GetOperationResultAsDataView() => ConvertArrayToDataTable(GetOperationResult());

        public DataView GetMatrixData(DataGrid dataGrid, bool randomize, int firstSize, int secondSize)
        {
            T[,] array = CreateDataArray(dataGrid, randomize, firstSize, secondSize);
            AddMatrixDataGrid(dataGrid, array);            
            _matrixTable[dataGrid].Size = (firstSize, secondSize);
            return ConvertArrayToDataTable(array);
        }

        public void ChangeValueForMatrixAt(DataGrid dataGrid, int x, int y, T value)
        {
            Matrix<T> matrix = _matrixTable[dataGrid];
            matrix[x, y] = value;
        }

        public void SetOperation(IOperation operation) => _operation = operation;

        public void Clear() => _matrixTable.Clear();
    }
}