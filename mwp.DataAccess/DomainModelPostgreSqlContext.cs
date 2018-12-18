﻿using mwp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace mwp.DataAccess
{
    public class DomainModelPostgreSqlContext : DbContext
    {
        public DomainModelPostgreSqlContext(DbContextOptions<DomainModelPostgreSqlContext> options) : base(options)
        {
        }

        public DbSet<Record> Record { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseLazyLoadingProxies().UseNpgsql(@"User ID=postgres;Password=masterpw;Host=localhost;Port=5432;Database=mwp-local;Pooling=true;");
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Record>().HasKey(m => m.Id);
            builder.Entity<RecordVisibility>().HasKey(m => m.Id);
            builder.Entity<User>().HasKey(m => m.Id);
            builder.Entity<UserGroup>().HasKey(m => m.Id);
            builder.Entity<UserRole>().HasKey(m => m.Id);

            // set default values
            builder.Entity<User>().Property(u => u.IsDeleted).HasDefaultValue(false);
            builder.Entity<User>().Property(u => u.CreatedOn).HasDefaultValueSql("now()");
            builder.Entity<UserGroup>().Property(u => u.CreatedOn).HasDefaultValueSql("now()");
            builder.Entity<Record>().Property(r => r.CreatedOn).HasDefaultValueSql("now()");
            // shadow properties
            //builder.Entity<Record>().Property<DateTime>("UpdatedCreatedOn");
            //builder.Entity<User>().Property<DateTime>("UpdatedCreatedOn");
            //builder.Entity<UserGroup>().Property<DateTime>("UpdatedCreatedOn");
            //builder.Entity<UserRole>().Property<DateTime>("UpdatedCreatedOn");

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            //UpdateUpdatedProperty<Record>();
            //UpdateUpdatedProperty<User>();
            //UpdateUpdatedProperty<UserGroup>();
            //UpdateUpdatedProperty<UserRole>();

            return base.SaveChanges();
        }

        //private void UpdateUpdatedProperty<T>() where T : class
        //{
        //    var modifiedSourceInfo =
        //        ChangeTracker.Entries<T>()
        //            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        //    foreach (var entry in modifiedSourceInfo)
        //    {
        //        entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
        //    }
        //}
    }
}
