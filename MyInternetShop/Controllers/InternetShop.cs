using AutoMapper;
using MyInternetShop.Data;
using MyInternetShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyInternetShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.JsonPatch;
using MyInternetShop.Data.EntityFactories;

namespace MyInternetShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternetShop : ControllerBase
    {
        private readonly IInternetShopRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly InternetShopContext _context;
        private readonly ILogger<InternetShop> _logger;
        private readonly IClientFactory _clientFactory;
        private readonly IOrderFactory _orderFactory;

        public InternetShop(IInternetShopRepository repository, IMapper mapper, LinkGenerator linkGenerator, InternetShopContext context,
            ILogger<InternetShop> logger,IClientFactory clientFactory,IOrderFactory orderFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _context = context;
            _logger = logger;
            _clientFactory = clientFactory;
            _orderFactory = orderFactory;

        }


        [HttpPost("new")]

        public async Task<ActionResult<OrderModel>> CreateOrderNewClient(NewOrderAndClient model)
        {
            _logger.LogInformation($"Create new Client And Order");
            try
            {
         
            var existingClient = await _repository.GetClientByIdAsync(model.ClientId);
            if (existingClient != null)
            {
                return BadRequest("Client already exists");
            }
            var existingProduct = await _repository.GetProductByIdAsync(model.ProductId);
            if (existingProduct == null)
            {
                return BadRequest("Product doesnt  exists");
            }
            var existingOrder = await _repository.GetOrderByIdAsync(model.OrderId);
            if (existingOrder != null)
            {
                return BadRequest("Order already  exists");
            }
             var location = _linkGenerator.GetPathByAction("GetOrderById","orders",new { Id = model.OrderId });

                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current Id");
                }

                Client newClient = await _clientFactory.ReturnClientAsync(model.ClientId, model.FullName);
                _repository.Add(newClient);

             if( await _repository.SaveChangesAsync())
             {
                    Order newOrder = await _orderFactory.ReturnOrderAsync(model.OrderId, model.DeliveryTime,
                        await _repository.GetClientByIdAsync(model.ClientId), await _repository.GetProductByIdAsync(model.ProductId));
                    
                    _repository.Add(newOrder);
                    if (await _repository.SaveChangesAsync())
                    {
                        return Created(location, _mapper.Map<OrderModel>(newOrder));
                    }
             }

        }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
            return BadRequest();

        }
    }
}
