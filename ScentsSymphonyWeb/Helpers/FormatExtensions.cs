namespace ScentsSymphonyWeb.Helpers
{
    public static class FormatExtensions
    {
        public static string ToRon(this decimal value)
        {
            return $"{value} RON";
        }

        public static string ToRon(this int value)
        {
            return $"{value} RON";
        }
    }
}