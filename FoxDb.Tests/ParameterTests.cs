#pragma warning disable 612, 618
using NUnit.Framework;
using System.Data;

namespace FoxDb
{
    [TestFixture(ProviderType.SqlCe)]
    [TestFixture(ProviderType.SqlServer)]
    [TestFixture(ProviderType.SqlServer2012)]
    [TestFixture(ProviderType.SQLite)]
    public class ParameterTests : TestBase
    {
        public ParameterTests(ProviderType providerType)
            : base(providerType)
        {

        }

        [Test]
        public void ParameterNamesAreCaseInsensitive()
        {
            var table = this.Database.Config.Table<Test001>();
            var builder = this.Database.QueryFactory.Build();
            builder.Add.AddColumns(table.InsertableColumns);
            builder.Add.SetTable(table);
            builder.Output.AddParameters(table.Columns);
            var query = builder.Build();
            var command = this.Database.CreateCommand(query);
            foreach (var column in table.Columns)
            {
                Assert.IsTrue(command.Parameters.Contains(column.ColumnName.ToLower()));
                Assert.IsTrue(command.Parameters.Contains(column.ColumnName.ToUpper()));
                var parameter = default(IDataParameter);
                Assert.IsTrue(command.Parameters.Contains(column.ColumnName.ToLower(), out parameter));
                Assert.IsTrue(command.Parameters.Contains(column.ColumnName.ToUpper(), out parameter));
            }
        }
    }
}
