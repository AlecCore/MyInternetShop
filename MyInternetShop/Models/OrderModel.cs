using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyInternetShop.Data.Entities;
namespace MyInternetShop.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }

        public DateTime DeliveryTime { get; set; }

        public ClientModel Client { get; set; }

        public ProductModel Product { get; set; }
    }
}
