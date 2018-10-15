using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace TVMaze_Scrapper.Services
{
    public static class MongoDBServices
    {
        private static readonly string ConnectionString = "mongodb://localhost/";

        // gets collection based on the object type and given collection
        public static IMongoCollection<T> GetCollection<T>(string collection, string db = "config")
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(db);
            var data = database.GetCollection<T>(collection);
            return data;
        }

        // gets bson data of the given collection
        public static IMongoCollection<BsonDocument> GetCollection(string collection, string db = "config")
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(db);
            var data = database.GetCollection<BsonDocument>(collection);
            return data;
        }

        // insert operation on single object data
        public static void InsertSingleData<T>(string collectionName, T Object)
        {
            GetCollection<T>(collectionName).InsertOne(Object);
        }

        // insert operation on nultiple object data
        public static void InsertMultipleData<T>(string collectionName, List<T> Object)
        {
            GetCollection<T>(collectionName).InsertMany(Object);
        }

        // remove all from given collection
        public static void ClearCollection(string collectionName)
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            GetCollection(collectionName).DeleteMany(filter);
        }
    }
}
