using FoxDb.Interfaces;

namespace FoxDb
{
    public static class EntityHashCode
    {
        public static int GetHashCode(ITableConfig table, object item)
        {
            var hash1 = (5381 << 16) + 5381;
            var hash2 = hash1;
            unchecked
            {
                foreach (var column in table.Columns)
                {
                    var value = column.Getter(item);
                    if (value == null)
                    {
                        continue;
                    }
                    hash1 = ((hash1 << 5) + hash1) ^ GetHashCode(value);
                }
            }
            return hash1 + (hash2 * 1566083941);
        }

        private static int GetHashCode(object value)
        {
            if (value is string text)
            {
                return text.GetDeterministicHashCode();
            }
            return value.GetHashCode();
        }

        public static void SetHashCode(ITableConfig table, object item)
        {
            var hashCode = GetHashCode(table, item);
            table.HashCode.Setter(item, hashCode);
        }
    }
}
