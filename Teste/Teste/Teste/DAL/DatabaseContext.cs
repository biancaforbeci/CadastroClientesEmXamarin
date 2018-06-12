using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using AppClientes.Infra;
using AppClientes.Models;

namespace AppClientes.DAL
{
    class DatabaseContext:DbContext
    {
        public static string DatabasePath { get; set; }

        public DbSet<Cliente> Clientes{ get; private set; }

        public DatabaseContext()
        {
        }

        public DatabaseContext(string databasePath)
        {
            DatabasePath = databasePath;
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            if (string.IsNullOrWhiteSpace(DatabasePath))
                throw new InvalidOperationException("The database path should be initialized prior the OnConfiguring() method call");

            optionsBuilder.UseSqlite($"Filename={DatabasePath}");

            var lf = new LoggerFactory();
            lf.AddProvider(new MyLoggerProvider());
            //optionsBuilder.UseLoggerFactory(lf); // Uncomment to log SQL queries
            //optionsBuilder.EnableSensitiveDataLogging(); // Uncomment to log SQL parameters
        }

        //https://stackoverflow.com/questions/45775267/how-to-validate-models-before-savechanges-in-entityframework-core-2
        // Alternative version: https://github.com/aspnet/EntityFrameworkCore/issues/3680#issuecomment-155502539
        public override int SaveChanges()
        {
            var changedEntities = ChangeTracker
                .Entries()
                .Where(_ => _.State == EntityState.Added || _.State == EntityState.Modified);

            /*foreach (var e in changedEntities)
            {
                object entity = e.Entity;
                GraphValidation.ValidateAndThrowIfInvalid(entity);
            }*/

            return base.SaveChanges();
            // FIXME: Don't forget to override SaveChangesAsync
        }

        public void UpdateEntity<T>(T entity, T existingEntity) where T : class
        {
            // https://blog.oneunicorn.com/2012/05/03/the-key-to-addorupdate/
            Entry(existingEntity).CurrentValues.SetValues(entity);
        }

        public double GetDatabaseFileSize()
        {
            if (File.Exists(DatabasePath))
                return new FileInfo(DatabasePath).Length;
            return double.NaN;
        }
    }
}
