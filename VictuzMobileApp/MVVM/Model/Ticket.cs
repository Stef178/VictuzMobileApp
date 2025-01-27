using SQLite;

namespace VictuzMobileApp.MVVM.Model
{
    public class Ticket
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int ParticipantId { get; set; }

        [NotNull]
        public int ActivityId { get; set; }

        [NotNull]
        public decimal Price { get; set; }

        public bool IsPaid { get; set; }

        [Ignore]
        public Activity Activity { get; set; }

        [Ignore]
        public Participant Participant { get; set; }
    }
}
