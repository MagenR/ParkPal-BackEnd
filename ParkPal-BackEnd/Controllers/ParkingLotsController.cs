using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ParkPal_BackEnd.Models;

namespace ParkPal_BackEnd.Controllers
{
    [RoutePrefix("api/parkinglots")]
    public class ParkingLotsController : ApiController
    {
        [HttpGet]
        [Route("SearchVacant")]
        public IHttpActionResult Get(DateTime startTime, DateTime endTime)
        {
            try
            {
                List<ParkingLot> pls = ParkingLot.Get(startTime, endTime);
                if (pls == null)
                    return Content(HttpStatusCode.Conflict, "No mathcing parking lots found.");
                return Ok(pls);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error. Could not process request.\n" + ex.Message);
            }
        }

        [HttpGet]
        [Route("SearchMath")]       
        public IHttpActionResult Get(int latitude, int longitude, DateTime startTime, DateTime endTime)
        {
            try
            {
                List<ParkingLot> pls = ParkingLot.Get(latitude, longitude, startTime, endTime);
                if (pls == null)
                    return Content(HttpStatusCode.Conflict, "No matching parking lots found.");
                return Ok(pls);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error. Could not process request.\n" + ex.Message);
            }
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
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