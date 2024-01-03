using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Areas
    
{
[Area("Admin")]
public class CarModalController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CarModalController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            IEnumerable<CarModal> objCarModalList = _db.CarModals;
            return View(objCarModalList);
        }
        //GET
        public IActionResult Create()
        {
         
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CarModal obj)
        {
            if (ModelState.IsValid)
            {
                _db.CarModals.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "CarModal Created Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //GET
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var CarModalFromDb = _db.CarModals.Find(Id);
            // var CarModalFromDbFirst = _db.CarModals.FirstOrDefault(x => x.Id == Id);
            // var CarModalFromDbSingle = _db.CarModals.SingleOrDefault(x => x.Id == Id);

            if (CarModalFromDb == null)
            {
                return NotFound();
            }
            return View(CarModalFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CarModal obj)
        {
            if (ModelState.IsValid)
            {
                _db.CarModals.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "CarModal Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //GET
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
           // var CarModalFromDb = _db.CarModals.Find(Id);
            // var CarModalFromDbFirst = _db.CarModals.FirstOrDefault(x => x.Id == Id);
            // var CarModalFromDbSingle = _db.CarModals.SingleOrDefault(x => x.Id == Id);

            var CarModal = _db.CarModals.FirstOrDefault(x => x.Id == Id);
            var Name = CarModal.Name;
            ViewBag.Car = Name;
            if (CarModal == null)
            {
                return NotFound();
            }
            return View(CarModal);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? Id)
        {
            var obj = _db.CarModals.Find(Id);
            if (obj == null )
            {
                return NotFound();
            }
                _db.CarModals.Remove(obj);
                _db.SaveChanges();
                TempData["success"] = "CarModal Deleted Successfully";
                return RedirectToAction("Index");
            
        }
    }
}
