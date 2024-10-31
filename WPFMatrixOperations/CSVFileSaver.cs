using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace WPFMatrixOperations
{
    public static class CSVFileSaver
    {                    
        public static void Save<T>(string filePath, T[,] data)
        {        
            string lineSeparator = Environment.NewLine;
            string columnSeparator = ";";

            string csvData = "";
            int height = data.GetLength(0);
            int width = data.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    csvData += data[i, j];
                    if (j < width - 1)
                    {
                        csvData += columnSeparator;
                    }
                }
                if (i < height - 1)
                {
                    csvData += lineSeparator;
                }
            }
            try
            {
                using StreamWriter writer = new(filePath);
                writer.Write(csvData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static SaveFileDialog ShowSaveFileDialog()
        {
            return new SaveFileDialog()
            {
                FileName = "Matrix.csv",
                DefaultExt = ".cvs",
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
            };
        }
    }
}