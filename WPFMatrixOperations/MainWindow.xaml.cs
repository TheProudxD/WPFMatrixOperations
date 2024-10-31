using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace WPFMatrixOperations
{
    public partial class MainWindow : Window
    {
        private readonly MatricesController<int> _matrixController = new();
        private readonly Dictionary<DataGrid, MatrixInput> _matrixTable = new();

        public MainWindow()
        {
            InitializeComponent();
            
            AmendMatrix(MatrixADataGrid);
            AmendMatrix(MatrixBDataGrid);
            AmendMatrix(MatrixCDataGrid);

            BtnInput.IsEnabled = false;
            BtnCalculate.IsEnabled = false;
            CbRandomize.IsChecked = true;
            CbSquareMatrixFirstMatrix.IsChecked = true;
            CbSquareMatrixSecondMatrix.IsChecked = true;
            BtnSave.IsEnabled = false;

            CmbCalculationType.SelectedIndex = 0;
            OnCalculationTypeChanged(CmbCalculationType, null!);

            MatrixInput firstMatrixDimension = new(TbFirstSizeInputFirstMatrix, TbSecondSizeInputFirstMatrix, CbSquareMatrixFirstMatrix);
            MatrixInput secondMatrixDimension = new(TbFirstSizeInputSecondMatrix, TbSecondSizeInputSecondMatrix, CbSquareMatrixSecondMatrix);
            
            _matrixTable.Add(MatrixADataGrid, firstMatrixDimension);
            _matrixTable.Add(MatrixBDataGrid, secondMatrixDimension);            
            
            SubscribeOnUI();
        }

        private void SubscribeOnUI()
        {
            foreach (var matrix in _matrixTable)
            {
                matrix.Value.InputChanged += OnInputChanged;
                matrix.Key.CellEditEnding += OnMatrixCellEdited!;
            }

            BtnInput.Click += OnInputButtonClicked;
            BtnCalculate.Click += OnCalculateButtonClicked;
            CmbCalculationType.SelectionChanged += OnCalculationTypeChanged;
            BtnSave.Click += OnSaveButtonClicked;
        }

        private void OnSaveButtonClicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = CSVFileSaver.ShowSaveFileDialog();

            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;

                int[,] data = _matrixController.GetOperationResult();
                CSVFileSaver.Save(filePath, data);
            }
        }

        private void OnCalculationTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ComboBox)sender).SelectedIndex)
            {
                case 0:
                    _matrixController.SetOperation(new SumOperation());
                    break;
                case 1:
                    _matrixController.SetOperation(new MultiplicationOperation());
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

        private void OnMatrixCellEdited(object sender, DataGridCellEditEndingEventArgs e)
        {
            int x = e.Row.GetIndex();
            int y = e.Column.DisplayIndex;
            int value = Convert.ToInt32(((TextBox)e.EditingElement).Text);
            _matrixController.ChangeValueForMatrixAt((DataGrid)sender, x, y, value);
        }

        private void OnCalculateButtonClicked(object sender, RoutedEventArgs e)
        {
            MatrixCDataGrid.Columns.Clear();
            MatrixCDataGrid.ItemsSource = _matrixController.GetOperationResultAsDataView();
            BtnSave.IsEnabled = true;
        }

        private void OnInputButtonClicked(object sender, RoutedEventArgs e)
        {
            BtnCalculate.IsEnabled = true;

            MatrixCDataGrid.Columns.Clear();
            _matrixController.Clear();

            foreach (var matrix in _matrixTable)
            {
                ChangeValueForMatrix(matrix.Key);
            }
        }

        private void ChangeValueForMatrix(DataGrid matrixDataGrid)
        {
            bool randomize = CbRandomize.IsChecked!.Value;
            (int first, int second) = _matrixTable[matrixDataGrid].GetSize();
            
            matrixDataGrid.Columns.Clear();
            matrixDataGrid.ItemsSource = _matrixController.GetMatrixData(matrixDataGrid, randomize, first, second);
            BtnSave.IsEnabled = false;
        }

        private void OnInputChanged() => BtnInput.IsEnabled = _matrixTable[MatrixADataGrid].IsInputValid() && _matrixTable[MatrixBDataGrid].IsInputValid();

        ~MainWindow()
        {
            foreach (var matrix in _matrixTable)
            {
                matrix.Value.InputChanged -= OnInputChanged;
                matrix.Key.CellEditEnding -= OnMatrixCellEdited!;
            }

            BtnInput.Click -= OnInputButtonClicked;
            BtnCalculate.Click -= OnCalculateButtonClicked;
            CmbCalculationType.SelectionChanged -= OnCalculationTypeChanged;
        }
    }
}