using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodRecord.Data.Entity;

namespace FoodRecord.Service.Services
{
    public interface IArticleService
    {
        public Task AddAsync(Article entity);
    }
}
