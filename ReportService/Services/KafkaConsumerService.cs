using Confluent.Kafka;
using Newtonsoft.Json;
using ReportService.Services.Abstract;
using ReportService.DTOs.Report;

namespace ReportService.Services
{
    public sealed class KafkaConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly string _topic = "report-request";
        private readonly IConsumer<Ignore, string> _consumer;

        public KafkaConsumerService(IServiceProvider serviceProvider, ILogger<KafkaConsumerService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            var config = new ConsumerConfig
            {
                BootstrapServers = "kafka:9092",
                GroupId = "report-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                AllowAutoCreateTopics = true,
                EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic); 
            await Task.Delay(2000, stoppingToken); 

            _logger.LogInformation($"Kafka consumer started, listening on topic: {_topic}");
            
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = _consumer.Consume(stoppingToken);
                    if (result != null)
                    {
                        _logger.LogInformation($"Message received: {result.Message.Value}");

                        try
                        {
                            await ProcessMessageAsync(result.Message.Value);
                            _consumer.Commit(result);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error while processing the Kafka message");
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Kafka consumer cancellation requested.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Kafka consumer");
            }
            finally
            {
                _consumer.Close();
            }
        }

        private async Task ProcessMessageAsync(string message)
        {
            try
            {
                var reportRequest = JsonConvert.DeserializeObject<ReportRequestDTO>(message);

                if (reportRequest == null || string.IsNullOrEmpty(reportRequest.Location))
                {
                    _logger.LogError("Invalid report request message format");
                    return;
                }

                using var scope = _serviceProvider.CreateScope();
                var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();

                var createResult = await reportService.CreateReportAsync(reportRequest.Location);

                if (createResult.IsSuccessful && createResult.Data != null)
                {
                    await reportService.ProcessReportAsync(createResult.Data.ID, reportRequest.Location);
                }
                else
                {
                    _logger.LogError($"Failed to create report: {createResult.Message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing report request");
            }
        }

        public override void Dispose()
        {
            _consumer?.Close();
            _consumer?.Dispose();
            base.Dispose();
        }
    }
}
