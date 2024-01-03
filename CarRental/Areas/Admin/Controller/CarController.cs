using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Areas
    
{
    [Area("Admin")]
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        private object fileStreams;
   

        public CarController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        
        {
             _db = db;
            _hostEnvironment = hostEnvironment;
        }
         [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            IEnumerable<Car> objCarList = _db.Cars.Include(x=>x.CarModal);
            return View(objCarList);
        }
        
        [HttpPost]

        public async Task<IActionResult> Index(string filter, int? numOfSeats, int? carModalId, DateTime startDate, DateTime? endDate)
        {
            //ViewData["GetCardetails"]= CarSearch;

            var carquery = from x in _db.Cars select x;
            if (!string.IsNullOrEmpty(filter))
            {
                carquery = carquery.Where(x => x.Name.Contains(filter));
            }
            if (numOfSeats.HasValue)
            {
                carquery = carquery.Where(x => x.NumOfSeats == numOfSeats);
            }
            if (carModalId.HasValue)
            {
                carquery = carquery.Where(x => x.CarModalId == carModalId);
            }
            return View(await carquery.AsNoTracking().ToListAsync());
        }


        //GET
        public IActionResult Upsert(int? Id)
        {
            Car car = new();
            IEnumerable<SelectListItem> CarModalList = _db.CarModals.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            ViewBag.CarModalList = CarModalList;
            List<SelectListItem> FuelType = new()
            {
                new SelectListItem { Value = "1", Text = "Petrol" },
                new SelectListItem { Value = "2", Text = "Diesel" },
                new SelectListItem { Value = "3", Text = "CNG" },
                new SelectListItem { Value = "4", Text = "Battery" }
            };
            ViewBag.FuelType = FuelType;
            if (Id == null || Id == 0)
            {
                //create Car

                return View(car);
            }
            else
            {
                //update Car
                car = _db.Cars.FirstOrDefault(x => x.Id == Id);
                
                return View(car);
            }

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Car car , IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                   
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\Cars");
                    var extension = Path.GetExtension(file.FileName).ToString();

                    if(car.CarImage!= null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, car.CarImage.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))

                    {
                        file.CopyTo(fileStream);
                    }
                    car.CarImage = @"\images\Cars\" + filename + extension; 

                }
            }
            if (car.Id == 0)
            {
                _db.Cars.Add(car);
                _db.SaveChanges();
                TempData["success"] = "Car created successfully";
            }
            else
            {
                _db.Cars.Update(car);
                _db.SaveChanges();
                TempData["success"] = "Car updated successfully";
            }
            //_db.SaveChanges();
            //TempData["success"] = "Car created successfully";
            return RedirectToAction("Index");
    }
        //GET
        public IActionResult Delete(int? Id)
        {
           
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var Car= _db.Cars.FirstOrDefault(x => x.Id == Id);
            var Name=Car.Name;
            ViewBag.Car = Name;

            if (Car== null)
            {
                return NotFound();
            }
            return View(Car);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? Id)
        {
            var obj = _db.Cars.Find(Id);
            if (obj == null )
            {
                return NotFound();
            }
            
            _db.Cars.Remove(obj);
                _db.SaveChanges();
                TempData["success"] = "Car Deleted Successfully";
                return RedirectToAction("Index");
            
        }
    }
}
