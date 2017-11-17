using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UIADatabase;

namespace UIA.Controllers
{
    public class MemberController : Controller
    {
		UIAEntities database = new UIAEntities();
		ImageService imageservice = new ImageService();
		//string connection = "data source=DESKTOP-ECNLPFS;initial catalog = UIA; user id = UIA_User; password= 123456 ;MultipleActiveResultSets=True;App=EntityFramework";

		public async Task<ActionResult> MemberRegistration(HttpPostedFileBase photo)
		{
			var imageUrl = await imageservice.UploadImageAsync(photo);

			string name = Request["name"];
			DateTime dob = Convert.ToDateTime(Request["dob"]);
			string email = Request["email"];
			string username = Request["username"];
			string password = Request["password"];

			Login login = new Login(username, password);
			Member member = new Member(name, dob, username, email,imageUrl.ToString());

			if (ModelState.IsValid)
			{
				database.Logins.Add(login);
				database.Members.Add(member);
				database.SaveChanges();
				ViewBag.Message = "Register Successfully";
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
		}

		public ActionResult MemberLogin(Login login)
		{
			Login check = database.Logins.Find(login.username);
			if (check != null)
			{
				if (check.password.Equals(login.password))
				{
					var memberquery = from m in database.Members where m.login_username == login.username select m;
					var memberinfo = memberquery.FirstOrDefault();
					Session["MemberInfo"] = memberinfo;
					Session["LoginInfo"] = login;
					return View("~/Views/Home/Index.cshtml");
				}
				else
				{
					ViewBag.Message = "Incorrect username/password.";
					return View("~/Views/Home/Login.cshtml");
				}
			}
			else
			{
				ViewBag.Message = "Please enter username and password!";
				return View("~/Views/Home/Login.cshtml");
			}
		}

		public async Task<ActionResult> UpdateProfile(HttpPostedFileBase photo)
		{
			string name = Request["name"];
			DateTime dob = Convert.ToDateTime(Request["dob"]);
			string email = Request["email"];
			string username = Request["username"];
			string password = Request["password"];
			var login_info = Session["LoginInfo"] as UIADatabase.Login;
			var member_info = Session["MemberInfo"] as UIADatabase.Member;
			int count = 0;
			string url = Request["url"];

			var imageUrl = await imageservice.UploadImageAsync(photo);

			Login login = new Login(username,password);
			//Member member = new Member(name, dob, username, email, imageUrl.ToString());

			var check_login = from l in database.Logins
							  where l.username == username
							  select l;

			foreach(var item in check_login)
			{
				if(item.username == login_info.username)
				{
					count++;
				}
			}

			if(count <= 1)
			{
				var find_member = database.Members.Find(member_info.id);

				if(find_member != null)
				{
					find_member.name = name;
					find_member.DOB = dob;
					find_member.email = email;
					find_member.image_url = imageUrl.ToString();

					var find_login = database.Logins.Find(login_info.username);
					if (find_login != null)
					{
						if (username.Equals(login_info.username) & !(password.Equals(login_info.password)))
						{
							find_login.password = password;
							database.Entry(find_login).State = EntityState.Modified;
							Session["LoginInfo"] = find_login;
						}
						else if (!(username.Equals(login_info.username)) & !(password.Equals(login_info.password)))
						{
							var check_login_ava = database.Logins.Find(login_info.username);
							if (check_login_ava == null)
							{
								database.Logins.Add(login);
								database.SaveChanges();

								find_member.login_username = username;
								database.Entry(find_login).State = EntityState.Deleted;
								Session["LoginInfo"] = login;
								//database.Entry(login).State = EntityState.Modified;
							}
							else
							{
								ViewBag.Message = "Update Unuccessfully! Username has used, please choose another username.";
								return View("~/Views/Home/Index.cshtml");
							}
						}
						else if (!(username.Equals(login_info.username)))
						{
							var check_login_ava = database.Logins.Find(login_info.username);
							if (check_login_ava == null)
							{
								database.Logins.Add(login);
								database.SaveChanges();
								find_member.login_username = username;
								database.Entry(find_login).State = EntityState.Deleted;
								Session["LoginInfo"] = login;
							}
							else
							{
								ViewBag.Message = "Update Unuccessfully! Username has used, please choose another username.";
								return View("~/Views/Home/Index.cshtml");
							}
						}
					}
				}

				database.Entry(find_member).State = EntityState.Modified;
				database.SaveChanges();

				Session["MemberInfo"] = find_member;

				ViewBag.Message = "Update Successfully";
				return View("~/Views/Home/Index.cshtml");
			}
			else
			{
				ViewBag.Message = "Update Unuccessfully! Username has used, please choose another username.";
				return View("~/Views/Home/Index.cshtml");
			}
		}

		// GET: Member
		public ActionResult Index()
        {
            return View();
        }

        // GET: Member/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Member/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Member/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Member/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Member/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Member/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
