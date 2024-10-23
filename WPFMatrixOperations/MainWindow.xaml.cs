using System;
using System.Windows;
using System.Windows.Controls;
using WPFMatrixOperations.Extensions;

namespace WPFMatrixOperations
{
    public partial class MainWindow : Window
    {
        private readonly MatricesController<int> _matrixController = new();

        private bool _isFirstInputEntered;
        private bool _isSquareMatrix;
        private bool _isSecondInputEntered;

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

            OnSquareMatrixChecked(null, null);
            cmbCalculationType.SelectedIndex = 0;
            SubscribeOnUI();
            OnCalculationTypeChanged(cmbCalculationType, null);
        }

        private void SubscribeOnUI()
        {
            cbSquareMatrix.Click += OnSquareMatrixChecked;
            tbFirstSizeInput.TextChanged += OnSizeInput;
            tbSecondSizeInput.TextChanged += OnSizeInput;
            btnEnter.Click += OnCalculateButtonClick;
            btnCalculate.Click += OnCalculateSumButtonClick;
            matrixADataGrid.CellEditEnding += OnMatrixCellEdit;
            matrixBDataGrid.CellEditEnding += OnMatrixCellEdit;
            cmbCalculationType.SelectionChanged += OnCalculationTypeChanged;
        }

        private void OnCalculationTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ComboBox)sender).SelectedIndex)
            {
                case 0:
                    _matrixController.SetOperation(new SumOperation());
                    break;
                case 1:
                    _matrixController.SetOperation(new SumOperation());
                    break;
                default:
                    MessageBox.Show("Выбрана не поддерживаемая операция", "Внимание!");
                    throw new Exception("Выбрана не поддерживаемая операция");
            }
        }

        private void AmendMatrix(DataGrid matrixDataGrid)
        {
            matrixDataGrid.CanUserAddRows = false;
            matrixDataGrid.CanUserDeleteRows = false;
            matrixDataGrid.CanUserReorderColumns = true;
            matrixDataGrid.CanUserSortColumns = false;
        }

        private void OnSquareMatrixChecked(object sender, RoutedEventArgs e)
        {
            _isSquareMatrix = cbSquareMatrix.IsChecked.Value;
            tbSecondSizeInput.Visibility = _isSquareMatrix ? Visibility.Hidden : Visibility.Visible;
            ChangeEnterButtonState();
        }

        private void OnSizeInput(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)e.Source;
            string content = textBox.Text;
            bool valid = content.IsDigit(out int result) && result.MoreThanZero();

            if (textBox == tbFirstSizeInput)
            {
                _isFirstInputEntered = valid;
            }
            else if (textBox == tbSecondSizeInput)
            {
                _isSecondInputEntered = valid;
            }

            ChangeEnterButtonState();
        }

        private void ChangeEnterButtonState() => btnEnter.IsEnabled = _isSquareMatrix ? _isFirstInputEntered : _isFirstInputEntered && _isSecondInputEntered;

        private void OnMatrixCellEdit(object? sender, DataGridCellEditEndingEventArgs e)
        {
            int x = e.Row.GetIndex();
            int y = e.Column.DisplayIndex;
            int value = Convert.ToInt32(((TextBox)e.EditingElement).Text);
            _matrixController.ChangeValueForMatrixAt((DataGrid)sender, x, y, value);
        }

        private void OnCalculateSumButtonClick(object sender, RoutedEventArgs e)
        {            
            matrixCDataGrid.Columns.Clear();
            matrixCDataGrid.ItemsSource = _matrixController.GetOperationResult();
        }

        private void OnCalculateButtonClick(object sender, RoutedEventArgs e)
        {
            btnCalculate.IsEnabled = true;
            SetMatrixSize();

            matrixCDataGrid.Columns.Clear();
            _matrixController.Clear();

            ChangeValueForMatrix(matrixADataGrid);
            ChangeValueForMatrix(matrixBDataGrid);
        }

        private void SetMatrixSize()
        {
            int firstSize = Convert.ToInt32(tbFirstSizeInput.Text);

            if (_isSquareMatrix)
            {
                _matrixController.Size = (firstSize, firstSize);
            }
            else
            {
                int secondSize = Convert.ToInt32(tbSecondSizeInput.Text);
                _matrixController.Size = (firstSize, secondSize);
            }
        }

        private void ChangeValueForMatrix(DataGrid matrixDataGrid)
        {
            bool randomize = cbRandomize.IsChecked.Value;

            matrixDataGrid.Columns.Clear();         
            matrixDataGrid.ItemsSource = _matrixController.GetMatrixData(matrixDataGrid, randomize);
        }

        ~MainWindow()
        {
            cbSquareMatrix.Checked -= OnSquareMatrixChecked;
            tbFirstSizeInput.TextChanged -= OnSizeInput;
            tbSecondSizeInput.TextChanged -= OnSizeInput;
            btnEnter.Click -= OnCalculateButtonClick;
            btnCalculate.Click -= OnCalculateSumButtonClick;
            matrixADataGrid.CellEditEnding -= OnMatrixCellEdit;
            matrixBDataGrid.CellEditEnding -= OnMatrixCellEdit;
        }
    }
}