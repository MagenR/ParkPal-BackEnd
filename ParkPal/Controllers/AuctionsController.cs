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
    [RoutePrefix("api/Auctions")]
    public class AuctionsController : ApiController
    {
        //--------------------------------------------------------------------------------------------------
        // GET
        //--------------------------------------------------------------------------------------------------

        // Gets all auction in a parking lot corresponding to a given time slot
        //[HttpGet]
        //[Route("auctioncampaigns")]
        //public IHttpActionResult GetAuctionCampaign(DateTime StartTime, DateTime EndTime)
        //{
        //    try
        //    {
        //        AuctionCampaign ac = new AuctionCampaign(); // Later give lot, and space.
        //        ac.GetAuctionCampaign(StartTime, EndTime);
        //        if (ac == null)
        //            return Content(HttpStatusCode.Conflict, "No parking auction campaign found.");
        //        return Ok(ac);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.Conflict, "Error. Could not process parking campaign fetch request.\n" + ex.Message);
        //    }
        //}

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        //--------------------------------------------------------------------------------------------------
        // POST
        //--------------------------------------------------------------------------------------------------

        // Inserts a parking arrangement and registers it as an auction.
        public IHttpActionResult Post([FromBody] Auction a)
        {
            if (a == null)
                return BadRequest();
            try
            {
                if (a.Insert() == 0)
                    return Content(HttpStatusCode.Conflict, "Error. Could not reserve parking and auction.");
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error. parking reservation and auction creation failed.\n" + ex.Message);
            }
        }

        // Optional - Registers an existing arrangemnt as an auction.



        // Inserts a new bidder to the auction campaign.
        //[HttpPost]
        //[Route("postacbidder")]
        //public IHttpActionResult PostBidder([FromBody] Bidder b)
        //{
            //try
            //{
            //    if (DataServiceDemo.ac.Insert(b) == 0)
            //        return Content(HttpStatusCode.Conflict, "Error. Could not insert the bidder to the campaign.");
            //    return Ok("bidder added succesfully!");
            //}
            //catch (Exception ex)
            //{
            //    return Content(HttpStatusCode.Conflict, "Error.Adding bidder failed.\n" + ex.Message);
            //}
        //}

        //--------------------------------------------------------------------------------------------------
        // PUT
        //--------------------------------------------------------------------------------------------------

        // Update a bidder's bid (if a single bidder form exists).
        //[HttpPut]
        //[Route("putacbidder")]
        //public IHttpActionResult PutBidder([FromBody] Bidder b)
        //{
            //try
            //{
            //    if (DataServiceDemo.ac.Update(b) == 0)
            //        return Content(HttpStatusCode.Conflict, "Error. Could not update the bidder's bid.");
            //    return Ok("bidder's bid updated succesfully!");
            //}
            //catch (Exception ex)
            //{
            //    return Content(HttpStatusCode.Conflict, "Error.Updating bidder's bid failed.\n" + ex.Message);
            //}
        //}

        //// Update a bidder list with new bid (if editing table and pressing save).
        
        //[HttpPut]
        //[Route("putacbidderlist")]
        //public IHttpActionResult PutBidderList([FromBody] List<Bidder> bc)
        //{
        //    try
        //    {
        //        if (DataServiceDemo.ac.Update(bc) == 0)
        //            return Content(HttpStatusCode.Conflict, "Error. Could not update the bidder list.");
        //        return Ok("bidder list updated succesfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.Conflict, "Error.Updating bidder list failed.\n" + ex.Message);
        //    }
        //}

        //--------------------------------------------------------------------------------------------------
        // DELETE
        //--------------------------------------------------------------------------------------------------

        // Removes auction on an existing parking arrangement.

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}
