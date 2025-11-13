using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAPI.MODEL
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();


        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }

        [BsonElement("Category")]
        public string Category { get; set; }
    }
}
