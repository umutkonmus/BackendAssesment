
using Microsoft.EntityFrameworkCore;
using ReportService.DatabaseContext;
using ReportService.Services.Abstract;
using ReportService.Services;
using ReportService.Mapper;

namespace ReportService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<PostgresDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddScoped<IReportService, Services.ReportService>();
            builder.Services.AddScoped<IDirectoryServiceClient, DirectoryServiceClient>();
            builder.Services.AddHostedService<KafkaConsumerService>();


            builder.Services.AddHttpClient("DirectoryService", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["DirectoryServiceUrl"] ?? "http://directoryservice:8080");
            });

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
