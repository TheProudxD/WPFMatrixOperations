using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WPFMatrixOperations
{
    public class MatricesController<T>
        where T : struct
    {
        private readonly Dictionary<DataGrid, Matrix<T>> _matrixTable = new();
        private IOperation _operation;

        public (int X, int Y) Size { get; set; }

        private T[,] CreateDataArray(bool randomize, int maxValue = 10)
        {
            Random random = new();
            T[,] array = new T[Size.X, Size.Y];
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    T value;

                    if (typeof(T) == typeof(int))
                    {
                        value = (T)(object)random.Next(maxValue);
                    }
                    else if (typeof(T) == typeof(double))
                    {
                        value = (T)(object)(random.NextDouble()* maxValue);
                    }
                    else if (typeof(T) == typeof(float))
                    {
                        value = (T)(object)(random.NextSingle() * maxValue);
                    }
                    else if (typeof(T) == typeof(long))
                    {
                        value = (T)(object)random.NextInt64(maxValue);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    array[i, j] = randomize ? value : default;
                }
            }

            return array;
        }

        private void Add(DataGrid matrixDataGrid, T[,] array)
        {
            Matrix<T> matrix = new(array);
            if (_matrixTable.ContainsKey(matrixDataGrid) == false)
            {
                _matrixTable.Add(matrixDataGrid, matrix);
            }
            else
            {
                throw new Exception();
            }
        }

        public void Clear() => _matrixTable.Clear();

        public DataView GetOperationResult() => ConvertArrayToDataTable(_operation.Perform(Size, _matrixTable.Values.ToList()).Array);

        public DataView GetMatrixData(DataGrid dataGrid, bool randomize)
        {
            var array = CreateDataArray(randomize);
            Add(dataGrid, array);
            return ConvertArrayToDataTable(array);
        }

        public DataView ConvertArrayToDataTable(T[,] array)
        {
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

        public void ChangeValueForMatrixAt(DataGrid dataGrid, int x, int y, T value)
        {
            var matrix = _matrixTable[dataGrid];
            matrix[x, y] = value;
        }

        public void SetOperation(IOperation operation)
        {
            _operation = operation;
        }
    }
}