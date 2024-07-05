using API.Models;

namespace API.Repository.IRepository
{
    public interface IUserRepo
    {
        bool IsUniqueUser(string username);
        User Authenticate(string username, string password);
        User Register(string username, string password);
    }
}
