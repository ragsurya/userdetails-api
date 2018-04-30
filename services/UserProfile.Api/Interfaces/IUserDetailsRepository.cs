using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserProfile.Api.Models;

namespace UserProfile.Api.Interfaces {
    public interface IUserDetailsRepository {
        Task<UserProfileModel> GetUserDetailAsync (string email);
        Task<bool> UpdateUserDetailAsync (string email, UserProfileModel userModel);
        Task  AddUserDetail (UserProfileModel item);
        Task<bool> RemoveAllUserDetails();
        Task<string> CreateDatabaseAndIndex ();
    }
}