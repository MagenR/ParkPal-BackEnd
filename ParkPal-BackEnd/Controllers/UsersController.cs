using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ParkPal_BackEnd.Models;
using ParkPal_BackEnd.Models.DAL;

namespace ParkPal_BackEnd.Controllers
{
    public class UsersController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [Route("api/login")]
        public IHttpActionResult Get(string login, string password)
        {
            try
            {
                AppUser u = AppUser.Get(login, DataServices.LoginType.Password, password);
                if (u == null)
                    return Content(HttpStatusCode.Conflict, "Error. Login failed. no such user exists.");
                return Ok(u);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error. Could not login.\n" + ex.Message);
            }
        }

        [Route("api/ValidateUsername")]

        [Route("api/ValidateEmail")]

        // POST api/<controller>
        [Route("api/signup")]
        public IHttpActionResult Post([FromBody] AppUser u)
        {
            try
            {
                if (u.Insert() == 0)
                    return Content(HttpStatusCode.Conflict, "Error. User could not be created.");
                return Ok("Registration succesful!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error. User could not be created.\n" + ex.Message);
            }
        }

        // PUT api/<controller>/5
        [Route("api/update")]
        public IHttpActionResult Put([FromBody] AppUser u)
        {
            try
            {
                if (u.Update() == 0)
                    return Content(HttpStatusCode.Conflict, "Error. Could not update user's info.");
                return Ok("Info was updated succesfully!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error. Could not update user's info.\n" + ex.Message);
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}