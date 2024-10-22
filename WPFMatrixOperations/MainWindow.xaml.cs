using System;
using System.Windows;
using System.Windows.Controls;

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

            btnEnter.IsEnabled = false;
            btnCalculate.IsEnabled = false;
            cbRandomize.IsChecked = true;
            cbSquareMatrix.IsChecked = true;

            SubscribeOnUI();
        }

        private void SubscribeOnUI()
        {
            tbFirstSizeInput.TextChanged += OnSizeInput;
            btnEnter.Click += BtnEnter_Click;
            btnCalculate.Click += OnCalculateSumButtonClick;
            matrixADataGrid.CellEditEnding += OnMatrixCellEdit;
            matrixBDataGrid.CellEditEnding += OnMatrixCellEdit;
        }

        private void OnSizeInput(object sender, TextChangedEventArgs e)
        {
            bool isDigit = int.TryParse(((TextBox)e.Source).Text, out var _);
            btnEnter.IsEnabled = isDigit;
        }

        private void OnMatrixCellEdit(object? sender, DataGridCellEditEndingEventArgs e)
        {
            var x = e.Row.GetIndex();
            var y = e.Column.DisplayIndex;
            var value = Convert.ToInt32(((TextBox)e.EditingElement).Text);
            _matrixController.ChangeValueForMatrixAt((DataGrid)sender, x, y, value);
        }

        private void OnCalculateSumButtonClick(object sender, RoutedEventArgs e)
        {            
            matrixCDataGrid.Columns.Clear();
            matrixCDataGrid.ItemsSource = _matrixController.GetSumData();
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
            var size = Convert.ToInt32(tbFirstSizeInput.Text);

            _matrixController.Size = size;
            _matrixController.Clear();

            ChangeValueForMatrix(matrixADataGrid);
            ChangeValueForMatrix(matrixBDataGrid);
        }

        private void ChangeValueForMatrix(DataGrid matrixDataGrid)
        {
            bool randomize = cbRandomize.IsChecked.Value;

            matrixDataGrid.Columns.Clear();         
            matrixDataGrid.ItemsSource = _matrixController.GetMatrixData(matrixDataGrid, randomize);
        }

        ~MainWindow()
        {
            tbFirstSizeInput.TextChanged += OnSizeInput;
            btnEnter.Click -= BtnEnter_Click;
            btnCalculate.Click -= OnCalculateSumButtonClick;
            matrixADataGrid.CellEditEnding -= OnMatrixCellEdit;
            matrixBDataGrid.CellEditEnding -= OnMatrixCellEdit;
        }
    }
}