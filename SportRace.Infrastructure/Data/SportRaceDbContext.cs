using Microsoft.EntityFrameworkCore;
using SportRace.Domain.Entities;

namespace SportRace.Infrastructure.Data
{
    public class SportRaceDbContext : DbContext
    {
        public SportRaceDbContext(DbContextOptions<SportRaceDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AgeGroup> AgeGroups { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<StartList> StartLists { get; set; }
        public DbSet<StartListEntry> StartListEntries { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Participant -> Registration
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Participant)
                .WithMany(p => p.Registrations)
                .HasForeignKey(r => r.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Event -> Registration
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Registrations)
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            // Discipline -> Registration
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Discipline)
                .WithMany()
                .HasForeignKey(r => r.DisciplineId)
                .OnDelete(DeleteBehavior.Restrict);

            // Category -> Registration
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Category)
                .WithMany()
                .HasForeignKey(r => r.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // AgeGroup -> Registration
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.AgeGroup)
                .WithMany()
                .HasForeignKey(r => r.AgeGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // Registration -> Result
            modelBuilder.Entity<Result>()
                .HasOne(r => r.Registration)
                .WithMany()
                .HasForeignKey(r => r.RegistrationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Event -> StartList
            modelBuilder.Entity<StartList>()
                .HasOne<Event>()
                .WithMany(e => e.StartLists)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            // StartList -> StartListEntry
            modelBuilder.Entity<StartListEntry>()
                .HasOne(e => e.StartList)
                .WithMany(s => s.Entries)
                .HasForeignKey(e => e.StartListId)
                .OnDelete(DeleteBehavior.Restrict);

            // Registration -> StartListEntry
            modelBuilder.Entity<StartListEntry>()
                .HasOne(e => e.Registration)
                .WithMany()
                .HasForeignKey(e => e.RegistrationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}