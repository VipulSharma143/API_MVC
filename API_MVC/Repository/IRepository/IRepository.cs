using API_MVC.Models;

namespace API_MVC.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(string url, int id);//find ka code
        Task<IEnumerable<T>> GetAllAsync(string url);
        Task<bool> CreateAsync(string url, T ObjToCreate);
        Task<bool> UpdateAsync(string url, T ObjToUpdate);
        Task<bool> DeleteAsync(string url, int id);
        Task<List<Bookings>> GetBookingsForDateAsync(string url, DateTime date);
    }
}
