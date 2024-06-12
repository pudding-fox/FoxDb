using FoxDb.Interfaces;
using System;
using System.Reflection;

namespace FoxDb
{
    public class HashCodeConfig : IHashCodeConfig
    {
        public HashCodeConfig(IConfig config, ITableConfig table, PropertyInfo property, Func<object, object> getter, Action<object, object> setter)
        {
            this.Config = config;
            this.Table = table;
            this.Property = property;
            this.Getter = getter;
            this.Setter = setter;
        }

        public IConfig Config { get; private set; }

        public ITableConfig Table { get; private set; }

        public PropertyInfo Property { get; set; }

        public Func<object, object> Getter { get; private set; }

        public Action<object, object> Setter { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}.{1}", this.Table, this.Property.Name);
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                hashCode += this.Table.GetHashCode();
                hashCode += this.Property.Name.GetHashCode();
            }
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj is IHashCodeConfig)
            {
                return this.Equals(obj as IHashCodeConfig);
            }
            return base.Equals(obj);
        }

        public bool Equals(IHashCodeConfig other)
        {
            if (other == null)
            {
                return false;
            }
            if ((TableConfig)this.Table != (TableConfig)other.Table)
            {
                return false;
            }
            if (this.Property != other.Property)
            {
                return false;
            }
            return true;
        }

        public static bool operator ==(HashCodeConfig a, HashCodeConfig b)
        {
            if ((object)a == null && (object)b == null)
            {
                return true;
            }
            if ((object)a == null || (object)b == null)
            {
                return false;
            }
            if (object.ReferenceEquals((object)a, (object)b))
            {
                return true;
            }
            return a.Equals(b);
        }

        public static bool operator !=(HashCodeConfig a, HashCodeConfig b)
        {
            return !(a == b);
        }
    }
}
