using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
        public Task Equal() => VerifyQuery(x => x == Duration.Epsilon);

        [Fact]
        public Task GreaterThan() => VerifyQuery(x => x > Duration.Zero);

        [Fact]
        public Task LessThan() => VerifyQuery(x => x < Duration.Epsilon.Plus(Duration.Epsilon));

        [Fact]
        public Task Update() => VerifyUpdate(x => x.Duration = x.Duration.Plus(Duration.Epsilon));
    }
}
