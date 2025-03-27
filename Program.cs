using Microsoft.AspNetCore.Builder;
using System;
using macro_tracker_web_service.Services;
using macro_tracker_web_service.Middleware;

namespace macro_tracker_web_service
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddHttpClient<FoodService>(client =>
            {
                var coreServiceUrl = builder.Configuration["CoreServiceUrl"] ?? "http://localhost:5105/api/";
                client.BaseAddress = new Uri(coreServiceUrl);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMacroExceptionHandler();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}