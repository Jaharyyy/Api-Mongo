using System;
using System.Collections.Generic;
using MongoDB.Driver;
using WebAPI.MODEL;

namespace WebAPI.DATAS
{
    public class ProductRepository
    {
        private readonly MongoDbContext _context;

        public ProductRepository(MongoDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAll() => _context.Products.Find(p => true).ToList();

        public Product GetById(Guid id) =>
            _context.Products.Find(p => p.Id == id).FirstOrDefault();

        public void Insert(Product product) => _context.Products.InsertOne(product);

        public void Update(Guid id, Product product) =>
            _context.Products.ReplaceOne(p => p.Id == id, product);

        public void Delete(Guid id) =>
            _context.Products.DeleteOne(p => p.Id == id);
    }
}
