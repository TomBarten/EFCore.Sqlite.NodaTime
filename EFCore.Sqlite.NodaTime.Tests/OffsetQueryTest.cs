using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class OffsetQueryTest : QueryTests<Offset>
    {
        public static readonly Offset Value = Offset.Zero;

        /// <inheritdoc />
        public OffsetQueryTest() : base(x => x.Offset)
        {
        }

        [Fact]
        public void Roundtrip() => Assert.Equal(Value, Query.Single());

        [Fact]
        public Task Equal() => VerifyQuery(x => x == Value);

        [Fact]
        public Task GreaterThan() => VerifyQuery(x => x > Value.Minus(Offset.MinValue));

        [Fact]
        public Task LessThan() => VerifyQuery(x => x < Value.Plus(Offset.MaxValue));

        [Fact]
        public Task Update() => VerifyUpdate(x => x.Offset = x.Offset.Plus(Offset.MaxValue));
    }
}
