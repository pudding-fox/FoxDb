using NUnit.Framework;
using System.Collections.Generic;

namespace FoxDb
{
    [TestFixture(ProviderType.SqlCe)]
    [TestFixture(ProviderType.SqlServer)]
    [TestFixture(ProviderType.SqlServer2012)]
    [TestFixture(ProviderType.SQLite)]
    public class HashCodeTests : TestBase
    {
        public HashCodeTests(ProviderType providerType)
            : base(providerType)
        {

        }

        [Test]
        public void HashCodeProperty_IsPopulated()
        {
            var set = this.Database.Set<Cake>(this.Transaction);
            var data = new List<Cake>();
            set.Clear();
            this.AssertSequence(data, set);
            data.AddRange(new[]
            {
                new Cake() { Field1 = "1_1", Field2 = "1_2", Field3 = "1_3" },
                new Cake() { Field1 = "2_1", Field2 = "2_2", Field3 = "2_3" },
                new Cake() { Field1 = "3_1", Field2 = "3_2", Field3 = "3_3" }
            });
            set.AddOrUpdate(data);
            this.AssertSequence(data, set);
            Assert.AreEqual(-2134694053, data[0].HashCode);
            Assert.AreEqual(-2103996793, data[1].HashCode);
            Assert.AreEqual(-2096134873, data[2].HashCode);
            data[1].Field1 = "updated";
            set.AddOrUpdate(data);
            this.AssertSequence(data, set);
            Assert.AreEqual(-2134694053, data[0].HashCode);
            Assert.AreEqual(-1415461129, data[1].HashCode);
            Assert.AreEqual(-2096134873, data[2].HashCode);
        }

        [Table(Name = "Test001")]
        public class Cake : Test001
        {
            [HashCode]
            public int HashCode { get; set; }
        }
    }
}
