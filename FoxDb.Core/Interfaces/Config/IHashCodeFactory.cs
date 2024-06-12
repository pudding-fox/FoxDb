using System.Reflection;

namespace FoxDb.Interfaces
{
    public interface IHashCodeFactory
    {
        IHashCodeConfig Create(IConfig config, ITableConfig table);

        IHashCodeConfig Create(IConfig config, ITableConfig table, PropertyInfo property);
    }
}
