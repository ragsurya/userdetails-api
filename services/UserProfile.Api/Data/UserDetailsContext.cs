using System;
using MongoDB.Driver;
using UserProfile.Api.Models;
using Microsoft.Extensions.Options;

namespace UserProfile.Api.Data
{
    public class UserDetailsContext
    {
        private readonly IMongoDatabase _database = null;

        public UserDetailsContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if(client != null){
                _database = client.GetDatabase(settings.Value.Database);
            }
        }

        public IMongoCollection<UserProfileModel> UserDetails
        {
            get{
                return _database.GetCollection<UserProfileModel>("UserDetails");
            }
        }
    }
}