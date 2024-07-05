using API_MVC.Models;
using API_MVC.Models.ViewModel;
using API_MVC.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API_MVC.Controllers
{
    public class TrailController : Controller
    {
        private readonly ITrailParkRepository _trailParkRepository;
        private readonly INationalParkRepo _nationalParkRepo;
        public TrailController(ITrailParkRepository trailParkRepository, INationalParkRepo nationalParkRepo)
        {
            _trailParkRepository = trailParkRepository;
            _nationalParkRepo = nationalParkRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var TrailParkList = await _trailParkRepository.GetAllAsync(SD.TrailAPIPath);
            return Json(new { data = TrailParkList });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status=await _trailParkRepository.DeleteAsync(SD.TrailAPIPath,id);
            if(status)
                return Json(new { success=true,message="Deleted"});
            return Json(new { success = false, message ="Something Wrong"});
        }
        #endregion
        public async Task<IActionResult> Upsert(int? id)
        {
         var NationalPark=await _nationalParkRepo.GetAllAsync(SD.NationalParkAPIPath);
            TrailVM trailVM = new TrailVM()
            {
                Trail=new Trail(),
                NationalParkList=NationalPark.Select(np=>new SelectListItem()
                {
                    Text=np.Name,
                    Value = np.Id.ToString()
                })
            };
            if(id==null) return View(trailVM);
            trailVM.Trail=await _trailParkRepository.GetAsync(SD.TrailAPIPath,
                id.GetValueOrDefault());
            if (trailVM.Trail != null) return NotFound();
            return View(trailVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailVM trailVM)
        { 
            if(ModelState.IsValid)
            {
                if(trailVM.Trail.Id==0)
                    await _trailParkRepository.CreateAsync(SD.TrailAPIPath,trailVM.Trail);
                else
                    await _trailParkRepository.UpdateAsync(SD.TrailAPIPath,trailVM.Trail);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var NationalPark = await _nationalParkRepo.GetAllAsync(SD.NationalParkAPIPath);
                trailVM = new TrailVM()
                {
                    Trail = new Trail(),
                    NationalParkList = NationalPark.Select(np => new SelectListItem()
                    {
                        Text = np.Name,
                        Value = np.Id.ToString()
                    })
                };
                return View(trailVM);   
            }
        }   
    }
}
