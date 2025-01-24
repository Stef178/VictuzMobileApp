using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using VictuzMobileApp.MVVM.Model;

namespace VictuzMobileApp.MVVM.Model
{
    public class Activity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public string Name { get; set; }
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

    }
}
