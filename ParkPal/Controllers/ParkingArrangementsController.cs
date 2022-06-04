﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ParkPal_BackEnd.Models;
using ParkPal_BackEnd.Models.DAL;

namespace ParkPal_BackEnd.Controllers
{
    [RoutePrefix("api/parkingarrangements")]
    public class ParkingArrangementsController : ApiController
    {
        //--------------------------------------------------------------------------------------------------
        // GET
        //--------------------------------------------------------------------------------------------------

        // GET request - Returns list of parking arrangments from (history/future schedule).
        private IHttpActionResult Get(int user_id, DataServices.IdSearchType idType, DataServices.Period period)
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

        // GET request - Returns list of parking arrangments, history.
        [HttpGet]
        [Route("api/getPastReservations")]
        public IHttpActionResult GetPastReservations(int user_id)
        {
            return Get(user_id, DataServices.IdSearchType.User, DataServices.Period.Past);
        }

        // GET request - Returns list of parking arrangments, future schedule.
        [HttpGet]
        [Route("api/getFutureReservations")]
        public IHttpActionResult GetFutureReservations(int user_id)
        {
            return Get(user_id, DataServices.IdSearchType.User, DataServices.Period.Future);
        }

        public IHttpActionResult GetFreeSlots(int parkingLot_id)
        {
            return Get(parkingLot_id, DataServices.IdSearchType.ParkingLot, DataServices.Period.Future);
        }

        //--------------------------------------------------------------------------------------------------
        // POST
        //--------------------------------------------------------------------------------------------------

        // POST request - Inserts a parking arrangement into the DB.
        [HttpPost]
        [Route("reserve")]
        public IHttpActionResult Post([FromBody] ParkingArrangement pa)
        {
            if (pa == null)
                return BadRequest();
            try
            {
                pa.findVacantSpot();
                //if (pa.ParentSpot.Number == 0 || pa.Insert() == 0)
                if (pa.ParentSpot.Number == 0)
                    return Content(HttpStatusCode.Conflict, "Error. Could not reserve parking.");
                return Ok(pa.ParentSpot.Number);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, "Error. parking reservation failed.\n" + ex.Message);
            }
        }

        //--------------------------------------------------------------------------------------------------
        // PUT
        //--------------------------------------------------------------------------------------------------

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