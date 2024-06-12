using System;
using System.Reflection;

namespace FoxDb.Interfaces
{
    public interface IHashCodeConfig : IEquatable<IHashCodeConfig>
    {
        IConfig Config { get; }

        ITableConfig Table { get; }

        PropertyInfo Property { get; }

        Func<object, object> Getter { get; }

        Action<object, object> Setter { get; }
    }
}
