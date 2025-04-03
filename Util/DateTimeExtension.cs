namespace TerracoDaCida.Util
{
    public static class DateTimeExtension
    {
        public static DateTime GetDataAtual(this DateTime dataAtual)
        {
            TimeZoneInfo brZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            dataAtual = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brZone);
            return dataAtual;
        }
    }
}
