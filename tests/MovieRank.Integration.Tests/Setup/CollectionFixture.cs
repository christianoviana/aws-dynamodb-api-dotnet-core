using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieRank.Integration.Tests.Setup
{
    [CollectionDefinition("api")]
    public class CollectionFixture:ICollectionFixture<TestContext>
    {
    }
}
