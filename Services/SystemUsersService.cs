using MoreMusic.DataLayer;
using MoreMusic.DataLayer.Entity;

namespace MoreMusic.Services
{
    public interface ISystemUserService
    {
        SystemUsers GetUserByCredentials(string username, string password);
    }
    public class SystemUsersService : ISystemUserService
    {
        private readonly MusicDbContext _context;

        public SystemUsersService(MusicDbContext context)
        {
            _context = context;
        }

        public SystemUsers GetUserByCredentials(string username, string password)
        {
            return _context.systemUsers.SingleOrDefault(u => u.UserName == username && u.Password == password);
        }
    }
}
