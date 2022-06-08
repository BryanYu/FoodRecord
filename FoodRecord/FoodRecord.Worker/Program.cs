using System.Reflection;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using FoodRecord.Data.Entity;
using FoodRecord.Data.Repository;
using FoodRecord.Service.DataProcess;
using FoodRecord.Service.Extension;
using FoodRecord.Service.Services;
using FoodRecord.Worker.Model.Settings;
using FoodRecord.Worker.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace FoodRecord.Worker
{
    internal class Program
    {
        static Task Main(string[] args) =>
            CreateHostBuilder(args).Build().RunAsync();

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);
            builder.ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                configuration.Sources.Clear();
                IHostEnvironment env = hostingContext.HostingEnvironment;
                configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
                configuration.AddEnvironmentVariables();
                configuration.AddCommandLine(args); 
            });

            
            builder.ConfigureServices((hostingContext, services) =>
            {
                var awsSetting = new AwsSetting();
                var section = hostingContext.Configuration.GetSection("AwsSetting");
                section.Bind(awsSetting);
                var mongoDbConnection = hostingContext.Configuration.GetConnectionString("MongoDbConnection");


                services.Configure<AwsSetting>(section);
                services.AddSingleton<IAmazonSQS, AmazonSQSClient>(item =>
                    {
                        var sqsClient = new AmazonSQSClient(
                            new BasicAWSCredentials(awsSetting.AccessKey, awsSetting.SecretKey)
                            , new AmazonSQSConfig
                            {
                                ServiceURL = awsSetting.SQS.ServiceUrl
                            });
                        return sqsClient;
                    })
                    .AddHostedService<QueueReceiver>();
                
                services.AddSingleton<IMongoClient, MongoClient>(item => new MongoClient(mongoDbConnection));
                
                services.AddSingleton<IMongoRepository<Article>, ArticleRepository>(item =>
                {
                    var mongoClient = item.GetRequiredService<IMongoClient>();
                    return new ArticleRepository(mongoClient, "foodrecord", "articles");
                });
                services.AddSingleton<IArticleService, ArticleService>();

                services.AddAllServiceOfInterface<IDataProcess>(ServiceLifetime.Singleton);

            });
            return builder;
        }


        
    }
}
