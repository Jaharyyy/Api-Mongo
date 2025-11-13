using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebAPI.DATAS;
using WebAPI.MODEL;

namespace WebAPI.DATAS
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository _repository;

        public ProductsController(ProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<List<Product>> Get() => _repository.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Product> Get(Guid id)
        {
            var product = _repository.GetById(id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Product product)
        {
            _repository.Insert(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public ActionResult Put(Guid id, [FromBody] Product product)
        {
            _repository.Update(id, product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            _repository.Delete(id);
            return NoContent();
        }

        [HttpPost("InsertSampleProducts")]
        public ActionResult InsertSampleProducts()
        {
            var sampleProducts = new List<Product>
            {
                new Product { Name = "Rímel", Price = 120, Category = "Maquillaje" },
                new Product { Name = "Corrector", Price = 90, Category = "Maquillaje" },
                new Product { Name = "Labial", Price = 150, Category = "Maquillaje" },
                new Product { Name = "Polvo", Price = 110, Category = "Maquillaje" },
                new Product { Name = "Sombras", Price = 200, Category = "Maquillaje" },
                new Product { Name = "Cepillo", Price = 50, Category = "Higiene" },
                new Product { Name = "Shampoo", Price = 180, Category = "Higiene" },
                new Product { Name = "Jabón", Price = 40, Category = "Higiene" },
                new Product { Name = "Crema", Price = 220, Category = "Higiene" },
                new Product { Name = "Perfume", Price = 350, Category = "Higiene" }
            };

            foreach (var product in sampleProducts)
                _repository.Insert(product);

            return Ok("10 productos de ejemplo insertados correctamente.");
        }
    }
}
