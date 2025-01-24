using SQLite;

namespace VictuzMobileApp.MVVM.Model
{
	public class ParticipantActivity
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[NotNull]
		public int ParticipantId { get; set; }

		[NotNull]
		public int ActivityId { get; set; }

		public DateTime RegistrationDate { get; set; }

		[Ignore]
		public Participant Participant { get; set; }

		[Ignore]
		public Activity Activity { get; set; }
	}
}
