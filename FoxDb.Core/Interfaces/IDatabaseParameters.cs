using System.Data;

namespace FoxDb.Interfaces
{
    public interface IDatabaseParameters
    {
        IDatabase Database { get; }

        IDatabaseQuery Query { get; }

        int Count { get; }

        bool Contains(string name);

        bool Contains(IColumnConfig column);

        bool Contains(string name, out IDataParameter parameter);

        bool Contains(IColumnConfig column, out IDataParameter parameter);

        void Reset();

        object this[string name] { get; set; }

        object this[IColumnConfig column] { get; set; }
    }
}
