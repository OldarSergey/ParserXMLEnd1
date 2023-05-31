using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParserXml.Data;
using ParserXml.Model.EntitiesDbContext;


namespace ParserXml.Service
{
    public class LoggingService : ILoggingsService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public LoggingService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task AddLoggingAsync(Logging newLog)
        {
            _context.Loggings.Add(newLog);
            await _context.SaveChangesAsync();
            
        }

        public async Task<List<Logging>> GetLoggingsByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userId = user.Id;

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Contains("Администратор"))
            {
                return await _context.Loggings
                    .Include(log => log.File)
                    .Include(log => log.User)
                    .Where(log => !log.File.IsDeleted)
                    .ToListAsync();
            }
            else
            {
                return await _context.Loggings
                   .Where(logging => logging.UserId == userId && !logging.File.IsDeleted)
                   .Include(log => log.File)
                   .ToListAsync();
            }
        }
    }

}
