using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UIA.Models;
using UIADatabase;

namespace UIA.Controllers
{
    public class RegisterController : Controller
    {
		UIAEntities database = new UIAEntities();
		//string connection = "data source=DESKTOP-ECNLPFS;initial catalog = UIA; user id = UIA_User; password= 123456 ;MultipleActiveResultSets=True;App=EntityFramework";

		public ActionResult MemberRegistration(UIADatabase.Member member)
		{
			
			if (ModelState.IsValid)
			{
				database.Members.Add(member);
				database.SaveChanges();
				return View("~/Views/Home/Login.cshtml");
				//return RedirectToAction("About","Home");
				/*
				// It is, so now we will save it in the database
				using (var sqlConnection = new SqlConnection(connection))
				{
					// Build your query (an example insert)
					var query = "INSERT INTO Member VALUES (@name,@dob,@username,@password,@email)";

					// Build a command to execute this
					using (var sqlCommand = new SqlCommand(query, sqlConnection))
					{
						// Open your connection
						sqlConnection.Open();

						// Add your parameters
						sqlCommand.Parameters.AddWithValue("@name", member.Name);
						sqlCommand.Parameters.AddWithValue("@dob", member.DOB);
						sqlCommand.Parameters.AddWithValue("@username", member.Username);
						sqlCommand.Parameters.AddWithValue("@password", member.Password);
						sqlCommand.Parameters.AddWithValue("@email", member.Email);

						// Execute your query
						sqlCommand.ExecuteNonQuery();

						StringBuilder user = new StringBuilder();
						user.Append("<b>Amount :</b> " + member.Name + "<br/>");
						user.Append("<b>Rate :</b> " + member.DOB + "<br/>");
						user.Append("<b>Time(year) :</b> " + member.Username + "<br/>");
						return Content(user.ToString()); 
					}
				}*/
			}
			return null;
			/*
			string name = member.Name;
			DateTime dob = member.DOB;
			string username = member.Username;
			StringBuilder user = new StringBuilder();
			user.Append("<b>Amount :</b> " + member.Name + "<br/>");
			user.Append("<b>Rate :</b> " + member.DOB + "<br/>");
			user.Append("<b>Time(year) :</b> " + member.Username + "<br/>");
			return Content(user.ToString());*/
		}
	}
}