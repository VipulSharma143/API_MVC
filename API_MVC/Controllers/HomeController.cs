using API_MVC.Models;
using API_MVC.Models.ViewModel;
using API_MVC.Repository;
using API_MVC.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace API_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepo _nationalParkRepo;
        private readonly ITrailParkRepository _trailParkRepository;

        public HomeController(ILogger<HomeController> logger, 
            INationalParkRepo nationalParkRepo, ITrailParkRepository trailParkRepository)
        {
            _logger = logger;
            _nationalParkRepo= nationalParkRepo;
            _trailParkRepository= trailParkRepository;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel indexViewModel = new IndexViewModel()
            {
                NationalParkList = await _nationalParkRepo.GetAllAsync(SD.NationalParkAPIPath),
                TrailList=await _trailParkRepository.GetAllAsync(SD.TrailAPIPath)
            };
            return View(indexViewModel);
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
    }
}
