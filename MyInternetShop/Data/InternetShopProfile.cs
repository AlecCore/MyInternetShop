using AutoMapper;
using MyInternetShop.Data.Entities;
using MyInternetShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInternetShop.Data
{
    public class InternetShopProfile : Profile
    {
        public InternetShopProfile()
        {
            this.CreateMap<Product, ProductModel>().ReverseMap();
        }
    }
}
