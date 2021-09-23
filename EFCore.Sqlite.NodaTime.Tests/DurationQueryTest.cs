using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class DurationQueryTest : QueryTests<Duration>
    {
        public static readonly Duration Value = Duration.Epsilon;

        /// <inheritdoc />
        public DurationQueryTest() : base(x => x.Duration)
        {
        }

        [Fact]
        public void Roundtrip() => Assert.Equal(Value, Query.Single());

        [Fact]
        public Task Equal() => VerifyQuery(x => x == Value);

        [Fact]
        public Task GreaterThan() => VerifyQuery(x => x > Value.Minus(Duration.Epsilon));

        [Fact]
        public Task LessThan() => VerifyQuery(x => x < Value.Plus(Duration.Epsilon));

        [Fact]
        public Task Update() => VerifyUpdate(x => x.Duration = x.Duration.Plus(Duration.Epsilon));
    }
}
