using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Catalog.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbOptions>(options => 
                configuration.GetSection(MongoDbOptions.DatabaseSettings).Bind(options));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICatalogContext, CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }


        //public static IServiceCollection AddSwagger(this IServiceCollection services, string xmlFileName)
        //{
        //    services.AddSwaggerGen(doc =>
        //    {
        //        doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media API", Version = "v1" });

        //        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
        //        doc.IncludeXmlComments(xmlPath);
        //    });

        //    return services;
        //}
    }
}
