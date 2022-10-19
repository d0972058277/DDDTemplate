namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
{
    public class RandomString
    {
        public static string Get(int length)
        {
            var random = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

        public static IReadOnlyList<string> Gets(int length, int count)
        {
            return Enumerable.Range(1, count).Select(i => Get(length)).ToList();
        }
    }
}