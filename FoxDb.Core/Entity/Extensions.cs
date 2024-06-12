namespace FoxDb
{
    public static partial class Extensions
    {
        public static int GetDeterministicHashCode(this string value)
        {
            unchecked
            {
                var hash1 = (5381 << 16) + 5381;
                var hash2 = hash1;
                for (var a = 0; a < value.Length; a += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ value[a];
                    if (a == value.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ value[a + 1];
                }
                return hash1 + (hash2 * 1566083941);
            }
        }
    }
}
