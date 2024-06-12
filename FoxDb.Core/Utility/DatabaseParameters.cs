using FoxDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace FoxDb
{
    public class DatabaseParameters : IDatabaseParameters
    {
        private DatabaseParameters()
        {
            this.ParameterNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public DatabaseParameters(IDatabase database, IDatabaseQuery query, IDataParameterCollection parameters) : this()
        {
            this.Database = database;
            this.Query = query;
            this.Parameters = parameters;
            this.Reset();
        }

        public IDatabase Database { get; private set; }

        public IDatabaseQuery Query { get; private set; }

        public IDataParameterCollection Parameters { get; private set; }

        public IDictionary<string, string> ParameterNames { get; private set; }

        public int Count
        {
            get
            {
                return this.Parameters.Count;
            }
        }

        public bool Contains(string name)
        {
            var parameter = default(IDataParameter);
            return this.Contains(name, out parameter);
        }

        public bool Contains(IColumnConfig column)
        {
            return this.Contains(Conventions.ParameterName(column));
        }

        public bool Contains(string name, out IDataParameter parameter)
        {
            if (this.Parameters.Contains(name))
            {
                parameter = this.Parameters[name] as IDataParameter;
                return true;
            }
            var parameterName = default(string);
            if (this.ParameterNames.TryGetValue(name, out parameterName))
            {
                parameter = this.Parameters[parameterName] as IDataParameter;
                return true;
            }
            for (var a = 0; a < this.Parameters.Count; a++)
            {
                parameter = this.Parameters[a] as IDataParameter;
                if (string.Equals(parameter.ParameterName, name, StringComparison.OrdinalIgnoreCase))
                {
                    this.ParameterNames[name] = parameter.ParameterName;
                    return true;
                }
            }
            parameter = null;
            return false;
        }

        public bool Contains(IColumnConfig column, out IDataParameter parameter)
        {
            return this.Contains(Conventions.ParameterName(column), out parameter);
        }

        public object this[string name]
        {
            get
            {
                var parameter = default(IDataParameter);
                if (!this.Contains(name, out parameter))
                {
                    throw new InvalidOperationException(string.Format("No such parameter \"{0}\".", name));
                }
                if (parameter.Value != null && !DBNull.Value.Equals(parameter.Value))
                {
                    return parameter.Value;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                var parameter = default(IDataParameter);
                if (!this.Contains(name, out parameter))
                {
                    throw new InvalidOperationException(string.Format("No such parameter \"{0}\".", name));
                }
                if (value != null)
                {
                    parameter.Value = value;
                }
                else
                {
                    parameter.Value = DBNull.Value;
                }
            }
        }

        public object this[IColumnConfig column]
        {
            get
            {
                var parameter = default(IDataParameter);
                if (!this.Contains(column, out parameter))
                {
                    throw new InvalidOperationException(string.Format("No such column \"{0}\".", column));
                }
                if (parameter.Value != null && !DBNull.Value.Equals(parameter.Value))
                {
                    return this.Database.Translation.GetLocalValue(column.ColumnType.Type, parameter.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                var parameter = default(IDataParameter);
                if (!this.Contains(column, out parameter))
                {
                    throw new InvalidOperationException(string.Format("No such column \"{0}\".", column));
                }
                if (value != null)
                {
                    parameter.Value = this.Database.Translation.GetRemoteValue(column.ColumnType.Type, value);
                }
                else
                {
                    parameter.Value = DBNull.Value;
                }
            }
        }

        public void Reset()
        {
            foreach (var parameter in this.Query.Parameters)
            {
                if (!this.Contains(parameter.Name))
                {
                    continue;
                }
                this[parameter.Name] = null;
            }
        }
    }
}
