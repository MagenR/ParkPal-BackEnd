using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ParkPal_BackEnd.Models.DAL;

namespace ParkPal_BackEnd.Models
{
    public class ParkingSpot : DataBaseObject
    {
        //--------------------------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------------------------
        
        int number;
        ParkingLot parentLot;
        List<ParkingArrangement> arrangements;

        //--------------------------------------------------------------------------------------------------
        // Props
        //--------------------------------------------------------------------------------------------------
       
        public int Number { get => number; set => number = value; }
        public ParkingLot ParentLot { get => parentLot; set => parentLot = value; }
        public List<ParkingArrangement> Arrangements { get => arrangements; set => arrangements = value; }

        //--------------------------------------------------------------------------------------------------
        // Constructors
        //--------------------------------------------------------------------------------------------------
        
        // for user's GETs
        public ParkingSpot(int Number, ParkingLot parentlot)
        {
            number = Number;
            ParentLot = parentlot;
        }

        public ParkingSpot() { }

        //--------------------------------------------------------------------------------------------------
        // Methods 
        //--------------------------------------------------------------------------------------------------

    }
}