using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.API.Data
{
    public class MongoDbOptions : IMongoDbOptions
    {
        public const string DatabaseSettings = "DatabaseSettings";

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
