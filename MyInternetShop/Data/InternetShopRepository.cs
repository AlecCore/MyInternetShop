using Microsoft.EntityFrameworkCore;
using MyInternetShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace MyInternetShop.Data
{
    public class InternetShopRepository : IInternetShopRepository
    {
        private readonly InternetShopContext _context;

        public InternetShopRepository(InternetShopContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Product[]> GetAllProductsAsync ()
        {
            IQueryable<Product> query = _context.Products;

            return await query.ToArrayAsync();
        }

        public async Task<Product> GetProductByIdAsync(int Id)
        {
            IQueryable<Product> query = _context.Products;
            query = query.Where(c => c.ProductId == Id);
            return await query.FirstOrDefaultAsync();
        }


    }
}
