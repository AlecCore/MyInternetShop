using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyInternetShop.Data.Entities;

namespace MyInternetShop.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public Double Price { get; set; }

        public string Name { get; set; }

    }
}
