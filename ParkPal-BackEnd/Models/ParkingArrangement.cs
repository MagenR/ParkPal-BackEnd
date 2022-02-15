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

        DateTime startTime, endTime;
        AppUser buyer;
        ParkingLot reservedAt;

        //--------------------------------------------------------------------------------------------------
        // Props
        //--------------------------------------------------------------------------------------------------

        public AppUser Buyer { get => buyer; set => buyer = value; }
        public ParkingLot ReservedAt { get => reservedAt; set => reservedAt = value; }
        public DateTime StartTime { get => startTime; set => startTime = value; }
        public DateTime EndTime { get => endTime; set => endTime = value; }      

        //--------------------------------------------------------------------------------------------------
        // Constructors
        //--------------------------------------------------------------------------------------------------

        // Full constructor
        public ParkingArrangement(AppUser buyer, ParkingLot reservedAt, DateTime startTime, DateTime endTime)
        {
            Buyer = buyer;
            ReservedAt = reservedAt;
            StartTime = startTime;
            EndTime = endTime;
        }

        // For parking lots.
        public ParkingArrangement(AppUser buyer, DateTime startTime, DateTime endTime)
        {
            Buyer = buyer;
            StartTime = startTime;
            EndTime = endTime;
        }

        // For users.
        public ParkingArrangement(ParkingLot reservedAt, DateTime startTime, DateTime endTime)
        {
            ReservedAt = reservedAt;
            StartTime = startTime;
            EndTime = endTime;
        }

        //--------------------------------------------------------------------------------------------------
        // Methods 
        //--------------------------------------------------------------------------------------------------

        // Returns the list of parking arrangements for given user and time period.
        public static List<ParkingArrangement> Get(int buyer_id, DataServices.Period datePeriod)
        {
            return DataServices.GetParkingArrangements(buyer_id, datePeriod);
        }

    }
}