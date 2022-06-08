using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodRecord.Service.Model.Enum;
using Newtonsoft.Json.Linq;

namespace FoodRecord.Service.DataProcess
{
    public interface IDataProcess
    {
        ProcessType ProcessType { get; }

        Task ProcessAsync(JObject jObject);
    }
}
