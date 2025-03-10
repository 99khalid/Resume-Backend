using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastrucure.Context
{
    public class ContextSeed
    {
        private static AppDBContext _dbContext;
        private static IServiceProvider _serviceProvider;
        public ContextSeed(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
      
        public void Seed ()
        {
            if (!_dbContext.Users.Any(u => u.UserRole == Role.Admin))
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword("10201020");
                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "admin",
                    PasswordHash = hashedPassword, // استبدلها بكلمة المرور المشفرة الفعلية
                    UserRole = Role.Admin,
                    State = State.NotDeleted,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow
                };

                _dbContext.Users.Add(adminUser);
                _dbContext.SaveChanges();
            }
        }
       

    }
}
