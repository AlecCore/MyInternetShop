using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace MyInternetShop.Data
{
    public class InternetShopContextFactory : IDesignTimeDbContextFactory<InternetShopContext>
    {
        public InternetShopContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();

            return new InternetShopContext(new DbContextOptionsBuilder<InternetShopContext>().Options, config);
        }
    }
}
