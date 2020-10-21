using GenericService.Models;
using GenericService.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GenericService.Repositories
{
    public class ContactDbContext : DbContext
    {
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Email> Emails { get; set; }

        public static string SchemaName = "contact";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionstring = "this would be a connection string to a postgresql database for migration and intialization configuration but I didn't get around to it.";
            optionsBuilder.UseNpgsql(connectionstring,
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", SchemaName));
            optionsBuilder.EnableSensitiveDataLogging();
        }
        public ContactDbContext(DbContextOptions options) : base(options)
        {

        }
        public virtual void DetachAllEntities()
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            foreach (EntityEntry entity in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Unchanged ||
                            e.State == EntityState.Deleted)
                .ToList())
            {
                Entry(entity.Entity).State = EntityState.Detached;
            }
            ChangeTracker.AutoDetectChangesEnabled = true;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureContact(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureContact(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<Contact> configuration = modelBuilder.Entity<Contact>();

            configuration.HasIndex(c => c.FirstName);
            configuration.HasIndex(c => c.LastName);
            configuration.HasIndex(c => c.IsDeleted);

            configuration.HasMany<Email>(c => c.Emails).WithOne().IsRequired(false).HasForeignKey(p => p.ContactId).IsRequired(false).OnDelete(DeleteBehavior.ClientSetNull);

        }

    }
}
