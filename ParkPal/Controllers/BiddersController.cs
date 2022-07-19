using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ParkPal_BackEnd.Models;
using ParkPal_BackEnd.Models.DAL;

namespace ParkPal.Controllers
{
    [RoutePrefix("api/Bidders")]
    public class BiddersController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

       // Inserts a new bidder to the auction campaign.
       [HttpPost]
       [Route("postacbidder")]
        public IHttpActionResult PostBidder([FromBody] Bidder b)
        {
            try
            {
                if (b.Insert() == 0)
                    return Content(HttpStatusCode.Conflict, "Error. Could not insert the bidder to the campaign.");
                return Ok(b.AuctionsRunLog);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error.Adding bidder failed.\n" + ex.Message);
            }
        }

        // Update a bidder's bid (if a single bidder form exists).
        [HttpPut]
        [Route("putacbidder")]
        public IHttpActionResult PutBidder([FromBody] Bidder b)
        {
            try
            {
                if (b.Update() == 0)
                    return Content(HttpStatusCode.Conflict, "Error. Could not update the bidder's bid.");
                return Ok(b.AuctionsRunLog);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error.Updating bidder's bid failed.\n" + ex.Message);
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}