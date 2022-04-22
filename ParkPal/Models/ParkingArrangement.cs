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

        // Full constructor
        public ParkingArrangement(int id, AppUser buyer, ParkingSpot parentSpot, DateTime startTime, DateTime endTime) : this( buyer,  startTime,  endTime)
        {
            Id = id;
            EndTime = endTime;
            ParentSpot = parentSpot;
        }

        // For parking lots.
        public ParkingArrangement(AppUser buyer, DateTime startTime, DateTime endTime)
        {
            Buyer = buyer;
            StartTime = startTime;
            EndTime = endTime;
        }

        // For users.
        public ParkingArrangement(ParkingSpot parentSpot, DateTime startTime, DateTime endTime)
        {
            ParentSpot = parentSpot;
            StartTime = startTime;
            EndTime = endTime;
        }

        //--------------------------------------------------------------------------------------------------
        // Methods 
        //--------------------------------------------------------------------------------------------------

        // Returns the list of parking arrangements for given user and time period.
        public static List<ParkingArrangement> Get(int searcher_id, DataServices.Period datePeriod)
        {
            return DataServices.GetParkingArrangements(searcher_id, datePeriod);
        }

    }
}