using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MyInternetShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MyInternetShop.Data
{
    public class InternetShopContext: DbContext
    {
        private readonly IConfiguration _config;

        public InternetShopContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("InternetShop"));
        }

        protected override void OnModelCreating(ModelBuilder bldr)
        {
            bldr.Entity<Product>()
              .HasData(new
              {
                  ProductId = 1,
                  Price=(Double)100,
                  Name="Galaxy S10"                
              });
            bldr.Entity<Client>()
              .HasData(new
              {
                  ClientId = 1,
                  FullName="Kopcha Oleksii",
              });
            bldr.Entity<Order>()
              .HasData(new
              {
                  OrderId = 1,
                  DeliveryTime = DateTime.Now,
                  ClientId=1,
                  ProductId=1
              });
        }

     }

   

}

