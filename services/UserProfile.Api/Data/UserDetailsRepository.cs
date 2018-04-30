using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using UserProfile.Api.Interfaces;
using UserProfile.Api.Models;

namespace UserProfile.Api.Data {
    public class UserDetailsRepository : IUserDetailsRepository {
        private readonly UserDetailsContext _context = null;
        public UserDetailsRepository (IOptions<Settings> settings) {
            _context = new UserDetailsContext (settings);
        }

        public async Task<UserProfileModel> GetUserDetailAsync (string email) {
            try {
                ObjectId internalId = GetInternalId (email);
                Task<UserProfileModel> userProfileTask = await Task.FromResult (_context.UserDetails.Find (user => user.Email == email).FirstOrDefaultAsync ());
                await userProfileTask.ContinueWith (t => {
                    if (t.IsFaulted) {
                        var exceptionsInTask = t.Exception.Flatten ().InnerExceptions;
                        foreach (var ex in exceptionsInTask) {
                            //Log the exceptions. Alert if need be
                        }
                    }
                    if (t.IsCanceled) {

                    }
                    if (t.Status == TaskStatus.RanToCompletion) {
                        //publish the metrics
                    }
                });
                return userProfileTask.Result;
            } catch (Exception ex) {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateUserDetailAsync (string email, UserProfileModel model) {
            var cancellationTokenSource = new CancellationTokenSource ();
            var filter = Builders<UserProfileModel>.Filter.Eq (s => s.Email, email);
            var update = Builders<UserProfileModel>.Update
                .Set (s => s.Address.AddressLine1, model.Address.AddressLine1)
                .Set (s => s.DateOfBirth, model.DateOfBirth);
            var options = new UpdateOptions {
                IsUpsert = true
            };
            try {
                UpdateResult updateResult = await _context.UserDetails.UpdateOneAsync (filter, update, options, cancellationTokenSource.Token);
                return updateResult.IsAcknowledged;

            } catch (Exception ex) {
                //Log the error
                throw;
            }
        }

        public async Task AddUserDetail (UserProfileModel item) {
            try {
                await _context.UserDetails.InsertOneAsync (item);
            } catch (Exception ex) {
                // log or manage the exception
                throw ex;
            }
        }

        public ObjectId GetInternalId (string id) {
            ObjectId internalId;
            if (!ObjectId.TryParse (id, out internalId)) {
                internalId = ObjectId.Empty;
            }
            return internalId;
        }

        public async Task<string> CreateDatabaseAndIndex () {
            try{
            return await _context.UserDetails.Indexes
                .CreateOneAsync(Builders<UserProfileModel>
                                .IndexKeys
                                .Ascending(item => item.UserId)
                                .Ascending(item => item.UserName)
                                .Ascending(item => item.Email)
                                .Ascending(item => item.DateOfBirth)
                                .Ascending(item => item.Address));
            }
            catch(Exception ex){
                //Log or manage the exception
                throw ex;
            }

        }

        public async Task<bool> RemoveAllUserDetails()
        {
            try
            {
                DeleteResult actionResult = await _context.UserDetails.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}