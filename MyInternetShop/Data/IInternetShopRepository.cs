using MyInternetShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInternetShop.Data
{
    public interface IInternetShopRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        Task<Product[]> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(int Id);

    }
}
