using System;
using System.Collections.Generic;
using System.Text;

namespace TicketPresentation
{
    public class Tickets
    {
        public int StudentID { get; set; }
        public int OrderNumber { get; set; }
        public int TimeDate { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public bool Availability { get; set; }
        public string EventName { get; set; }
        public DateTime EventTime { get; set; }
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }
    }
}
