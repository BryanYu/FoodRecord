using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodRecord.Data.Entity;
using FoodRecord.Worker.Repository;

namespace FoodRecord.Service.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMongoRepository<Article> _repository;

        public ArticleService(IMongoRepository<Article> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Article entity)
        {
            await _repository.AddAsync(entity);
        }
    }
}
