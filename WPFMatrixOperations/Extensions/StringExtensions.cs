namespace WPFMatrixOperations.Extensions
{
    public static class StringExtensions
    {
        public static bool IsDigit(this string content, out int result) => int.TryParse(content, out result);

        public static bool MoreThanZero(this int result) => result > 0;
    }
}