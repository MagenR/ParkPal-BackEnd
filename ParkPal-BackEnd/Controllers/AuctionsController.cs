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
    [RoutePrefix("api/auctions")]
    public class AuctionsController : ApiController
    {

        [HttpGet]
        [Route("auctioncampaigns")]
        public IHttpActionResult Get()
        {
            try
            {
                AuctionCampaign ac = DataServiceDemo.ac; // Later give lot, and space.
                if (ac == null)
                    return Content(HttpStatusCode.Conflict, "No parking auction campaign found.");
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

        // Insert a new seller to the auction campaign.
        public IHttpActionResult PostSeller([FromBody] Seller c)
        {
            try
            {
                if (DataServiceDemo.ac.Insert(c) == 0)
                    return Content(HttpStatusCode.Conflict, "Error. Could not insert the seller to the campaign.");
                return Ok("seller added succesfully!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error.Adding seller failed.\n" + ex.Message);
            }
        }

        // Insert a new bidder to the auction campaign.
        public IHttpActionResult PostBidder([FromBody] Bidder b)
        {
            try
            {
                if (DataServiceDemo.ac.Insert(b) == 0)
                    return Content(HttpStatusCode.Conflict, "Error. Could not insert the bidder to the campaign.");
                return Ok("bidder added succesfully!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error.Adding bidder failed.\n" + ex.Message);
            }
        }

        public IHttpActionResult PutBidder([FromBody] List<Bidder> b)
        {
            try
            {
                if (DataServiceDemo.ac.Update(b) == 0)
                    return Content(HttpStatusCode.Conflict, "Error. Could not update the bidder's bid.");
                return Ok("bidder's bid updated succesfully!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error.Updating bidder's bid failed.\n" + ex.Message);
            }
        }

        // Update a bidder with new bid.
        public IHttpActionResult PutBidderList([FromBody] List<Bidder> bc)
        {
            try
            {
                if (DataServiceDemo.ac.Update(bc) == 0)
                    return Content(HttpStatusCode.Conflict, "Error. Could not update the bidder list.");
                return Ok("bidder list updated succesfully!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error.Updating bidder list failed.\n" + ex.Message);
            }
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
