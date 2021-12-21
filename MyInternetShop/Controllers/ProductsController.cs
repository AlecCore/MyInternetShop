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

        public ProductsController(IInternetShopRepository repository, IMapper mapper, LinkGenerator linkGenerator,InternetShopContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _context = context;

        }

        [HttpGet]
        public async Task<ActionResult<ProductModel[]>> GetAllProducts()
        {
            try
            {
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
        public async Task<ActionResult<ProductModel>> Post (ProductModel model)
        {
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

    }

  
}
