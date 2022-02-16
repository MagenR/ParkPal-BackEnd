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
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        //--------------------------------------------------------------------------------------------------
        // GET
        //--------------------------------------------------------------------------------------------------
        [HttpGet]
        [Route("loginUser")]
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

        private IHttpActionResult Get(string login, DataServices.LoginType type)
        {
            try
            {
                AppUser u = AppUser.Get(login, type);
                if (u == null)
                    return Ok();
                return Content(HttpStatusCode.Conflict, "Error. " + type.ToString() + " already exists.");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error. Could not process request.\n" + ex.Message);
            }
        }

        [HttpGet]
        [Route("{username}/ValdiateUsername")]
        public IHttpActionResult ValdiateUsername(string username)
        {
            return Get(username, DataServices.LoginType.Username);
        }

        [HttpGet]
        [Route("{email}/ValidateEmail")]
        public IHttpActionResult ValidateEmail(string email)
        {
            return Get(email, DataServices.LoginType.Email);
        }

        //--------------------------------------------------------------------------------------------------
        // POST
        //--------------------------------------------------------------------------------------------------
        [HttpPost]
        [Route("signup")]
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

        //--------------------------------------------------------------------------------------------------
        // PUT
        //--------------------------------------------------------------------------------------------------
        [HttpPut]
        [Route("update")]
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

        //--------------------------------------------------------------------------------------------------
        // DELETE
        //--------------------------------------------------------------------------------------------------
        public void Delete(int id)
        {
        }

    }
}