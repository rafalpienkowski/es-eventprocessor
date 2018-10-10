using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StatlerWaldorfCorp.EventProcessor.Models;
using StatlerWaldorfCorp.EventProcessor.Queues;
using StackExchange.Redis;
using RabbitMQ.Client;
using StatlerWaldorfCorp.EventProcessor.Queues.AMQP;
using RabbitMQ.Client.Events;
using StatlerWaldorfCorp.EventProcessor.Location;
using StatlerWaldorfCorp.EventProcessor.Location.Redis;

namespace StatlerWaldorfCorp.EventProcessor
{
    public class Startup
    {
        public Startup(IHostingEnvironment environment,ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            var builder = new ConfigurationBuilder()
                        .SetBasePath(environment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                        .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddOptions();

            services.Configure<QueueOptions>(Configuration.GetSection("QueueOptions"));
            services.Configure<AMQPOptions>(Configuration.GetSection("amqp"));

            //services.AddRedisConnectionMultiplexer(Configuration);

            services.AddTransient<IConnectionFactory, AMQPConnectionFactory>();
            services.AddTransient<EventingBasicConsumer, AMQPEventingConsumer>();

            services.AddSingleton<ILocationCache, RedisLocationCache>();
        }
    }
}