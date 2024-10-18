using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFMatrixOperations
{
    public partial class MainWindow : Window
    {
        private Matrix? _matrixA;
        private Matrix? _matrixB;

        private int N;

        public MainWindow()
        {
            InitializeComponent();

            AmendMatrix(matrixADataGrid);
            AmendMatrix(matrixBDataGrid);
            AmendMatrix(matrixCDataGrid);

            btnEnter.Click += BtnEnter_Click;
            btnCalculate.Click += BtnCalculateSum_Click;
            matrixADataGrid.CellEditEnding += MatrixADataGrid_CellEditEnding;
            matrixBDataGrid.CellEditEnding += MatrixBDataGrid_CellEditEnding;
        }

        private void MatrixADataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e) => _matrixA[e.Row.GetIndex(), e.Column.DisplayIndex] = Convert.ToInt32((e.EditingElement as TextBox).Text);

        private void MatrixBDataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e) => _matrixB[e.Row.GetIndex(), e.Column.DisplayIndex] = Convert.ToInt32((e.EditingElement as TextBox).Text);

        private void BtnCalculateSum_Click(object sender, RoutedEventArgs e)
        {
            Matrix resultMatrix = _matrixA + _matrixB;

            matrixCDataGrid.ItemsSource = ConvertArrayToDataTable(resultMatrix.Array).DefaultView;
            matrixCDataGrid.Columns.Clear();

            for (int i = 0; i < N; i++)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Binding = new Binding("[" + i + "]");
                matrixCDataGrid.Columns.Add(column);
            }
        }

        private void AmendMatrix(DataGrid matrixDataGrid)
        {
            matrixDataGrid.CanUserAddRows = false;
            matrixDataGrid.CanUserDeleteRows = true;
            matrixDataGrid.CanUserReorderColumns = true;
            matrixDataGrid.CanUserSortColumns = false;
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            btnCalculate.IsEnabled = true;
            N = Convert.ToInt32(tbSizeInput.Text);

            _matrixA = null;
            _matrixB = null;
            ChangeValueForMatrix(matrixADataGrid);
            ChangeValueForMatrix(matrixBDataGrid);
        }

        private void ChangeValueForMatrix(DataGrid matrixDataGrid)
        {
            bool randomize = cbRandomize.IsChecked.Value;
            int[,] array = new int[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    array[i, j] = randomize ? Random.Shared.Next(10) : 0;
                }
            }

            matrixDataGrid.ItemsSource = ConvertArrayToDataTable(array).DefaultView;

            matrixDataGrid.Columns.Clear();
            for (int i = 0; i < N; i++)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Binding = new Binding("[" + i + "]");
                matrixDataGrid.Columns.Add(column);
            }

            Matrix matrix = new Matrix(array);

            if (_matrixA == null)
                _matrixA = matrix;
            else
                _matrixB = matrix;
        }

        private DataTable ConvertArrayToDataTable(int[,] array)
        {
            DataTable dataTable = new DataTable();

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

            return dataTable;
        }
    }
}