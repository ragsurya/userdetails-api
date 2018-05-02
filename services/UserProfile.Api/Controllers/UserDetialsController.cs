using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserProfile.Api.Interfaces;
using UserProfile.Api.Models;

namespace UserProfile.Api.Controllers {
    [Produces ("application/json")]
    [Route ("api/[controller]")]
    public class UserDetailsController : Controller {
        private readonly IUserDetailsRepository _userDetailsRepository;

        public UserDetailsController (IUserDetailsRepository repository) {
            _userDetailsRepository = repository;
        }
        // GET api/values
        [HttpGet("{email}")]
        public async Task<UserProfileModel> Get (string email) {
            Console.WriteLine("REQUEST START DATE", DateTime.Now);
            return await _userDetailsRepository.GetUserDetailAsync(email) ?? new UserProfileModel();
            
        }

        // GET api/values/5
        [HttpGet ("{id}")]
        public string Get (int id) {
            return "value";
        }

        // POST api/values
       [HttpPost]
        public void Post([FromBody] UserProfileModel userDetail)
        {
            _userDetailsRepository.AddUserDetail(userDetail);
        }
         // PUT api/notes/5
        [HttpPut("{email}")]
        public void Put(string email, [FromBody]UserProfileModel model)
        {
            _userDetailsRepository.UpdateUserDetailAsync(email, model);
        }

        
    }
}