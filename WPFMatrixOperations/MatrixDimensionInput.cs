using System;
using System.Windows;
using System.Windows.Controls;
using WPFMatrixOperations.Extensions;

namespace WPFMatrixOperations
{
    public class MatrixInput
    {
        private bool _isFirstInputValid;
        private bool _isSecondInputValid;
        private bool _isSquareMatrix;

        private readonly TextBox _firstDimensionInputTextBox;
        private readonly TextBox _secondDimensionInputTextBox;
        private readonly CheckBox _squareMatrixCheckBox;

        public event Action InputChanged;

        public MatrixInput(TextBox firstDimensionInputTextBox, TextBox secondDimensionInputTextBox, CheckBox squareMatrixcheckBox)
        {
            _firstDimensionInputTextBox = firstDimensionInputTextBox;
            _secondDimensionInputTextBox = secondDimensionInputTextBox;
            _squareMatrixCheckBox = squareMatrixcheckBox;

            firstDimensionInputTextBox.TextChanged += OnSizeInput;
            secondDimensionInputTextBox.TextChanged += OnSizeInput;
            _squareMatrixCheckBox.Click += OnSquareMatrixChecked;

            OnSquareMatrixChecked(null!, null!);
        }

        private void OnSizeInput(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)e.Source;
            string content = textBox.Text;
            bool valid = content.IsDigit(out int result) && result.MoreThanZero();

            if (textBox == _firstDimensionInputTextBox)
            {
                _isFirstInputValid = valid;
            }
            else if (textBox == _secondDimensionInputTextBox)
            {
                _isSecondInputValid = valid;
            }

            InputChanged?.Invoke();
        }


        private void OnSquareMatrixChecked(object sender, RoutedEventArgs e)
        {
            _isSquareMatrix = _squareMatrixCheckBox.IsChecked!.Value;
            _secondDimensionInputTextBox.Visibility = _isSquareMatrix ? Visibility.Hidden : Visibility.Visible;
            InputChanged?.Invoke();
        }

        public (int firstSize, int secondSize) GetSize()
        {
            int firstSize = Convert.ToInt32(_firstDimensionInputTextBox.Text);
            if (_isSquareMatrix)
            {
                return (firstSize, firstSize);
            }

            int secondSize = Convert.ToInt32(_secondDimensionInputTextBox.Text);
            return (firstSize, secondSize);
        }

        public bool IsInputValid() => _isSquareMatrix ? _isFirstInputValid : _isFirstInputValid && _isSecondInputValid;
    }
}