using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VictuzMobileApp.MVVM.Model
{
    public class Activity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public string Name { get; set; }
        [NotNull]
        public string Category { get; set; }
        [NotNull]
        public string Description { get; set; }
        [NotNull]
        public DateTime StartTime { get; set; }
        [NotNull]
        public DateTime EndTime { get; set; }
        [Ignore]
        public Organisor? Organisor { get; set; }
        [Ignore]
        public ICollection<Participant>? Participants { get; set; }

		[Ignore]
		public ICollection<ParticipantActivity>? ParticipantActivities { get; set; }

	}
}
