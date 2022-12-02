using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using SimpleOpenBank.Application.Contracts.Infratructure;
using SimpleOpenBank.Application.Models;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleOpenBank.Infratructure.Consumer
{
    public class TransferConsumer: BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly string _topic;
        private readonly IConsumer<string, string> _kafkaConsumer;
         private readonly IServiceScopeFactory _serviceScopeFactory;
        public TransferConsumer(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
             var consumerConfig = new ConsumerConfig
            {
                GroupId = "st_consumer_group",
                BootstrapServers = configuration["KafkaConsumer:BootstrapServer"],
                AutoOffsetReset = AutoOffsetReset.Earliest,

            };
            _topic = configuration["KafkaConsumer:TopicName"];
            _kafkaConsumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            new Thread(() => CreateTransferConsumer(stoppingToken)).Start();

            return Task.CompletedTask;
        }

        public async void CreateTransferConsumer(CancellationToken cancellationToken)
        {
            _kafkaConsumer.Subscribe(_topic)
;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumerResult = _kafkaConsumer.Consume(cancellationToken);
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _emailTransferProducer = scope.ServiceProvider.GetService<IEmailTransferProducer>();
                        var notificacao = JsonSerializer.Deserialize<Notificacao>(consumerResult.Message.Value);
                        await _emailTransferProducer.SendEmailTransfer(notificacao);
                    }
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occured: {e.Error.Reason}");
                    break;
                }
            }
        }

        public override void Dispose()
        {
            _kafkaConsumer.Close();
            _kafkaConsumer.Dispose();

            base.Dispose();
        }
    }
}
