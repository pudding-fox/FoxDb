using System.Reflection;

namespace FoxDb
{
    public static class HashCodeValidator
    {
        public static bool Validate(PropertyInfo property)
        {
            if (property == null)
            {
                return false;
            }
            var attribute = property.GetCustomAttribute<HashCodeAttribute>(true);
            if (attribute == null)
            {
                return false;
            }
            if (property.GetGetMethod() == null || property.GetSetMethod() == null || property.PropertyType != typeof(int))
            {
                return false;
            }
            return true;
        }
    }
}
