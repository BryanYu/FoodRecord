using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodRecord.Service.Model.Enum;

namespace FoodRecord.Service.Model.DTO
{
    public class ArticleDataDTO
    {
        public string Id { get; set; }
        
        public string Title { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }

        public ArticleCategory Category { get; set; }
        
    }
}
