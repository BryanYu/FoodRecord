using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodRecord.Data.Entity;
using FoodRecord.Worker.Repository;
using MongoDB.Driver;

namespace FoodRecord.Data.Repository
{
    public class ArticleRepository : MongoRepository<Article>
    {
        public ArticleRepository(IMongoClient client, string dataBase, string collection) : base(client, dataBase,
            collection)
        {
        }
    }
}
