
using CommandCenter.Entity;
using CommandCenter.MongoDB;
using CommandCenter.Setting;
using CommandCenter.Settings;
using Common.MassTransit;
using MassTransit;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CommandCenter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMongo().AddMongoRepository<Order>("pizzaItems").AddMassTransitWithRabbitMQ();

            builder.Services.AddControllers( options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            } );
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CommandCenter", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CommandCenter v1"));
            }

            app.UseHttpsRedirection();


            app.UseRouting();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}