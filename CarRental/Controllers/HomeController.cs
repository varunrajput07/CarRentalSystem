using CarRental.Data;
using CarRental.Models;
using CarRental.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
namespace CarRental.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        //private object _emailSender;
        //private string emailFrom;
        public IEmailHelper _emailHelper;
        public object Log { get; private set; }

        public HomeController(ApplicationDbContext db, IEmailHelper emailHelper, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
            _emailHelper = emailHelper;
        }

        public IActionResult Index()
        {
            Car car = new();
            IEnumerable<SelectListItem> CarModalList = _db.CarModals.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            ViewBag.CarModalList = CarModalList;

            return View(car);
        }
 
        private SmtpClient getSMTPClientInstance()
        {
            throw new NotImplementedException();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public async Task<IActionResult> SearchCar(string filter, int? numOfSeats, int? carModalId, DateTime? BookingStartDate, DateTime? BookingEndDate)
        {
            // ViewData["GetCardetails"] = SearchCar;
            //var carquery = from x in _db.Cars select x;
            var booking = _db.CarBookings.AsQueryable();
            if (BookingStartDate.HasValue)
            {

                //BookingStDate < inputenddate && inputstartdate < bookingenddate 
                booking = booking.Where(x => (x.BookingStartDate.Date <= BookingEndDate.Value.Date && BookingStartDate.Value.Date <= x.BookingEndDate.Date));
                
                //booking = booking.Where(x =>(x.BookingStartDate.Date <= BookingStartDate.Value.Date && x.BookingEndDate.Date >= BookingEndDate.Value.Date)
                //|| (x.BookingStartDate.Date < BookingStartDate.Value.Date && x.BookingEndDate.Date < BookingEndDate.Value.Date)
                //|| (x.BookingStartDate.Date < BookingStartDate.Value.Date && x.BookingEndDate.Date <= BookingEndDate.Value.Date)
                //|| (x.BookingStartDate.Date < BookingStartDate.Value.Date && x.BookingEndDate.Date > BookingEndDate.Value.Date));

            }
            else
            {
                booking = booking.Where(x => 1 == 2);
            }
            
            var carquery = from _Cars in _db.Cars.Include(x=>x.CarModal) 
                           //join _CarModal in _db.CarModals on _Cars.CarModalId= _CarModal.Id
                           join _CarBookings in booking on _Cars.Id equals _CarBookings.CarId into cb
                           from CarBookings in cb.DefaultIfEmpty()
                           select new
                           {
                               //Name = _Cars.Name,
                               //NumOfSeats = _Cars.NumOfSeats,
                               //CarModalId = _Cars.CarModalId,
                               Car=_Cars,
                               BookingStartDate =CarBookings!=null?(DateTime?) CarBookings.BookingStartDate:null,
                               //CarModalName=_CarModal.CarModalName,
                               BookingEndDate = CarBookings != null ?(DateTime?)CarBookings.BookingEndDate:null
                           };
            carquery = carquery.Where(x => x.BookingStartDate == null);
           //  from person in people
           // join pet in pets on person equals pet.Owner into gj
           // from subpet in gj.DefaultIfEmpty()
            if (!string.IsNullOrEmpty(filter))

            {
                carquery = carquery.Where(x => x.Car.Name.Contains(filter));
            }
            if (numOfSeats.HasValue)
            {
                carquery = carquery.Where(x => x.Car.NumOfSeats == numOfSeats);
            }
            if (carModalId.HasValue)
            {
                carquery = carquery.Where(x => x.Car.CarModalId == carModalId);
            }
            //if (BookingStartDate.HasValue)
            //{//BookingStDate <=inputstdate and bookingenddate >=input end date
            //    carquery = carquery.Where(x => !(x.BookingStartDate.HasValue) || !(x.BookingStartDate.Value.Date <= BookingStartDate.Value.Date && x.BookingEndDate.Value.Date  >= BookingEndDate.Value.Date));
            //    carquery = carquery.Where(x => !(x.BookingStartDate.HasValue) || !(x.BookingStartDate.Value.Date <= BookingStartDate.Value.Date && x.BookingEndDate.Value.Date <= BookingEndDate.Value.Date));
            //    carquery = carquery.Where(x => !(x.BookingStartDate.HasValue) || !(x.BookingStartDate.Value.Date >= BookingStartDate.Value.Date && x.BookingEndDate.Value.Date <= BookingEndDate.Value.Date));
            //    carquery = carquery.Where(x => !(x.BookingStartDate.HasValue) || !(x.BookingStartDate.Value.Date >= BookingStartDate.Value.Date && x.BookingEndDate.Value.Date >= BookingEndDate.Value.Date));

            //}
            //if (BookingEndDate.HasValue)
            //{
            //    carquery = carquery.Where(x => x.BookingEndDate == BookingEndDate);
            //}

            ViewBag.stdate = BookingStartDate;
            ViewBag.enddate = BookingEndDate;

            return View(await carquery.Select(x=>x.Car).ToListAsync());
        }
        //Get
        public IActionResult CarBooking(int id , DateTime? stdate , DateTime? enddate)
        {
            if (id == 0)
            {
                return NotFound();
            }
            CarBooking carBooking = new();
            carBooking.CarId = id;
            if (stdate.HasValue)
            {
                carBooking.BookingStartDate = stdate.Value;
                carBooking.BookingEndDate = enddate.HasValue ? enddate.Value.Date : stdate.Value;
            }
            else
            {
                carBooking.BookingStartDate = DateTime.Now.Date;
                carBooking.BookingEndDate = DateTime.Now.Date;
            }

            var car = _db.Cars.FirstOrDefault(x => x.Id == carBooking.CarId);
            carBooking.Amount = car.Price;
            var PaymentAmount = (carBooking.BookingEndDate - carBooking.BookingStartDate).Days * car.Price;
            carBooking.PaymentAmount = PaymentAmount;

            return View(carBooking);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CarBooking(CarBooking obj )
        {
            if (ModelState.IsValid)
            {
                var car = _db.Cars.FirstOrDefault(x => x.Id == obj.CarId);
                //int(IEnumerable<Car>) Amount = (IEnumerable<Car>)obj.Price;
                var PaymentAmount = (obj.BookingEndDate - obj.BookingStartDate).Days * car.Price;
                obj.PaymentAmount = PaymentAmount;
                obj.Amount = car.Price;
                obj.BookingStatus = 1;
                _db.CarBookings.Add(obj);
                _db.SaveChanges();
                #region
                string message = "Hello  " + obj.FirstName + " " + @obj.LastName + "<br/> Your Booking request BookingDate:" + @obj.BookingStartDate + "<br/>and BookingEnd date:" + @obj.BookingEndDate + " CarBooking Successfully<br/>You Will  receive booking confirmation email shortly from our team.<br/><br/><br/>Thank You <br/>Car Rental Service.";
                _emailHelper.SendEmail(message, obj.Email);
                #endregion
                TempData["success"] = "CarBooking Successfully!!!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //GET
        public IActionResult Contact()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Contact obj )
        {
            if (ModelState.IsValid)
            {
                
                #region
                string message = "Hello Admin , <br/> You have Message from "+ obj.Name + ".<br/><br/>User Phone number is " + obj.ContactNo+" and Email Address is "+obj.Email+ ".<br/><br/> A user message is " + obj.Message + "<br/><br/><br/>Thank You";
                _emailHelper.SendEmail(message, "varunrajput5747@gmail.com");
                #endregion
                TempData["success"] = "Message Send Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //GET
        public IActionResult About()
        {

            return View();
        }

    }
}

    