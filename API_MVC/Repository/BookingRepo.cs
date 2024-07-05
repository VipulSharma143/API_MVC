using API_MVC.Models;
using API_MVC.Repository.IRepository;

namespace API_MVC.Repository
{
    public class BookingRepo:Repository<Bookings>,IBookingRepo
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BookingRepo(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
