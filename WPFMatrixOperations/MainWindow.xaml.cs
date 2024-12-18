﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MathLibrary.Operations;
using Microsoft.Win32;
using WPFMatrixOperations.Extensions;

namespace WPFMatrixOperations;

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

        MatrixInput firstMatrixDimension = new(TbFirstSizeInputFirstMatrix, TbSecondSizeInputFirstMatrix,
            CbSquareMatrixFirstMatrix);

        MatrixInput secondMatrixDimension = new(TbFirstSizeInputSecondMatrix, TbSecondSizeInputSecondMatrix,
            CbSquareMatrixSecondMatrix);

        _matrixTable.Add(MatrixADataGrid, firstMatrixDimension);
        _matrixTable.Add(MatrixBDataGrid, secondMatrixDimension);

        SubscribeOnUI();
    }

    private void SubscribeOnUI()
    {
        foreach (KeyValuePair<DataGrid, MatrixInput> matrix in _matrixTable)
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

        if (dialog.ShowDialog() == false)
            return;

        string filePath = dialog.FileName;

        var data = _matrixController.GetOperationResult();
        CSVFileSaver.Save(filePath, data);
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

        if (((TextBox)e.EditingElement).Text.TryParse(out int value) == false)
        {
            MessageBox.Show("Введите число, соответствующего типа", "Внимание!", MessageBoxButton.OK,
                MessageBoxImage.Error);
            CheckLetters();
        }
        else
        {
            _matrixController.ChangeValueForMatrixAt((DataGrid)sender, x, y, value);
        }
    }

    private void CheckLetters()
    {
        bool isAllLettersDigits = true;

        foreach (DataView dv in _matrixController
                     .GetAllDataGrids()
                     .Select(dataGrid => dataGrid.ItemsSource as DataView))
        {
            DataTable table = dv.Table;

            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (dv[i].Row[j].ToString()!.TryParse<int>(out _))
                        continue;

                    isAllLettersDigits = false;
                    break;
                }
            }
        }

        BtnCalculate.IsEnabled = isAllLettersDigits;
    }

    private void OnCalculateButtonClicked(object sender, RoutedEventArgs e)
    {
        MatrixCDataGrid.Columns.Clear();
        Stopwatch stopwatch = Stopwatch.StartNew();
        DataView result = _matrixController.GetOperationResultAsDataView();
        stopwatch.Stop();

        MatrixCDataGrid.ItemsSource = result;
        tbTimer.Text = stopwatch.Elapsed.TotalMilliseconds + "мс";
        BtnSave.IsEnabled = true;
    }

    private void OnInputButtonClicked(object sender, RoutedEventArgs e)
    {
        MatrixCDataGrid.Columns.Clear();
        _matrixController.Clear();

        foreach (KeyValuePair<DataGrid, MatrixInput> matrix in _matrixTable)
        {
            ChangeValueForMatrix(matrix.Key);
        }

        CheckLetters();
    }

    private void ChangeValueForMatrix(DataGrid matrixDataGrid)
    {
        bool randomize = CbRandomize.IsChecked!.Value;
        (int first, int second) = _matrixTable[matrixDataGrid].GetSize();

        matrixDataGrid.Columns.Clear();
        matrixDataGrid.ItemsSource = _matrixController.GetMatrixData(matrixDataGrid, randomize, first, second);
        BtnSave.IsEnabled = false;
    }

    private void OnInputChanged() => BtnInput.IsEnabled =
        _matrixTable[MatrixADataGrid].IsInputValid() && _matrixTable[MatrixBDataGrid].IsInputValid();

    ~MainWindow()
    {
        foreach (KeyValuePair<DataGrid, MatrixInput> matrix in _matrixTable)
        {
            matrix.Value.InputChanged -= OnInputChanged;
            matrix.Key.CellEditEnding -= OnMatrixCellEdited!;
        }

        BtnInput.Click -= OnInputButtonClicked;
        BtnCalculate.Click -= OnCalculateButtonClicked;
        CmbCalculationType.SelectionChanged -= OnCalculationTypeChanged;
    }
}