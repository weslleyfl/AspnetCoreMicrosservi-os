namespace Catalog.API.Data
{
    public interface IMongoDbOptions
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}