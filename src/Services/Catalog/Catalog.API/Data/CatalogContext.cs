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

        // https://www.youtube.com/watch?v=iWTdJ1IYGtg

        // Ex: DbSet 
        public IMongoCollection<Product> Products { get; }

        public CatalogContext(IMongoDbOptions options, IMongoClient mongoClient)
        {

            var database = mongoClient.GetDatabase(options.DatabaseName);

            Products = database.GetCollection<Product>(options.CollectionName);

            CatalogContextSeed.SeedData(Products);

        }





    }
}
