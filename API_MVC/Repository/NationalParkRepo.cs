using API_MVC.Models;
using API_MVC.Repository.IRepository;

namespace API_MVC.Repository
{
    public class NationalParkRepo:Repository<NationalPark>,INationalParkRepo
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public NationalParkRepo(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
