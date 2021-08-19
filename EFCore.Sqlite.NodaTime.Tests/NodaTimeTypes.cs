using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class NodaTimeTypes
    {
        public int Id { get; set; }
        public Instant Instant { get; set; }
        public LocalTime LocalTime { get; set; }
        public LocalDate LocalDate { get; set; }
        public LocalDateTime LocalDateTime { get; set; }
        //public ZonedDateTime ZonedDateTime { get; set; }
        public Duration Duration { get; set; }
        //public Offset Offset { get; set; }
    }
}
