namespace Architecture
{
    public class SystemDateTime
    {
        private static Func<DateTime> _utcNowFunc = () => default;

        public static DateTime UtcNow => _utcNowFunc();

        public static void InitUtcNow(Func<DateTime> func) => _utcNowFunc = func;
    }
}