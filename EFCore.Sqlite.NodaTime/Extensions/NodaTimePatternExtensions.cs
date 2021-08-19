using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Extensions
{
    internal static class NodaTimePatternExtensions
    {
        public static ValueConverter<T, string> AsValueConverter<T>(this IPattern<T> pattern)
            => new Converter<T>(pattern);

        public static ValueComparer<T> AsValueComparer<T>(this IPattern<T> pattern)
            => new Comparer<T>(pattern);

        private class Converter<T> : ValueConverter<T, string>
        {
            public Converter(IPattern<T> pattern)
                : base(x => pattern.Format(x), x => pattern.Parse(x).GetValueOrThrow())
            {
            }
        }

        private class Comparer<T> : ValueComparer<T>
        {
            private static readonly IPattern<T> _sqlitePattern;

            static Comparer()
            {
                var patternFieldInfo = typeof(SqlitePatterns)
                    .GetFields(BindingFlags.Static | BindingFlags.NonPublic)
                    .Single(fieldInfo => fieldInfo.FieldType == typeof(IPattern<T>));

                if (patternFieldInfo.GetValue(null) is not IPattern<T> pattern)
                {
                    throw new Exception($"No {nameof(SqlitePatterns)} defined for type {typeof(T).Name}");
                }

                _sqlitePattern = pattern;
            }

            public Comparer(IPattern<T> pattern)
                : base(
                    (left, right) => DoEquals(left, right),
                    (input) => DoHashCode(input),
                    (input) => DoSnapshot(input)!)
            {
                if (!_sqlitePattern.Equals(pattern))
                {
                    throw new Exception(
                        $"Invalid {typeof(IPattern<T>).Name} configured " +
                        $"for {typeof(ValueComparer<T>).Name} configured");
                }
            }

            private static bool DoEquals(T left, T right)
            {
                var leftFormatted = _sqlitePattern.Format(left);
                var rightFormatted = _sqlitePattern.Format(right);

                return leftFormatted.Equals(rightFormatted, StringComparison.Ordinal);
            }

            private static int DoHashCode(T input) =>
                input != null ? input.GetHashCode() : 0;

            private static T DoSnapshot(T input)
            {
                var inputFormatted = _sqlitePattern.Format(input);

                var parseResult = _sqlitePattern.Parse(inputFormatted);

                return parseResult.GetValueOrThrow();
            }
        }
    }
}
