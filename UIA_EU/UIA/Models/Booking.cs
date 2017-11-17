using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UIA.Models
{
	public class Booking
	{
		public int Booking_Id { get; set; }
		public DateTime Booking_Datetime { get; set; }
		public decimal Total_Fare { get; set; }
		public string Booking_Status { get; set; }

		public Booking (int Booking_Id,DateTime Booking_Datetime, decimal Total_Fare, string Booking_Status)
		{
			this.Booking_Id = Booking_Id;
			this.Booking_Datetime = Booking_Datetime;
			this.Total_Fare = Total_Fare;
			this.Booking_Status = Booking_Status;
		}
	}
}