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
    public class ParkingArrangementsController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int user_id, DataServices.Period period)
        {
            try
            {
                List<ParkingArrangement> pas = ParkingArrangement.Get(user_id, period);
                if (pas == null)
                    return Content(HttpStatusCode.Conflict, "Error. Login failed. no such user exists.");
                return Ok(pas);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error. Could not login.\n" + ex.Message);
            }
        }

        [Route("api/getPastReservations")]
        public IHttpActionResult GetPastReservations(int user_id)
        {
            return Get(user_id, DataServices.Period.Past);
        }

        [Route("api/getFutureReservations")]
        public IHttpActionResult GetFutureReservations(int user_id)
        {
            return Get(user_id, DataServices.Period.Future);
        }

        // POST api/<controller>
        public IHttpActionResult Post([FromBody] ParkingArrangement pa)
        {
            try
            {
                if (pa.Insert() == 0)
                    return Content(HttpStatusCode.Conflict, "Error. Could not reserve parking.");
                return Ok("Parking reserved succesfully!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error. parking reservation failed.\n" + ex.Message);
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}