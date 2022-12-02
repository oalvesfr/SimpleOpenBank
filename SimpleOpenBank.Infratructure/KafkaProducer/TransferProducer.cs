using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using SimpleOpenBank.Application.Contracts.Infratructure;
using SimpleOpenBank.Application.Models;
using SimpleOpenBank.Infratructure.Mail;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleOpenBank.Infratructure.Producer
{
    public class TransferProducer : ITransferProducer
    {
        private readonly IProducer<int, string> _producerBuilder;
        private readonly string _topic;

        public TransferProducer(IConfiguration configuration)
        {
            var producer = configuration.GetSection("KafkaProducer").GetChildren().ToDictionary(x => x.Key, x => x.Value).ToList();

            if (producer.Any())
            {
                var producerConfig = new ProducerConfig()
                {
                    BootstrapServers = producer.FirstOrDefault(x => x.Key.Equals("BootstrapServer")).Value
                };
                _producerBuilder = new ProducerBuilder<int, string>(producerConfig).Build();

                _topic = producer.FirstOrDefault(x => x.Key.Equals("TopicName")).Value;
            }
        }

       public async Task PublishEvent(Notificacao notificacao)
        {
            var message = new Message<int, string>
            {
                Key = notificacao.UserId,
                Value = JsonSerializer.Serialize(notificacao)
            };

            await Publish(message);
        }
        private async Task<(DeliveryResult<int, string>, string)> Publish(Message<int, string> message)
        {
            try
            {
                return (await _producerBuilder.ProduceAsync(_topic, message), string.Empty);
            }
            catch (ProduceException<int, string> ex)
            {
                return (null, ex.Error.Reason);
            }
        }
    }
}
