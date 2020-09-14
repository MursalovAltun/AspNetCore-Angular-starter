using System;
using System.Collections.Generic;
using System.Linq;
using Fido2NetLib;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ProductsController : ControllerBase
    {
        private readonly List<Product> _products = new List<Product>();

        // We can set return type directly
        [HttpGet]
        public Product Get(Guid id)
        {
            return _products.SingleOrDefault(product => product.Id == id);
        }

        // Or set generic IActionResult with ProducesResponseType to let swagger to know response type and nswag to generate code
        [HttpGet]
        [SwaggerResponse(typeof(IEnumerable<Product>))]
        public IActionResult GetAll()
        {
            return Ok(_products);
        }

        [HttpPost]
        [SwaggerResponse(typeof(Product))]
        public Product Add(Product product)
        {
            _products.Add(product);

            return _products.Last();
        }

        [HttpPut]
        public Product Update(Product product)
        {
            var updateIndex = _products.FindIndex(p => p.Id == product.Id);

            _products[updateIndex] = product;

            return _products[updateIndex];
        }
    }

    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}