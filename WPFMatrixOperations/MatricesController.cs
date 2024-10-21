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

        private int _matrixSize;

        public void SetMatricesSize(int size) => _matrixSize = size;

        private T[,] CreateDataArray(bool randomize)
        {
            int max = 10;
            Random random = new Random();
            T[,] array = new T[_matrixSize, _matrixSize];
            for (int i = 0; i < _matrixSize; i++)
            {
                for (int j = 0; j < _matrixSize; j++)
                {
                    T value = default;

                    if (typeof(T) == typeof(int))
                    {
                        value = (T)(object)random.Next(max);
                    }
                    else if (typeof(T) == typeof(double))
                    {
                        value = (T)(object)(random.NextDouble()* max);
                    }
                    else if (typeof(T) == typeof(float))
                    {
                        value = (T)(object)(random.NextSingle() * max);
                    }
                    else if (typeof(T) == typeof(long))
                    {
                        value = (T)(object)random.NextInt64(max);
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

        public void Add(DataGrid matrixDataGrid, T[,] array)
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

        public void Clear()
        {
            _matrixTable.Clear();
        }

        public DataView GetSumData() => ConvertArrayToDataTable(FindSum().Array);

        public DataView GetMatrixData(DataGrid dataGrid, bool randomize)
        {
            var array = CreateDataArray(randomize);
            Matrix<T> matrix = new(array);
            _matrixTable.Add(dataGrid, matrix);
            return ConvertArrayToDataTable(array);
        }

        private Matrix<T> FindSum()
        {
            var N = _matrixTable.First().Value.Array.Length;
            Matrix<T> resultMatrix = new(new T[N, N]);
            _matrixTable.Values.ToList().ForEach(x => resultMatrix += x);
            return resultMatrix;
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

        public void ChangeValueForMatrixAt(DataGrid? sender, int x, int y, T value)
        {
            var matrix = _matrixTable[sender];
            matrix[x, y] = value;
        }
    }
}