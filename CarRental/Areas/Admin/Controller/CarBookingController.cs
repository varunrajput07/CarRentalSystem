using CarRental.Data;
using CarRental.Models;
using CarRental.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace CarRental.Areas

{
    [Area("Admin")]
    public class CarBookingController : Controller
    {
        private readonly ApplicationDbContext _db;
        private object configuration;
        private object appSettings;

        public IEmailHelper _emailHelper;
        public IConfiguration _configuration;
       

        public IConfiguration MySettingsConfiguration { get; }

        public CarBookingController(ApplicationDbContext db, IEmailHelper emailHelper, IConfiguration configuration) 
        {
            _emailHelper = emailHelper;
            _configuration = configuration; 
            _db = db;
        }
         // [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            IEnumerable<CarBooking> objCarBookingList = _db.CarBookings.Include(x=>x.Car);
            return View(objCarBookingList);
        }
        //GET
        public IActionResult Create(int? Id)
        {
            CarBooking carBooking = new();
            IEnumerable<SelectListItem> CarList = _db.Cars.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            ViewBag.CarList = CarList;
            if (Id == null || Id == 0)
            {
                return View(carBooking);
            }
            else
            {
                //update Car
               

                carBooking = _db.CarBookings.FirstOrDefault(x => x.Id == Id);
                List<SelectListItem> BookingStatus = new()
                {
                    new SelectListItem { Value = "1", Text = "Pending" },
                    new SelectListItem { Value = "2", Text = "Approve" },
                    new SelectListItem { Value = "3", Text = "Reject" }
                };
                ViewBag.BookingStatus = BookingStatus;
                
                return View(carBooking);
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        //     public IActionResult Create(CarBooking obj)
        //{
        //     if (ModelState.IsValid)
        //     {
        //         _db.CarBookings.Add(obj);
        //         _db.SaveChanges();
        //         TempData["success"] = "CarBooking Created Successfully";
        //         return RedirectToAction("Index");
        //     }
        //     return View(obj);
        // }

        public IActionResult Create(CarBooking obj)
        {
            if (obj.Id == 0)
            {
                _db.CarBookings.Add(obj);
                _db.SaveChanges();
                
                TempData["success"] = "CarBooking created successfully";
            }
            else
            {
                var carBooking = _db.CarBookings.AsNoTracking().FirstOrDefault(x => x.Id == obj.Id);
                var carBookingOldStatus = carBooking.BookingStatus;
                carBooking = obj;
                
                _db.CarBookings.Update(carBooking);
                _db.SaveChanges();

                #region Approval/Rejection Email
                string ApproveEmailBody = "Hello " + obj.FirstName + "" + @obj.LastName + "<span>.<span/><br/> Your Booking request BookingDate:" + @obj.BookingStartDate + "<br/>and BookingEnd date:" + @obj.BookingEndDate + "<span>.<span/><br/>You Select Car Rent Price is " +obj.PaymentAmount+ "<span>.<span/><br/>your Booking Request is Approved.<br/><br/>Thank You <br/>Car Rental Service.";
                string RejectEmailBody = "Hello " + obj.FirstName + "" + @obj.LastName + "<span>.<span/><br/> Your Booking request BookingDate:" + @obj.BookingStartDate + "<br/>and BookingEnd date:" + @obj.BookingEndDate + "<span>.<span/><br/>You Select Car Rent Price is " + obj.PaymentAmount + "<span>.<span/><br/>your Booking Request is Rejected.<br/><br/>Thank You <br/>Car Rental Service.";

                if (carBookingOldStatus == 1 && obj.BookingStatus == 2)
                {
                    _emailHelper.SendEmail(ApproveEmailBody, obj.Email);
                }
                else if (carBookingOldStatus == 1 && obj.BookingStatus == 3)
                {
                    _emailHelper.SendEmail(RejectEmailBody, obj.Email);
                }
                else if (carBookingOldStatus == 2 && obj.BookingStatus == 3)
                {
                    _emailHelper.SendEmail(RejectEmailBody, obj.Email);
                }
                else if (carBookingOldStatus == 3 && obj.BookingStatus == 2)
                {
                    _emailHelper.SendEmail(ApproveEmailBody, obj.Email);
                }
                #endregion

                TempData["success"] = "CarBooking updated successfully";
            }
            //string message = "Hello  " + obj.FirstName + "" + @obj.LastName + "<br/> Your Booking request BookingDate:" + @obj.BookingStartDate + "<br/>and BookingEnd date:" + @obj.BookingEndDate + " Approve<br/>You Will  receive booking confirmation email shortly from our team.<br/><br/><br/>Thank You <br/>Car Rental Service.";
            //Email(message, obj.Email);
            // string bookingReqEmail = "bookingReqEmail.Replace("{FirstName}",obj.FirstName){@obj.LastName}<br/>Your Booking request for { @obj.Name}";
            //  Email(bookingReqEmail, obj.Email);

            return RedirectToAction("Index");

           // return View(obj);
        }
        //public void Email(string htmlString , string receiverEmail)
        //{
        //    string fromAddress = _configuration.GetSection("Smtp").GetSection("FromAddress").Value;
        //    string Server = _configuration.GetSection("Smtp").GetSection("Server").Value;
        //    string Port = _configuration.GetSection("Smtp").GetSection("Port").Value;
        //    string password = _configuration.GetSection("Smtp").GetSection("Password").Value;
        //    try
        //    {
        //        MailMessage message = new MailMessage();
        //        SmtpClient smtp = new SmtpClient();
        //        message.From = new MailAddress(fromAddress);
        //        message.To.Add(new MailAddress(receiverEmail));
        //        message.Subject = "CarRental";
        //        message.IsBodyHtml =  true; //to make message body as html  
        //        message.Body = htmlString;
        //        smtp.Port = Convert.ToInt32(Port);
        //        smtp.Host = Server;//"smtp.gmail.com"; //for gmail host  
        //        smtp.EnableSsl = true;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new NetworkCredential(fromAddress, password);
        //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtp.Send(message);
        //    }
        //    catch (Exception ) {  }
        //}
        


        //[HttpGet]
        //public IActionResult GetUsers()
        //{
        //    var dbvalue = appSettings.value.Dbconnection;

        //    var list = new List<string>();
        //    list.Add("John");
        //    list.Add("Doe");
        //    return Ok(list);

        //}

    }
}
//        [HttpGet]

//        public async Task<IActionResult> Index(string CarSearch)
//        {
//            ViewData["GetCardetails"]= CarSearch;

//            var carquery = from x in _db.Cars select x;
//            if(!string.IsNullOrEmpty(CarSearch))
//            {
//                carquery = carquery.Where(x => x.NumOfSeats.Contains(CarSearch) || x.Name.Contains(CarSearch));
//            }
//            return View(await carquery.AsNoTracking().ToListAsync());
//        }

//        //GET
//        public IActionResult Upsert(int? Id)
//        {
//            Car car = new();
//            IEnumerable<SelectListItem> CarModalList = _db.CarModals.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
//            ViewBag.CarModalList = CarModalList;
//            if (Id == null || Id == 0)
//            {
//                //create Car

//                return View(car);
//            }
//            else
//            {
//                //update Car
//                car = _db.Cars.FirstOrDefault(x => x.Id == Id);
//                return View(car);
//            }

//        }

//        //POST
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Upsert(CarBooking obj)
//        {

//            if (obj.Id == 0)
//            {
//                _db.CarBookings.Add(obj);

//            }
//            else
//            {
//                _db.CarBookings.Update(obj);

//            }
//            _db.SaveChanges();
//            TempData["success"] = "Car created successfully";
//            return RedirectToAction("Index");

//        return View(obj);
//    }
//        //GET
//        public IActionResult Delete(int? Id)
//        {
//            if (Id == null || Id == 0)
//            {
//                return NotFound();
//            }
//            var CarFromDb = _db.CarBookings.Find(Id);
//            // var CarModalFromDbFirst = _db.CarModals.FirstOrDefault(x => x.Id == Id);
//            // var CarModalFromDbSingle = _db.CarModals.SingleOrDefault(x => x.Id == Id);

//            if (CarFromDb == null)
//            {
//                return NotFound();
//            }
//            return View(CarFromDb);
//        }

//        //POST
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult DeletePost(int? Id)
//        {
//            var obj = _db.CarBookings.Find(Id);
//            if (obj == null )
//            {
//                return NotFound();
//            }
//                _db.CarBookings.Remove(obj);
//                _db.SaveChanges();
//                TempData["success"] = "Car Deleted Successfully";
//                return RedirectToAction("Index");

//        }
//    }
//}
