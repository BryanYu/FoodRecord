using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FoodRecord.Worker.Repository
{
    public class MongoRepository<T> : IMongoRepository<T>
    {
        protected readonly IMongoClient _client;

        protected readonly IMongoDatabase _dataBase;

        protected readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoClient client, string dataBase, string collection)
        {
            this._client = client;
            this._dataBase = _client.GetDatabase(dataBase);
            this._collection = this._dataBase.GetCollection<T>(collection);
        }

        public virtual async Task AddAsync(T data)
        {
            await this._collection.InsertOneAsync(data);
        }

        public virtual async Task UpdateAsync(string id, T data)
        {
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            await this._collection.FindOneAndReplaceAsync(filter, data);
        }

        public virtual async Task DeleteAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            await this._collection.DeleteOneAsync(filter);
        }

        public virtual async Task<T> FindAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            var result = await this._collection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public virtual List<T> GetAll()
        {
            return this._collection.Find(FilterDefinition<T>.Empty).ToList();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await this._collection.Find(FilterDefinition<T>.Empty).ToListAsync();
        }

        public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await this._collection.Find(filter).ToListAsync();
        }
    }
}
