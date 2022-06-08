using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodRecord.Worker.Model.Settings
{
    public class AwsSetting
    {
        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        public SQS SQS { get; set; }
    }
    
    public class SQS
    {
        public string ServiceUrl { get; set; }

        public string QueueUrl { get; set; }

        public int DelaySecond { get; set; }
    }
}
