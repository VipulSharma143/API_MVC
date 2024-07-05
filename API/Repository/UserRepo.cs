using API.Data;
using API.Models;
using API.Repository.IRepository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace API.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;

        public UserRepo(ApplicationDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value; // Retrieve the actual AppSettings object
        }

        public User Authenticate(string username, string password)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (userInDb == null)
                return null;

            // JWT Token creation
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userInDb.Id.ToString()),
                    new Claim(ClaimTypes.Role, userInDb.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            userInDb.Token = tokenHandler.WriteToken(token);

            // Clear sensitive data
            userInDb.Password = "";

            return userInDb;
        }

        public bool IsUniqueUser(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public User Register(string username, string password)
        {
            User user = new User()
            {
                Username = username,
                Password = password,
                Role = "Admin"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }
    }
}
