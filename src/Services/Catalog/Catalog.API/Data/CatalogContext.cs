using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {

      
        //private readonly MongoDbOptions _mongoDbOptions;

        public CatalogContext(IMongoDbOptions options)
        {
            //_mongoDbOptions = options ?? throw new ArgumentNullException(nameof(options));

            var client = new MongoClient(options.ConnectionString);
            var database = client.GetDatabase(options.DatabaseName);

            Products = database.GetCollection<Product>(options.CollectionName);
            // CatalogContextSeed.SeedData(Products);

        }

        // Ex: DbSet 
        public IMongoCollection<Product> Products { get; }
        


    }
}
