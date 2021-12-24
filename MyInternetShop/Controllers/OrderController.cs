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

namespace MyInternetShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IInternetShopRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly InternetShopContext _context;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IInternetShopRepository repository, IMapper mapper, LinkGenerator linkGenerator, InternetShopContext context,
            ILogger<OrdersController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _context = context;
            _logger = logger;

        }


        [HttpGet]
        public async Task<ActionResult<OrderModel[]>> GetAllOrders(bool includeClients = false, bool includeProducts = false)
        {
            try
            {
                _logger.LogInformation($"GetAllOrders");
                var results = await _repository.GetAllOrdersAsync(includeClients, includeProducts);

                return _mapper.Map<OrderModel[]>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<OrderModel>> GetOrderById(int Id, bool includeClients = false, bool includeProducts = false)
        {
            try
            {
                _logger.LogInformation($"GetOrderById");
                var results = await _repository.GetOrderByIdAsync(Id,includeClients, includeProducts);

                return _mapper.Map<OrderModel>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
        }
        [HttpPut]
        public async Task<ActionResult<OrderModel>> PutOrder(OrderModel model)
        {
            try
            {
                _logger.LogInformation($"PutOrder");
                var oldOrder = await _repository.GetOrderByIdAsync(model.OrderId,true,true);
                if (oldOrder == null) return NotFound($"Could not find Order");
                _mapper.Map(model, oldOrder);

                if (model.Client != null)
                {
                    var client = await _repository.GetClientByIdAsync(model.Client.ClientId);
                    if (client != null)
                    {
                        oldOrder.Client = client;
                    }
                }
                if (model.Product != null)
                {
                    var product = await _repository.GetProductByIdAsync(model.Product.ProductId);
                    if (product != null)
                    {
                        oldOrder.Product = product;
                    }
                }
                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<OrderModel>(oldOrder);
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
            return BadRequest("Failed to put the order");
        }
        [HttpPost]
        public async Task<ActionResult<OrderModel>> CreateOrder(OrderModel model)
        {
            _logger.LogInformation($"Create Order");
            var existing = await _repository.GetOrderByIdAsync(model.OrderId);
            if (existing != null)
            {
                return BadRequest("Order already exists");
            }
            try
            {
                var location = _linkGenerator.GetPathByAction("GetOrderById",
                       "orders",
                     new { Id = model.OrderId });
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current Id");
                }
                var order = _mapper.Map<Order>(model);
                order.Client = await _repository.GetClientByIdAsync(model.Client.ClientId);
                order.Product= await _repository.GetProductByIdAsync(model.Product.ProductId);
                _repository.Add(order);
                if (await _repository.SaveChangesAsync())
                {

                    return Created(location, _mapper.Map<OrderModel>(order));
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
            return BadRequest();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeletOrder(int Id)
        {
            try
            {
                _logger.LogInformation($"DeleteOrder");
                var oldOrder = await _repository.GetOrderByIdAsync(Id);
                if (oldOrder == null) return NotFound($"Could not find Product");
                _repository.Delete(oldOrder);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
            return BadRequest("Failed to delete the Order");
        }

    }
 


}

