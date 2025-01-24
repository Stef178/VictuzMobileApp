using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace VictuzMobileApp.MVVM.Model
{
    public class Ticket
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Ignore]
        public Activity Activity { get; set; }
        [Ignore]
        public Participant Participant { get; set; }
        [NotNull]
        public decimal Price { get; set; }
    }
}
