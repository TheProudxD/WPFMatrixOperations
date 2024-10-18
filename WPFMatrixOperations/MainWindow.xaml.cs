using System;
using System.Data;
using System.Diagnostics.Metrics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFMatrixOperations
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AmendMatrix(matrix1DataGrid);
            AmendMatrix(matrix2DataGrid);

            btnEnter.Click += BtnEnter_Click;
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
            int N = Convert.ToInt32(tbSizeInput.Text);

            ChangeValueForMatrix(N, matrix1DataGrid);
            ChangeValueForMatrix(N, matrix2DataGrid);
        }

        private void ChangeValueForMatrix(int N, DataGrid matrixDataGrid)
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
/*
public partial class MainWindow : Window
{
    public Matrix MatrixInstance { get; set; }

    public MainWindow()
    {
        InitializeComponent();

        int[,] array = new int[,]
        {
        {1, 2, 3},
        {4, 5, 6},
        {7, 8, 9}
        };

        MatrixInstance = new Matrix(array);

        DataGrid dataGrid = new DataGrid();
        dataGrid.AutoGenerateColumns = false;
        dataGrid.ItemsSource = MatrixInstance.Data;

        int counter = 0;
        for (int i = 0; i < array.GetLength(0); i++)
        {
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Column " + (i + 1);
            column.Binding = new Binding($"Value");
            dataGrid.Columns.Add(column);
            counter++;
        }

        Content = dataGrid;
    }
}
public class MatrixItem
{
    public int Row { get; set; }
    public int Column { get; set; }
    public int Value { get; set; }
}*/
/*

         public MainWindow()
        {
            this.MatrixSize = Enumerable.Range(1, 10).ToArray();
            this.DataContext = this;
        }
        public IList MatrixSize { get; private set; }
        public object Matrix { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var size = (int)e.AddedItems[0];
            this.UpdateMatrix(size);
        }

        void UpdateMatrix(int size)
        {
            var dt = new DataTable();
            for (var i = 0; i < size; i++)
                dt.Columns.Add(new DataColumn("c" + i, typeof(string)));
            for (var i = 0; i < size; i++)
            {
                var r = dt.NewRow();
                for (var c = 0; c < size; c++)
                    r[c] = "hello";
                dt.Rows.Add(r);
            }
            this.Matrix = dt.DefaultView;
            PropertyChanged(this, new PropertyChangedEventArgs("Matrix"));
        }
    }*/
