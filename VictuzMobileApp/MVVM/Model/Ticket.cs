using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VictuzMobileApp.MVVM.Model
{
    internal class Ticket
    {
        public int Id { get; set; }
        public Activity Activity { get; set; }
        public Participant Participant { get; set; }
        public decimal Price { get; set; }
    }
}
