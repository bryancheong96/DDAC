using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UIA.Models;
using UIADatabase;

namespace UIA.Controllers
{
    public class FlightController : Controller
    {
		UIAEntities database = new UIAEntities();
		DateTime arrival_date1 = Convert.ToDateTime("12/31/2017");
		DateTime arrival_date = Convert.ToDateTime("12/31/2017");
		// GET: Flight
		public ActionResult SearchFlight()
        {
			//declare variable & conversion
			string string_origin = Request["origin"];
			int origin = Convert.ToInt32(string_origin);

			string string_destination = Request["destination"];
			int destination = Convert.ToInt32(string_destination);

			string round = Request["round"];
			if (!(string.IsNullOrEmpty(round)))
			{
				string string_arrival_date = Request["arrival_date"];
				arrival_date1 = Convert.ToDateTime(string_arrival_date);
				arrival_date = new DateTime(arrival_date1.Year, arrival_date1.Month, arrival_date1.Day, 23, 59, 59);

			}
			string string_origin_date = Request["destination_date"];
			DateTime origin_date1 = Convert.ToDateTime(string_origin_date);
			DateTime origin_date = new DateTime(origin_date1.Year, origin_date1.Month, origin_date1.Day, 23, 59, 59);

			string number = Request["number"];
			string child_number = Request["child_number"];
			string class_type = Request["class"];

			//validation
			if (origin == destination)
			{
				ViewBag.Message = "Origin and Destination cannot be the same.";
				return View("~/Views/Home/Index.cshtml");
			}
			else
			{
				if (!(string.IsNullOrEmpty(round)))
				{
					if (arrival_date1 < origin_date1)
					{
						ViewBag.Message = "Arrival date cannot earlier than departure date";
						return View("~/Views/Home/Index.cshtml");
					}
					else
					{
						//query
						var depart = from o in database.Routes
									 join f in database.Flights on o.route_id equals f.route_id
									 where o.origin_airport_id == origin & o.dest_airport_id == destination
									 orderby o.origin_airport_id ascending
									 select f;

						var arrival = from o in database.Routes
									  join f in database.Flights on o.route_id equals f.route_id
									  where o.origin_airport_id == destination & o.dest_airport_id == origin
									  orderby o.origin_airport_id ascending
									  select f;
						TempData["origin_date1"] = origin_date1;
						TempData["origin_date"] = origin_date;
						ViewData["arrival_date1"] = arrival_date1;
						ViewData["arrival_date"] = arrival_date;
						ViewData["arrival_query"] = arrival.ToList();
						var number1 = Convert.ToInt32(number);
						TempData["passenger"] = number1;
						TempData["type"] = class_type;
						TempData["child"] = child_number;
						return View(depart.ToList());
					}
				}
				else
				{
					var depart = from o in database.Routes
								 join f in database.Flights on o.route_id equals f.route_id
								 where o.origin_airport_id == origin & o.dest_airport_id == destination
								 orderby o.origin_airport_id ascending
								 select f;
					TempData["origin_date1"] = origin_date1;
					TempData["origin_date"] = origin_date;
					var number1 = Convert.ToInt32(number);
					TempData["passenger"] = number1;
					TempData["type"] = class_type;
					TempData["child"] = child_number;
					
					/*
					StringBuilder user = new StringBuilder();
					var check = TempData["passenger"];

					var fare1 = from f in database.FareTariffs
								where f.flight_id == 53302 & f.class_type == class_type
								select f;
					var fare = fare1.FirstOrDefault();

					user.Append("<b>Rate :</b> " + fare.adult_fare + "<br/>");
					return Content(user.ToString());
					*/
					return View(depart.ToList());
				}
			}
        }

		// GET: SearchFlight
		public ActionResult SelectedFlight()
		{
			string child = Request["child"];
			string type = Request["type"];
			string flight1 = Request["flight1"];
			string flight2 = Request["flight2"];

			TempData["child"] = child;
			TempData["type"] = type;
			TempData["flight1"] = flight1;
			TempData["flight2"] = flight2;
			if (!(flight2.Equals("0")))
			{
				TempData["flight2"] = flight2;
			}

			return View("~/Views/Flight/Passenger.cshtml");
		}

		public ActionResult PassengerDetail(List<Passenger> detail)
		{
			int check1 = Convert.ToInt32(Request["count"]);
			int check2 = Convert.ToInt32(Request["child_count"]);
			int flight1 = Convert.ToInt32(TempData["flight1"]);
			int flight2 = Convert.ToInt32(TempData["flight2"]);
			string type = Request["type"].ToString();

			var departquery = from f in database.FlightRecords
							  join ft in database.FareTariffs on f.flight_id equals ft.flight_id
							  where f.flight_id == flight1
						      select f;
			var depart = departquery.FirstOrDefault();
			ViewData["depart_flight1"] = depart;
			ViewData["count"] = check1;
			ViewData["child_count"] = check2;
			ViewData["return_flight1"] = null;
			ViewData["type"] = type;

			var farequery = from f in database.FareTariffs
							where f.flight_id == flight1 && f.class_type == type
							select f;
			var farequery2 = farequery.FirstOrDefault();
			decimal fare1 = (check1 * Math.Round(Convert.ToDecimal(farequery2.adult_fare),2)) + (check2 * Math.Round(Convert.ToDecimal(farequery2.child_fare),2));
			decimal fare = Math.Round(fare1, 2);
			ViewData["fare"] = fare;

			if (flight2 != 0)
			{
				var returnquery = from f in database.FlightRecords
								  where f.flight_id == flight2
								  select f;
				var return_flight = returnquery.FirstOrDefault();
				ViewData["return_flight1"] = return_flight;

				var farequery3 = from f in database.FareTariffs
								where f.flight_id == flight2 && f.class_type == type
								select f;
				var farequery4 = farequery3.FirstOrDefault();
				decimal fare2 = (check1 * Math.Round(Convert.ToDecimal(farequery4.adult_fare), 2)) + (check2 * Math.Round(Convert.ToDecimal(farequery4.child_fare), 2));
				decimal flight2_fare = Math.Round(fare2, 2);
				ViewData["fare2"] = flight2_fare;
			}

			//StringBuilder user = new StringBuilder();
			for (int i=1; i<=check1; i++)
			{
				string last_name = Request["last_name_"+i];
				string first_name = Request["first_name_"+i];
				string passport = Request["passport_"+i];
				DateTime dob = Convert.ToDateTime(Request["dob_"+i]);

				Passenger passenger = new Passenger(last_name,first_name,passport,dob);
				TempData["detail"+i] = passenger;
			}
			for (int i = 1; i <= check2; i++)
			{
				string last_name = Request["child_last_name_" + i];
				string first_name = Request["child_first_name_" + i];
				string passport = Request["child_passport_" + i];
				DateTime dob = Convert.ToDateTime(Request["child_dob_" + i]);

				Passenger passenger = new Passenger(last_name, first_name, passport, dob);
				TempData["child_detail" + i] = passenger;
			}
			return View("~/Views/Flight/Confirmation.cshtml");
		}
		
		public ActionResult Confirmation()
		{
			//get fare
			decimal fare = Convert.ToDecimal(Request["fare"]);
			decimal fare2 = Convert.ToDecimal(Request["fare2"]);
			decimal total = fare + fare2;
			//get flight if and checking whether has flight2/return flight
			int flight1 = Convert.ToInt32(Request["flight1_id"]);
			int flight2 = Convert.ToInt32(Request["flight2_id"]);
			//get member id from session
			var member = (UIADatabase.Member)Session["MemberInfo"];
			//get count of passenger
			int count = Convert.ToInt32(Request["count"]);
			int child_count = Convert.ToInt32(Request["child_count"]);
			//get class type
			string type = Request["type"].ToString();
			//global access
			UIADatabase.Booking booking2 = null;
			UIADatabase.Booking booking1 = new UIADatabase.Booking(member.id, DateTime.Now, fare, "Booked");
			database.Bookings.Add(booking1);
			database.SaveChanges();

			if (flight2 != 0)
			{
				booking2 = new UIADatabase.Booking(member.id, DateTime.Now, fare2, "Booked");
				database.Bookings.Add(booking2);
				database.SaveChanges();
			}

			//StringBuilder user = new StringBuilder();

			for (int i = 1; i <= count; i++)
			{
				string last_name = Request["last_name_" + i];
				string first_name = Request["first_name_" + i];
				string passport = Request["passport_" + i];
				DateTime dob = Convert.ToDateTime(Request["dob_" + i]);
				UIADatabase.Ticket ticket = new UIADatabase.Ticket(booking1.booking_id, flight1, last_name, first_name, passport, dob, type, "Booked");
				database.Tickets.Add(ticket);
				database.SaveChanges();
					
					//user.Append("<b>passenger :</b> " + ticket.passenger_lastname + "<br/>");
					//user.Append("<b>passenger_dob :</b> " + ticket.class_type + "<br/>");
					//user.Append("<b>passenger_dob :</b> " + type + "<br/>");
				/*
				catch (DbEntityValidationException dbEx)
				{
					foreach (var validationErrors in dbEx.EntityValidationErrors)
					{
						foreach (var validationError in validationErrors.ValidationErrors)
						{
							Trace.TraceInformation("Property: {0} Error: {1}",
													validationError.PropertyName,
													validationError.ErrorMessage);
							user.Append("<b>passenger :</b> " + validationError.PropertyName + "<br/>");
							user.Append("<b>passenger_dob :</b> " + validationError.ErrorMessage + "<br/>");
						}
					}
				} */
				//save passenger
				//TempData["detail" + i] = passenger;
			}
			for (int i = 1; i <= child_count; i++)
			{
				string last_name = Request["child_last_name_" + i];
				string first_name = Request["child_first_name_" + i];
				string passport = Request["child_passport_" + i];
				DateTime dob = Convert.ToDateTime(Request["child_dob_" + i]);

				Passenger passenger = new Passenger(last_name, first_name, passport, dob);
				
				UIADatabase.Ticket ticket = new UIADatabase.Ticket(booking1.booking_id, flight1, last_name, first_name, passport, dob, type, "Booked");
				database.Tickets.Add(ticket);
				database.SaveChanges();
				
				//user.Append("<b>child :</b> " + ticket.passenger_lastname + "<br/>");
				//save passenger
				//TempData["child_detail" + i] = passenger;
			}

			if (flight2 != 0)
			{
				for (int i = 1; i <= count; i++)
				{
					string last_name = Request["last_name_" + i];
					string first_name = Request["first_name_" + i];
					string passport = Request["passport_" + i];
					DateTime dob = Convert.ToDateTime(Request["dob_" + i]);

					Passenger passenger = new Passenger(last_name, first_name, passport, dob);
					UIADatabase.Ticket ticket = new UIADatabase.Ticket(booking2.booking_id, flight2, last_name, first_name, passport, dob, type, "Booked");
					database.Tickets.Add(ticket);
					database.SaveChanges();
					//user.Append("<b>passenger2 :</b> " + ticket.passenger_lastname + "<br/>");
					//save passenger
					//TempData["detail" + i] = passenger;
				}
				for (int i = 1; i <= child_count; i++)
				{
					string last_name = Request["child_last_name_" + i];
					string first_name = Request["child_first_name_" + i];
					string passport = Request["child_passport_" + i];
					DateTime dob = Convert.ToDateTime(Request["child_dob_" + i]);

					Passenger passenger = new Passenger(last_name, first_name, passport, dob);
					UIADatabase.Ticket ticket = new UIADatabase.Ticket(booking2.booking_id, flight2, last_name, first_name, passport, dob, type, "Booked");
					database.Tickets.Add(ticket);
					database.SaveChanges();
					//user.Append("<b>child2 :</b> " + ticket.passenger_lastname + "<br/>");
					//save passenger
					//TempData["child_detail" + i] = passenger;
				}
			}

			/*
			user.Append("<b>fare :</b> " + fare + "<br/>");
			user.Append("<b>fare2 :</b> " + fare2 + "<br/>");
			user.Append("<b>total :</b> " + total + "<br/>");
			user.Append("<b>flight1 :</b> " + flight1 + "<br/>");
			user.Append("<b>flight2 :</b> " + flight2 + "<br/>");
			user.Append("<b>id :</b> " + member.id + "<br/>");
			user.Append("<b>count :</b> " + count + "<br/>");
			user.Append("<b>child_count :</b> " + child_count + "<br/>");
			user.Append("<b>type :</b> " + type + "<br/>");
			

			return Content(user.ToString());*/
			ViewBag.Message = "Flight booked successfully.";
			return View("~/Views/Home/Index.cshtml");
		}
	}
}