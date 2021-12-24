using Microsoft.EntityFrameworkCore;
using MyInternetShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MyInternetShop.Data
{
    public class InternetShopRepository : IInternetShopRepository
    {
        private readonly InternetShopContext _context;
        private readonly ILogger<InternetShopRepository> _logger;

        public InternetShopRepository(InternetShopContext context, ILogger<InternetShopRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Add to Database");
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Delete from Database");
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"SaveChanges");
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Product[]> GetAllProductsAsync()
        {
            _logger.LogInformation($"Getting all products from Database");
            IQueryable<Product> query = _context.Products;

            return await query.ToArrayAsync();
        }

        public async Task<Product> GetProductByIdAsync(int Id)
        {
            _logger.LogInformation($"Get product by id from Database");
            IQueryable<Product> query = _context.Products;
            query = query.Where(c => c.ProductId == Id);
            return await query.FirstOrDefaultAsync();
        }
        public async Task<Client> GetClientByIdAsync(int Id)
        {
            _logger.LogInformation($"Get Client by id from Database");
            IQueryable<Client> query = _context.Clients;
            query = query.Where(c => c.ClientId == Id);
            return await query.FirstOrDefaultAsync();
        }
        public async Task<Order[]> GetAllOrdersAsync(bool includeClients = false, bool includeProducts = false)
        {
            _logger.LogInformation($"Getting all Orders");


            IQueryable<Order> query = _context.Orders;//.Include(order => order.Client);
            if (includeClients)
            {
               query= query.Include(order => order.Client);
            }

            if (includeProducts)
            {
                query = query.Include(order => order.Product);
            }

            return  await query.ToArrayAsync();
        }
        public async Task<Order> GetOrderByIdAsync(int Id, bool includeClients = false, bool includeProducts = false)
        {
            _logger.LogInformation($"Get product by id from Database");
            IQueryable<Order> query = _context.Orders;
            if (includeClients)
            {
                query = query.Include(order => order.Client);
            }

            if (includeProducts)
            {
                query = query.Include(order => order.Product);
            }

            query = query.Where(c => c.OrderId == Id);
            return await query.FirstOrDefaultAsync();
        }
    }
}
