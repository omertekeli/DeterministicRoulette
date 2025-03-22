namespace Runtime.Utilies
{
    public static class NumberFormatter
    {
        public static string FormatWithCommas(int number)
        {
            return number.ToString("N0"); // Virgüllü format
        }

        public static string FormatWithCommas(float number)
        {
            return number.ToString("#,##0.##"); // Ondalıklı format (virgül ile)
        }
    }
}