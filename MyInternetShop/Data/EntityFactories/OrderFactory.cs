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
    public class OrderFactory : IOrderFactory
    {
        private readonly IMapper _mapper;
        protected class OrderInformation
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }

            public Client orderClient { get; set; }

            public Product orderProduct { get; set; }
        }

        public OrderFactory(IMapper mapper)
        {
            _mapper = mapper;
        }
        public Order ReturnOrder(object orderInformation)
        {
            var _orderInformation = (OrderInformation)orderInformation;
            return new Order { OrderId=_orderInformation.Id,DeliveryTime=_orderInformation.Date,Client=_orderInformation.orderClient,Product=_orderInformation.orderProduct };
        }

        public async Task<Order> ReturnOrderAsync (int OrderId, DateTime DeliveryTime, Client Client, Product Product)
        {
            var _orderInformation = new OrderInformation { Id = OrderId, Date = DeliveryTime,orderClient=Client,orderProduct=Product };
            return await Task<Order>.Run(() => ReturnOrder(_orderInformation));
        }


    }
}
