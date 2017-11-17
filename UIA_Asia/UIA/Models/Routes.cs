using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UIA.Models
{
	public class Routes
	{
		public int Route_Id { get; set; }
		public Airport Origin_Airport_Id { get; set; }
		public Airport Dest_Airport_Id { get; set; }
	}
}