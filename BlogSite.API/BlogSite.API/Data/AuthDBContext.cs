using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.API.Data
{
    public class AuthDBContext : IdentityDbContext
    {
        public AuthDBContext(DbContextOptions<AuthDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "a5ae11f3-510e-4c7d-b5dc-9993659d3e1a";
            var writerRoleId = "2c737d52-207f-4346-9f0a-6b5ff4488626";

            // create reader and writer roles

            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id= writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            // seed roles data
            builder.Entity<IdentityRole>().HasData(roles);

            // create admin user
            var adminUserId = "0b8c87f4-67a0-434f-b524-30ffa070e6ff";

            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@blogsite.com",
                Email = "admin@blogsite.com",
                NormalizedEmail = "admin@blogsite.com".ToUpper(),
                NormalizedUserName = "admin@blogsite.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@2822");

            builder.Entity<IdentityUser>().HasData(admin);

            // Give roles to admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId= adminUserId,
                    RoleId= writerRoleId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
