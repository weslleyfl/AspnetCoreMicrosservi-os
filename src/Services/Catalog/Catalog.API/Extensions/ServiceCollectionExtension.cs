using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Infrastructure.Filters;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace Catalog.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<MongoDbOptions>(configuration.GetSection(MongoDbOptions.DatabaseSettings));

            services.AddSingleton<IMongoDbOptions>(options =>
                options.GetRequiredService<IOptions<MongoDbOptions>>().Value);

            services.AddSingleton<IMongoClient>(options =>
                new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString")));


            return services;
        }

        /// <summary>
        /// Add services to the container
        /// </summary>    
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {

            services.AddScoped<ICatalogContext, CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }

        public static IServiceCollection AddCustomMVC(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).AddNewtonsoftJson();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });


            return services;

        }


        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(doc =>
            {
                //doc.DescribeAllEnumsAsStrings();
                doc.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Catalog.API",
                    Version = "v1",
                    Description = "The Catalog Microservice HTTP API. This is a Data-Driven/CRUD microservice sample"
                });

            });

            return services;
        }
    }
}
