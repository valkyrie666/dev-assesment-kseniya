using AutoMapper;
using DataExporter.Mapper;
using DataExporter.Services;
using Microsoft.OpenApi.Models;

namespace DataExporter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<ExporterDbContext>();
            builder.Services.AddScoped<IPolicyService, PolicyService>();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AddingStuffAndCheckingI", Version = "v1" });
            });

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AddingStuffAndChecking v1");
            });

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
