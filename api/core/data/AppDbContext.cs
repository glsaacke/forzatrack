using Microsoft.EntityFrameworkCore;
using api.core.models;

namespace api.core.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Build> Builds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.HasKey(u => u.UserId);
                e.Property(u => u.UserId).HasColumnName("user_id");
                e.Property(u => u.Username).HasColumnName("username");
                e.Property(u => u.Email).HasColumnName("email");
                e.Property(u => u.Password).HasColumnName("password");
                e.Property(u => u.Deleted).HasColumnName("deleted");
                e.HasQueryFilter(u => u.Deleted == 0);
            });

            modelBuilder.Entity<Car>(e =>
            {
                e.ToTable("Cars");
                e.HasKey(c => c.CarId);
                e.Property(c => c.CarId).HasColumnName("car_id");
                e.Property(c => c.Make).HasColumnName("make");
                e.Property(c => c.Model).HasColumnName("model");
                e.Property(c => c.Year).HasColumnName("year");
                e.Property(c => c.Deleted).HasColumnName("deleted");
                e.HasQueryFilter(c => c.Deleted == 0);
            });

            modelBuilder.Entity<Record>(e =>
            {
                e.ToTable("Records");
                e.HasKey(r => r.RecordId);
                e.Property(r => r.RecordId).HasColumnName("record_id");
                e.Property(r => r.UserId).HasColumnName("user_id");
                e.Property(r => r.CarId).HasColumnName("car_id");
                e.Property(r => r.Event).HasColumnName("event");
                e.Property(r => r.ClassRank).HasColumnName("class_rank");
                e.Property(r => r.TimeMin).HasColumnName("time_min");
                e.Property(r => r.TimeSec).HasColumnName("time_sec");
                e.Property(r => r.TimeMs).HasColumnName("time_ms");
                e.Property(r => r.CpuDiff).HasColumnName("cpu_diff");
                e.Property(r => r.AddDate).HasColumnName("date");
                e.Property(r => r.Deleted).HasColumnName("deleted");
                e.HasQueryFilter(r => r.Deleted == 0);
            });

            modelBuilder.Entity<Build>(e =>
            {
                e.ToTable("Builds");
                e.HasKey(b => b.BuildId);
                e.Property(b => b.BuildId).HasColumnName("build_id");
                e.Property(b => b.UserId).HasColumnName("user_id");
                e.Property(b => b.CarId).HasColumnName("car_id");
                e.Property(b => b.Rank).HasColumnName("rank");
                e.Property(b => b.SpeedST).HasColumnName("speed_st");
                e.Property(b => b.HandlingST).HasColumnName("handling_st");
                e.Property(b => b.AccelerationST).HasColumnName("acceleration_st");
                e.Property(b => b.LaunchST).HasColumnName("launch_st");
                e.Property(b => b.BrakingST).HasColumnName("braking_st");
                e.Property(b => b.OffroadST).HasColumnName("offroad_st");
                e.Property(b => b.TopSpeed).HasColumnName("top_speed");
                e.Property(b => b.ZeroToSixty).HasColumnName("zero_to_sixty");
                e.Property(b => b.Deleted).HasColumnName("deleted");
                e.HasQueryFilter(b => b.Deleted == 0);
            });
        }
    }
}
