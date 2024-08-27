using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NET_FRAMEWORKS_EXAMEN_OPDRACHT.Areas.Identity.Data;
using NET_FRAMEWORKS_EXAMEN_OPDRACHT.Data;
using NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models;
namespace Garage2.Data



{
    public class GarageContext : IdentityDbContext<GarageUser>
    {

        public GarageContext (DbContextOptions<GarageContext> options)
            : base(options)
        {

            /*"GarageContext": "Server=(localdb)\\mssqllocaldb;Database=GarageKeeper;Trusted_Connection=True;MultipleActiveResult*//*Sets=true",*/
        }
    //    builder.Services.AddDbContext<GarageContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("GarageContext") ?? throw new InvalidOperationException("Connection string 'Garage2Context' not found.")));


        

        public DbSet<NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models.Expense> Expense { get; set; } = default!;

        public DbSet<NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models.Car> Car { get; set; } = default!;

        public DbSet<NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models.Appointment> Appointment { get; set; } = default!;

        public DbSet<NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models.Customer> Customer { get; set; } = default!;

        public DbSet<NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models.Invoice> Invoice { get; set; } = default!;

        public DbSet<NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models.Order> Order { get; set; } = default!;

		public DbSet<NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models.Language> Languages { get; set; } = default!;
		public void InitiateDatabase()
        {
            Database.Migrate();  




        }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Cars)
                .WithOne(car => car.Customer)
                .HasForeignKey(car => car.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Car>()
                .HasMany(car => car.Orders)
                .WithOne(order => order.Car)
                .HasForeignKey(order => order.CarID)
                .OnDelete(DeleteBehavior.NoAction);

           modelBuilder.Entity<Invoice>()
                .HasOne(invoice => invoice.Car)
                .WithMany()
                .HasForeignKey(invoice => invoice.CarID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .HasOne(order => order.Car)
                .WithMany()
                .HasForeignKey(order => order.CarID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Expense>()
                .Ignore(c => c.ExpenseId);

           
            modelBuilder.Entity<Expense>().HasKey(e => e.ExpenseId);

            modelBuilder.Entity<Expense>()
            .Property(c => c.Amount)
            .HasColumnType("decimal(18,2)");



            base.OnModelCreating(modelBuilder);
        }

        
    }
}
