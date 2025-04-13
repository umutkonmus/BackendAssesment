
using DirectoryService.DatabaseContext;
using DirectoryService.Mapper;
using DirectoryService.Services.Abstracts;
using DirectoryService.Services.Concretes;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IPersonService, PersonService>();
            builder.Services.AddScoped<IContactTypeService, ContactTypeService>();
            builder.Services.AddScoped<IContactInfoService, ContactInfoService>();
            builder.Services.AddSingleton<IKafkaProducerService,KafkaProducerService>();

            builder.Services.AddDbContext<PostgresDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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
