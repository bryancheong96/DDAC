using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UIA.Models
{
	public class Passenger
	{
		public string Last_Name { get; set; }
		public string First_Name { get; set; }
		public string Passport { get; set; }
		public DateTime DOB { get; set; }

		public Passenger(string Last_Name, string First_Name, string Passport, DateTime DOB)
		{
			this.Last_Name = Last_Name;
			this.First_Name = First_Name;
			this.Passport = Passport;
			this.DOB = DOB;
		}
	}
}