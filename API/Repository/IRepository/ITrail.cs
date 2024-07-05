using API.Models;

namespace API.Repository.IRepository
{
    public interface ITrail
    {
        ICollection<Trail> GetTrailsInNationalPark(int nationalParkId);
        ICollection<Trail> GetTrails();
        Trail GetTrail(int trailId);
        bool TrailExists(int trailId);
        bool TrailExists(string trailName);
        bool CreateTrail(Trail trail);
        bool UpdateTrail(Trail trail);  
        bool DeleteTrail(Trail trail);
        bool Save();
    }
}
