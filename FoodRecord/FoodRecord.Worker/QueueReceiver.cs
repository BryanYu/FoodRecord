using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using FoodRecord.Service;
using FoodRecord.Service.DataProcess;
using FoodRecord.Service.Model.Enum;
using FoodRecord.Worker.Model.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace FoodRecord.Worker
{
    public class QueueReceiver : BackgroundService
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly Dictionary<ProcessType, IDataProcess> _dataProcessMapping;
        private readonly AwsSetting _awsSetting;
        private readonly ILogger<QueueReceiver> _logger;

        public QueueReceiver(IAmazonSQS sqsClient, IOptions<AwsSetting> awsSetting,
            IEnumerable<IDataProcess> dataProcesses, ILogger<QueueReceiver> logger)
        {
            _sqsClient = sqsClient;
            _dataProcessMapping = dataProcesses.ToDictionary(key => key.ProcessType, value => value);
            _awsSetting = awsSetting.Value;
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var response =
                    await _sqsClient.ReceiveMessageAsync(
                        new ReceiveMessageRequest
                        {
                            QueueUrl = _awsSetting.SQS.QueueUrl,
                            MaxNumberOfMessages = 10,
                            WaitTimeSeconds = 5
                        }, stoppingToken);
                if (response.HttpStatusCode != HttpStatusCode.OK)
                {
                    _logger.LogWarning(
                        $"Access QueueUrl:{_awsSetting.SQS.QueueUrl} not work, HttpStatusCode:{response.HttpStatusCode}");
                    continue;
                }

                if (!response.Messages.Any() || response.Messages == null)
                {
                    continue;
                }

                foreach (var message in response.Messages)
                {
                    var jObject = JObject.Parse(message.Body);
                    var type = jObject.Value<string>("processType");

                    if (!Enum.TryParse(typeof(ProcessType), type, true, out var output))
                    {
                        _logger.LogWarning(
                            $"Receive Messages is null or empty");
                        continue;
                    }

                    ProcessType processType = (ProcessType)output;

                    if (!_dataProcessMapping.ContainsKey(processType))
                    {
                        _logger.LogWarning($"dataProcess not found");
                        continue;
                    }
                    var dataProcess = _dataProcessMapping[processType];
                    var data = jObject.Value<JObject>("data");
                    await dataProcess.ProcessAsync(data);
                    
                    await _sqsClient.DeleteMessageAsync(
                        new DeleteMessageRequest(_awsSetting.SQS.QueueUrl, message.ReceiptHandle), stoppingToken);
                }

                var delayTime = TimeSpan.FromSeconds(_awsSetting.SQS.DelaySecond);
                await Task.Delay(delayTime, stoppingToken);
            }
        }
        
    }
}
