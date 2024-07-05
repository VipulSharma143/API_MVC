using API_MVC.Models;
using API_MVC.Repository.IRepository;

namespace API_MVC.Repository
{
    public class TrailRepo:Repository<Trail>,ITrailParkRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TrailRepo(IHttpClientFactory httpClientFactory):base(httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
