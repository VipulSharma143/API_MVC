using API.Data;
using API.Models;
using API.Repository.IRepository;

namespace API.Repository
{
    public class NationalParkRepo : INationalPark
    {
        private readonly ApplicationDbContext _context;
        public NationalParkRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _context.NationalPark.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _context.NationalPark.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            return  _context.NationalPark.Find(nationalParkId);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _context.NationalPark.ToList();
        }

        public bool NationalParkExists(int nationalParkId)
        {
            return _context.NationalPark.Any(np=>np.Id == nationalParkId);  
        }

        public bool NationalParkExists(string nationalParkName)
        {
            return _context.NationalPark.Any(np =>np.Name == nationalParkName);
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _context.NationalPark.Update(nationalPark);
            return Save();
        }
    }
}
