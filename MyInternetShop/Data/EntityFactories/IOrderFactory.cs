using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MyInternetShop.Data.Entities;
using MyInternetShop.Models;
namespace MyInternetShop.Data.EntityFactories
{
    public interface IOrderFactory
    {

        public Order ReturnOrder(object orderInformation);
        public Task<Order> ReturnOrderAsync(int OrderId, DateTime DeliveryTime,Client Client,Product Product );
    }
}
