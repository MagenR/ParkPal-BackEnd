using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ParkPal_BackEnd.Models.DAL;

namespace ParkPal_BackEnd.Models
{
    public class ParkingArrangement : DataBaseObject
    {
        //--------------------------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------------------------

        int id;
        DateTime startTime, endTime;
        AppUser buyer;
        ParkingSpot parentSpot;

        //--------------------------------------------------------------------------------------------------
        // Props
        //--------------------------------------------------------------------------------------------------

        public int Id { get => id; set => id = value; }
        public AppUser Buyer { get => buyer; set => buyer = value; }
        public ParkingSpot ParentSpot { get => parentSpot; set => parentSpot = value; }
        public DateTime StartTime { get => startTime; set => startTime = value; }
        public DateTime EndTime { get => endTime; set => endTime = value; }

        //--------------------------------------------------------------------------------------------------
        // Constructors
        //--------------------------------------------------------------------------------------------------

        // For User's GETs.
        public ParkingArrangement(int id, ParkingSpot parentSpot, DateTime startTime, DateTime endTime)
        {
            Id = id;
            ParentSpot = parentSpot;
            StartTime = startTime;
            EndTime = endTime;
        }

        public ParkingArrangement() { }

        //--------------------------------------------------------------------------------------------------
        // Methods 
        //--------------------------------------------------------------------------------------------------

        // Returns the list of parking arrangements for given user and time period.
        public static List<ParkingArrangement> Get(int searcher_id, DataServices.Period datePeriod)
        {
            return DataServices.GetParkingArrangements(searcher_id, datePeriod);
        }

        // Returns a vacant slot number to be used in the parking arrangemnt reservation.
        public int FindVacantSpot()
        {
            return DataServices.GetVacantSlot(this);
        }

        // Finds a free slot according to query parameters and inserts to DB.
        public override int Insert()
        {
            ParentSpot.Number = FindVacantSpot();
            if (ParentSpot.Number == 0)
                throw new Exception("Could not find a free slot by given query parameters.");
            return base.Insert();
        }

    }
}