using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodRecord.Data.Entity
{
    public class Article
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ArticleId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }
        
        public int Category { get; set; }

        public int ClickCount { get; set; }
    }
}
