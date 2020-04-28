
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpringMvc.Datalayer
{
   public class MongoUserDBContext : IMongoUserDBContext
    {
            private IMongoDatabase _db { get; set; }
            private MongoClient _mongoClient { get; set; }
            public IClientSessionHandle Session { get; set; }
            public MongoUserDBContext(IOptions<Mongosettings> configuration)
            {
                _mongoClient = new MongoClient(configuration.Value.Connection);
                _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);

            }

            public IMongoCollection<T> GetCollection<T>(string name)
            {
                return _db.GetCollection<T>(name);
            }
        }
}
