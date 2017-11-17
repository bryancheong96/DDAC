using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace UIA.Models
{
	[DataContract]
	public class Member
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime DOB { get; set; }
		public Login Login { get; set; }
		public string Email { get; set; }
	}
}