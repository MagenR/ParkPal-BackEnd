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
        //--------------------------------------------------------------------------------------------------
        // GET
        //--------------------------------------------------------------------------------------------------

       // Gets all auctions in a parking lot corresponding to a given time slot
       [HttpGet]
       [Route("auctionsintime")]
        public IHttpActionResult GetAuctions(int parkingLotId, DateTime StartTime, DateTime EndTime)
        {
            try
            {
                List<Auction> a = Auction.Get(parkingLotId, StartTime, EndTime);
                if (a == null)
                    return Content(HttpStatusCode.Conflict, "No auctions found.");
                return Ok(a);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error. Could not process auctions get request.\n" + ex.Message);
            }
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        //--------------------------------------------------------------------------------------------------
        // POST
        //--------------------------------------------------------------------------------------------------

        // Inserts a parking arrangement and registers it as an auction.
        [HttpPost]
        [Route("reserve")]
        public IHttpActionResult Post([FromBody] Auction a)
        {
            if (a == null)
                return BadRequest();
            try
            {
                if (a.Insert() == 0)
                    return Content(HttpStatusCode.Conflict, "Error. Could not reserve parking and auction.");
                return Ok(a.SoldArrangement.ParentSpot.Number);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error. parking reservation and auction creation failed.\n" + ex.Message);
            }
        }

        // Optional - Registers an existing arrangemnt as an auction.

        //--------------------------------------------------------------------------------------------------
        // PUT
        //--------------------------------------------------------------------------------------------------

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        //--------------------------------------------------------------------------------------------------
        // DELETE
        //--------------------------------------------------------------------------------------------------

        // Removes auction on an existing parking arrangement.

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
