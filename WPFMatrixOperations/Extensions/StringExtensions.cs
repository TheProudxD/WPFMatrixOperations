using System;

namespace WPFMatrixOperations.Extensions
{
    public static class StringExtensions
    {
        public static bool IsDigit(this string content, out int result) => int.TryParse(content, out result);

        public static bool MoreThanZero(this int result) => result > 0;

        public static bool TryParse<T>(this string text, out T? result)
        {
            result = default(T);

            if (typeof(T) == typeof(int))
            {
                bool success = int.TryParse(text, out int res);
                result = (T)(object)res;
                return success;
            }

            if (typeof(T) == typeof(double))
            {
                bool success = double.TryParse(text, out double res);
                result = (T)(object)res;
                return success;
            }

            if (typeof(T) == typeof(float))
            {
                bool success = float.TryParse(text, out float res);
                result = (T)(object)res;
                return success;
            }

            throw new ArgumentOutOfRangeException("Unsupported data type");
        }
    }
}