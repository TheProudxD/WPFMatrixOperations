using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WPFMatrixOperations
{
    public partial class MainWindow : Window
    {
        private readonly MatricesController<int> _matrixController = new();

        public MainWindow()
        {
            InitializeComponent();

            AmendMatrix(matrixADataGrid);
            AmendMatrix(matrixBDataGrid);
            AmendMatrix(matrixCDataGrid);

            btnEnter.Click += BtnEnter_Click;
            btnCalculate.Click += BtnCalculateSum_Click;
            matrixADataGrid.CellEditEnding += MatrixDataGrid_CellEditEnding;
            matrixBDataGrid.CellEditEnding += MatrixDataGrid_CellEditEnding;
        }

        private void MatrixDataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
        {
            var x = e.Row.GetIndex();
            var y = e.Column.DisplayIndex;
            var value = Convert.ToInt32(((TextBox)e.EditingElement).Text);
            _matrixController.ChangeValueForMatrixAt((DataGrid)sender, x, y, value);
        }

        private void BtnCalculateSum_Click(object sender, RoutedEventArgs e)
        {
            matrixCDataGrid.ItemsSource = _matrixController.GetSumData();
            matrixCDataGrid.Columns.Clear();

            for (int i = 0; i < 2; i++)
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
            var size = Convert.ToInt32(tbSizeInput.Text);

            _matrixController.SetMatricesSize(size);
            _matrixController.Clear();
            ChangeValueForMatrix(matrixADataGrid);
            ChangeValueForMatrix(matrixBDataGrid);
        }

        private void ChangeValueForMatrix(DataGrid matrixDataGrid)
        {
            bool randomize = cbRandomize.IsChecked.Value;

            matrixDataGrid.Columns.Clear();

            for (int i = 0; i < 2; i++)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Binding = new Binding("[" + i + "]");
                matrixDataGrid.Columns.Add(column);
            }            
            
            matrixDataGrid.ItemsSource = _matrixController.GetMatrixData(matrixDataGrid, randomize);
        }
    }
}