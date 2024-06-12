using FoxDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FoxDb
{
    public class HashCodeFactory : IHashCodeFactory
    {
        public IHashCodeConfig Create(IConfig config, ITableConfig table)
        {
            var properties = new EntityPropertyEnumerator(table.TableType);
            foreach (var property in properties)
            {
                if (!HashCodeValidator.Validate(property))
                {
                    continue;
                }
                return Create(config, table, property);
            }
            return null;
        }

        public IHashCodeConfig Create(IConfig config, ITableConfig table, PropertyInfo property)
        {
            var columnType = Factories.Type.Create(TypeConfig.By(property));
            var accessor = Factories.PropertyAccessor.Column.Create<object, object>(property, columnType);
            return new HashCodeConfig(config, table, property, accessor.Get, accessor.Set);
        }
    }
}
