using Confluent.Kafka;
using DirectoryService.DTOs.Report;
using DirectoryService.Services.Abstracts;
using Newtonsoft.Json;

namespace DirectoryService.Services.Concretes
{
    public sealed class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic = "report-request";

        public KafkaProducerService()
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }
        public async Task SendReportRequestAsync(ReportRequestDTO dto)
        {
            var message = JsonConvert.SerializeObject(dto);
            await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
        }
    }
}
