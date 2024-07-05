using API_MVC.Models;
using API_MVC.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace API_MVC.Controllers
{
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepo _nationalparkRepo;
        public NationalParkController(INationalParkRepo nationalparkRepo)
        {
            _nationalparkRepo = nationalparkRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var nationalParkList = await _nationalparkRepo.GetAllAsync(SD.NationalParkAPIPath);
            return Json(new { data = nationalParkList });
        }

        public async Task<IActionResult> Upsert(int?  id)
        {
            NationalPark nationalPark = new NationalPark();
            if (id == null) return View(nationalPark);
            nationalPark = await _nationalparkRepo.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault());
            if (nationalPark == null)  return NotFound();
            return View(nationalPark);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _nationalparkRepo.DeleteAsync(SD.NationalParkAPIPath, id);
            if (status)
                return Json(new { success = true, message = "Data Successfully Deleted" });
            return Json(new { success = false, message = "Something went wrong" });
        }

        #endregion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark nationalPark)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    nationalPark.Picture = p1;
                }
                else
                {
                    var nationalParkInDb = await _nationalparkRepo.GetAsync(SD.NationalParkAPIPath, nationalPark.Id);
                }
                if (nationalPark.Id == 0)
                    await _nationalparkRepo.CreateAsync(SD.NationalParkAPIPath, nationalPark);
                else
                    await _nationalparkRepo.UpdateAsync(SD.NationalParkAPIPath, nationalPark);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(nationalPark);
            }
        }

    }
}
