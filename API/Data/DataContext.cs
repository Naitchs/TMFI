using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static API.Entities.ExcelModels;
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
        public DbSet<ExcelData> ExcelData { get; set; }
        public DbSet<ExcelFile> ExcelFile { get; set; }

        public DbSet<AppCourse> Courses { get; set; }
        public DbSet<AppSubject> Subjects { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<CourseSubject> CourseSubjects { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }
        public DbSet<AppLogs> Logs { get; set; }


        //    public DbSet<AppCourse> Courses { get; set; }
        //    public DbSet<AppEnrollment> Enrollments { get; set; }
        //    public DbSet<Attendance> Attendances { get; set; }
        //    public DbSet<Grade> Grades { get; set; }

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

            builder.Entity<ExcelFile>()
        .HasOne(ef => ef.ExcelData)
        .WithMany(ed => ed.Files)
        .HasForeignKey(ef => ef.ExcelDataId);


            // Configure Course-Subject many-to-many relationship
            builder.Entity<CourseSubject>()
                .HasKey(cs => new { cs.CourseID, cs.SubjectID });

            builder.Entity<CourseSubject>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.CourseSubjects)
                .HasForeignKey(cs => cs.CourseID);

            builder.Entity<CourseSubject>()
                .HasOne(cs => cs.Subject)
                .WithMany(s => s.CourseSubjects)
                .HasForeignKey(cs => cs.SubjectID);

            // Configure Course-Student many-to-many relationship
            builder.Entity<CourseStudent>()
                .HasKey(cs => new { cs.CourseID, cs.StudentID });

            builder.Entity<CourseStudent>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(cs => cs.CourseID);

            builder.Entity<CourseStudent>()
                .HasOne(cs => cs.Student)
                .WithMany(s => s.CourseStudents)
                .HasForeignKey(cs => cs.StudentID);

            // Configure Attendance-Student and Attendance-Subject relationships
            builder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.StudentID);

            builder.Entity<Attendance>()
                .HasOne(a => a.Subject)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.SubjectID);



        }

    }
}