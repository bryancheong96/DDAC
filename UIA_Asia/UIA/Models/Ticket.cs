using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UIA.Models
{
	public class Ticket
	{
		public int Ticket_No { get; set; }
		public Booking Booking_Id { get; set; }
		public FlightRecord Flight_Id { get; set; }
		public string Passenger_Lastname { get; set; }
		public string Passenger_Firstname { get; set; }
		public string Passenger_Passport { get; set; }
		public System.DateTime Passenger_dob { get; set; }
		public string Class_Type { get; set; }
		public decimal Fare { get; set; }
		public string Ticket_Status { get; set; }
	}
}