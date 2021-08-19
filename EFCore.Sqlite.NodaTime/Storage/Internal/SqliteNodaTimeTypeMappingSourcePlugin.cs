using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteNodaTimeTypeMappingSourcePlugin : IRelationalTypeMappingSourcePlugin
    {
        private readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings = new()
        {
            { typeof(LocalDateTime), new SqliteTypeMapping<LocalDateTime>(SqlitePatterns.LocalDateTime) },
            { typeof(LocalDate), new SqliteTypeMapping<LocalDate>(SqlitePatterns.LocalDate) },
            { typeof(LocalTime), new SqliteTypeMapping<LocalTime>(SqlitePatterns.LocalTime) },
            { typeof(Instant), new SqliteTypeMapping<Instant>(SqlitePatterns.Instant) },
            //{ typeof(ZonedDateTime), new SqliteTypeMapping<ZonedDateTime>(SqlitePatterns.ZonedDateTime) },
            { typeof(Duration), new SqliteTypeMapping<Duration>(SqlitePatterns.Duration) },
            //{ typeof(Offset), new SqliteTypeMapping<Offset>(SqlitePatterns.Offset) },
        };

        public RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo)
        {
            if (mappingInfo.ClrType is null)
            {
                return null;
            }

            if (_clrTypeMappings.TryGetValue(mappingInfo.ClrType, out var mapping))
            {
                return mapping;
            }

            return null;
        }
    }
}
