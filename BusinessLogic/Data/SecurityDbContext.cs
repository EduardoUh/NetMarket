using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection.Emit;

namespace BusinessLogic.Data
{
    public class SecurityDbContext(DbContextOptions<SecurityDbContext> options) : IdentityDbContext<User>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /*builder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);*/
        }
    }
}
