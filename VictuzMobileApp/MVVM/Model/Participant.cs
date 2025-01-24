using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace VictuzMobileApp.MVVM.Model
{
    public class Participant
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public string Name { get; set; }
        [NotNull]
        public string Email { get; set; }
        [NotNull]
        public string Password { get; set; }

		[Ignore]
		public ICollection<ParticipantActivity>? ParticipantActivities { get; set; }
	}
}
