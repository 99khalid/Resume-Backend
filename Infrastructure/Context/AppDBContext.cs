using Domain.Enums;
using Domain.Models;

namespace Infrastrucure.Context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        #region Users 
        public virtual DbSet<User> Users{ get; set; }       
        #endregion

        #region Resume 
        public virtual DbSet<Resume> Resumes { get; set; }
        public virtual DbSet<Education> Educations { get; set; }
        public virtual DbSet<Experience> Experiences { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }       

        #endregion




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Resume>()
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Resume>()
            .HasOne(r => r.User)
            .WithMany(u => u.Resumes)
            .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Education>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Education>()
                .HasOne(e => e.Resume)
                .WithMany(r => r.EducationHistory)
                .HasForeignKey(e => e.ResumeId);

            modelBuilder.Entity<Experience>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Experience>()
                .HasOne(e => e.Resume)
                .WithMany(r => r.WorkExperience)
                .HasForeignKey(e => e.ResumeId);

            modelBuilder.Entity<Skill>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Skill>()
                .HasOne(s => s.Resume)
                .WithMany(r => r.Skills)
                .HasForeignKey(s => s.ResumeId);

            modelBuilder.Entity<Attachment>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd();         

            modelBuilder.Entity<User>()
                .Property(u => u.UserRole)
                .HasConversion(
                    v => v.ToString(),
                    v => (Role)Enum.Parse(typeof(Role), v));

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        public bool AllMigrationsApplied()
        {
            var applied = this.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = this.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
        public void Migrate()
        {
            this.Database.Migrate();
        }
    }
}
