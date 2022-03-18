using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ParkPal_BackEnd.Models;

namespace ParkPal_BackEnd.Controllers
{
    [RoutePrefix("api/auctions")]
    public class AuctionsController : ApiController
    {
        [HttpGet]
        [Route("auctioncampaigns")]
        public IHttpActionResult Get()
        {
            try
            {
                List<AuctionCampaign> ac = ParkingLot.Get(); // Later give lot, and space.
                if (ac == null)
                    return Content(HttpStatusCode.Conflict, "No parking campaign found.");
                return Ok(ac);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error. Could not process parking campaign fetch request.\n" + ex.Message);
            }
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public IHttpActionResult Post([FromBody] AuctionCampaign ac)
        {

        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
