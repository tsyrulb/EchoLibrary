using Repository;
using System.Text.Json.Nodes;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Services {
    public class UserService
    {
        MariaDbContext _context;
        public UserService(MariaDbContext cd)
        {
            _context = cd;
        }

        public async Task<bool> userValidation(JsonObject data)
        {
            if (data == null
                || !data.ContainsKey("username")
                || !data.ContainsKey("password"))
                return false;
            string username = data["username"].ToString();
            string password = data["password"].ToString();
            User u = new User();
            User user = await _context.UserDB.FirstOrDefaultAsync(m => m.Username == username);
            if (user == null)
                return false;
            if (password != user.Password)
                return false;
            return true;
        }
        public async Task<IEnumerable<User>> Data()
        {
            return await _context.UserDB.ToListAsync();
        }
        public async Task<int> addUser(JsonObject user)
        {
            User newUser = new User()
            {
                Username = user["username"].ToString(),
                Nickname = user["name"].ToString(),
                Password = user["password"].ToString(),
                contacts = new List<Contact>()
            };
            _context.Add(newUser);
            return await _context.SaveChangesAsync();
        }
    }
}
