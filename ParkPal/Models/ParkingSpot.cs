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
        
        // Full constructor
        public ParkingSpot(int Number, ParkingLot parentlot, List<ParkingArrangement> arrangements)
        {
            number = Number;
            ParentLot = parentlot;
            Arrangements = arrangements;
        }

        public ParkingSpot() { }

        //--------------------------------------------------------------------------------------------------
        // Methods 
        //--------------------------------------------------------------------------------------------------

    }
}