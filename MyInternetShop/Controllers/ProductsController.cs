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
    public class ProductsController : ControllerBase
    {
        private readonly IInternetShopRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly InternetShopContext _context;
        private readonly ILogger<ProductsController> _logger;



        public ProductsController(IInternetShopRepository repository, IMapper mapper, LinkGenerator linkGenerator,InternetShopContext context,
            ILogger<ProductsController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _context = context;
            _logger = logger;

        }

        [HttpGet]
        public async Task<ActionResult<ProductModel[]>> GetAllProducts()
        {
            try
            {
                _logger.LogInformation($"GetAllProducts");
                var results = await _repository.GetAllProductsAsync();

                return _mapper.Map<ProductModel[]>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ProductModel>> GetProductById(int Id)
        {
            try
            {
                _logger.LogInformation($"GetProductById");
                var result = await _repository.GetProductByIdAsync(Id);
                if (result == null) NotFound();
                return _mapper.Map<ProductModel>(result);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Data base failure");
            }

        }

        [HttpPost]
        public async Task<ActionResult<ProductModel>> CreateProduct (ProductModel model)
        {
            _logger.LogInformation($"CreateProduct");
            var existing = await _repository.GetProductByIdAsync(model.ProductId);
            if ( existing != null)
            {
                return BadRequest("Product already exists");
            }
            try
            {
                var location = _linkGenerator.GetPathByAction("GetProductById",
                       "products",
                     new { Id = model.ProductId });
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current Id");
                }
                var product = _mapper.Map<Product>(model);
                _repository.Add(product);

                if (await _repository.SaveChangesAsync())
                {
                    
                    return Created(location, _mapper.Map<ProductModel>(product));
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult<ProductModel>> PutProduct(ProductModel model)
        {
            try
            {
                _logger.LogInformation($"PutProduct");
                var oldProduct = await _repository.GetProductByIdAsync(model.ProductId);
                if (oldProduct == null) return NotFound($"Could not find Product");
                _mapper.Map(model, oldProduct);
                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<ProductModel>(oldProduct);
                }
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
            return BadRequest("Failed to put the product");
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeletProduct(int Id)
        {
            try
            {
                _logger.LogInformation($"DeleteProduct");
                var oldProduct = await _repository.GetProductByIdAsync(Id);
                if (oldProduct == null) return NotFound($"Could not find Product");
                _repository.Delete(oldProduct);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
            return BadRequest("Failed to delete the product");
        }

        [HttpPatch("{Id}")]
        public async Task<ActionResult<ProductModel>> PatchProduct(int Id,[FromBody] JsonPatchDocument<ProductModel> patchEntity)
        {
            try
            {
                _logger.LogInformation($"PatchProduct");
                var oldProduct = await _repository.GetProductByIdAsync(Id);

                if (oldProduct == null) return NotFound($"Could not find Product");
                //Возвращаем сущность что меняем Моделькой
                var newProductModel = _mapper.Map<ProductModel>(oldProduct);
                //Патчим модельку
                patchEntity.ApplyTo(newProductModel);
                //Мапим модельку к сущности
                _mapper.Map(newProductModel,oldProduct);

                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<ProductModel>(oldProduct);
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
            return BadRequest("Failed to patch the product");
        }

    }

  
}
