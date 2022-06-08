using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodRecord.Data.Entity;
using FoodRecord.Service.Model.DTO;
using FoodRecord.Service.Model.Enum;
using FoodRecord.Service.Services;
using Newtonsoft.Json.Linq;

namespace FoodRecord.Service.DataProcess
{
    public class AddArticleProcess : IDataProcess
    {
        private readonly IArticleService _articleService;
        public ProcessType ProcessType => ProcessType.AddArticle;
        public AddArticleProcess(IArticleService articleService)
        {
            _articleService = articleService;
        }
        public async Task ProcessAsync(JObject jObject)
        {
            var dto = jObject.ToObject<ArticleDataDTO>();
            var entity = new Article
            {
                ArticleId = dto.Id,
                Title = dto.Title,
                Content = dto.Content,
                Author = dto.Author,
                Category = (int)dto.Category,
                ClickCount = 1
            };
            await _articleService.AddAsync(entity);
        }
    }
}
