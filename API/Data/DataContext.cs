using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static API.Entities.MediaModels;

namespace API.Data
{
  public class DataContext : IdentityDbContext<AppUser, AppRole, int,
          IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, 
          IdentityRoleClaim<int>, IdentityUserToken<int>>
  {
    public DataContext(DbContextOptions options) : base(options)
    {
    }

        // public DbSet<AppUser> Users { get; set; }
           public DbSet<AppDocumentation> AppDocumentations { get; set; }
           public DbSet<Video> Videos { get; set; }
           public DbSet<Files> Files { get; set; }
           public DbSet<Picture> Pictures { get; set; }
           public DbSet<AppIp> Ips { get; set; }
           public DbSet<AppSap> Saps { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

            builder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

            builder.Entity<Video>()
            .HasOne(v => v.AppDocumentation)
            .WithMany(ad => ad.Videos)
            .HasForeignKey(v => v.AppDocumentationId);

            builder.Entity<Files>()
            .HasOne(f => f.AppDocumentation)
            .WithMany(ad => ad.Files)
            .HasForeignKey(f => f.AppDocumentationId);

            builder.Entity<Picture>()
            .HasOne(p => p.AppDocumentation)
            .WithMany(ad => ad.Pictures)
            .HasForeignKey(p => p.AppDocumentationId);

        }
        
    }
}