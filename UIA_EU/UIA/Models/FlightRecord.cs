using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UIA.Models
{
	public class FlightRecord
	{
		public int Flight_Id { get; set; }
		public Flight Flight_No { get; set; }
		public DateTime Departure_Datetime { get; set; }
		public DateTime Arrival_Datetime { get; set; }
	}
}