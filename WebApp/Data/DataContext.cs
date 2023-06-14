using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class DataContext : DbContext 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        //public DataContext(DbSet<User> user,
        //                   DbSet<UserDto> userDto,
        //                   DbSet<Inspection> inspections,
        //                   DbSet<InspectionType> inspectionTypes,
        //                   DbSet<Status> statuses,
        //                   DbSet<LogActivities> logActivities,
        //                   DbSet<game> game)
        //{
        //    User = user;
        //    UserDto = userDto;
        //    Inspections = inspections;
        //    InspectionTypes = inspectionTypes;
        //    Statuses = statuses;
        //    LogActivities = logActivities;
        //    this.game = game;
        //}

        public DataContext() : base()
        {
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserDto> UserDto { get; set; }
        public virtual DbSet<Inspection> Inspections { get; set; }
        public virtual DbSet<InspectionType> InspectionTypes { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<LogActivities> LogActivities { get; set; }
        public virtual DbSet<game> game { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("User").HasKey(u => u.Id);
            modelBuilder.Entity<UserDto>().ToTable("UserDto").HasKey(u => u.Id);
        }

        public DbSet<Inspection> GetInspections()
        {
            return Inspections;
        }
    }
}
