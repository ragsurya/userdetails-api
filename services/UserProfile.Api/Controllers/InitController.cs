using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using UserProfile.Api.Interfaces;
using UserProfile.Api.Models;

namespace UserProfile.Api.Controllers {
    [Route ("api/[controller]")]
    public class InitController : Controller {
        public readonly IUserDetailsRepository _repository;
        public InitController (IUserDetailsRepository repository) {
            _repository = repository;
        }
        // Call an initialization - api/system/init
        [HttpGet ("{setting}")]
        public string Get (string setting) {
            if (setting == "init") {
                _repository.RemoveAllUserDetails ();
                var name = _repository.CreateDatabaseAndIndex ();

                _repository.AddUserDetail (new UserProfileModel () {
                    Email = "hello@gmail.com",
                        DateOfBirth = "27/08/1987",
                        UserId = 100,
                        UserName = "hello.world",
                        Address = new Address {
                            AddressLine1 = "Address line 1 for hello",
                                AddressLine2 = "Address Line 2 for hello",
                                City = "Petrboro",
                                PostCode = "PE7 8GW"
                        }
                });
                _repository.AddUserDetail (new UserProfileModel () {
                    Email = "raghav@gmail.com",
                        DateOfBirth = "27/08/1987",
                        UserId = 101,
                        UserName = "hello.world",
                        Address = new Address {
                            AddressLine1 = "Address line 1 for hello2",
                                AddressLine2 = "Address Line 2 for hello2",
                                City = "Petrboro",
                                PostCode = "PE7 8GW"
                        }
                });
                _repository.AddUserDetail (new UserProfileModel () {
                    Email = "rag1@gmail.com",
                        DateOfBirth = "27/03/1981",
                        UserId = 101,
                        UserName = "rag1.surya",
                        Address = new Address {
                            AddressLine1 = "10 Carolside Grove",
                                AddressLine2 = "Hampton Gardens",
                                City = "Peterborough",
                                PostCode = "PE7 8QF"
                        }
                });

                return "Collections added";
            }

            return "Unknown";
        }

         [HttpGet]
        public ActionResult Ping () {
            return StatusCode ((int) HttpStatusCode.OK);
        }
    }
}