using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UIA.Models;
using UIADatabase;

namespace UIA.Controllers
{
	public class HomeController : Controller
	{
		UIAEntities database = new UIAEntities();

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		// GET: Register
		public ActionResult Register()
		{
			return View();
		}

		public ActionResult Login()
		{
			return View();
		}

		public ActionResult Logout()
		{
			Session["MemberInfo"] = null;
			Session["LoginInfo"] = null;

			return View("~/Views/Home/Index.cshtml");
		}

		public ActionResult Search()
		{
			var member = Session["MemberInfo"];
			if (member != null)
			{
				var select_list = from a in database.Airports orderby a.airport_country, a.airport_name select a;
				return View(select_list.ToList());
			}
			else
			{
				return View("~/Views/Home/Login.cshtml");
			}
		}

		public ActionResult ViewBooking()
		{
			return View();
		}

		public ActionResult EditProfile()
		{
			var login = Session["LoginInfo"] as UIADatabase.Login;
			var memberdetail1 = from l in database.Logins
							   join m in database.Members on l.username equals m.login_username
							   where l.username == login.username
							   select m;
			var memberdetail = memberdetail1.FirstOrDefault();
			ViewData["member_detail"] = memberdetail;
			return View();
		}

	}
}