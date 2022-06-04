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
        public ParkingArrangement(AppUser buyer, ParkingSpot parentSpot, DateTime startTime, DateTime endTime)
        { 
            Id = id;
            Buyer = buyer;
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
        public void findVacantSpot()
        {
            ParentSpot.Number = DataServices.GetVacantSlotsList(this);
        }

    }
}