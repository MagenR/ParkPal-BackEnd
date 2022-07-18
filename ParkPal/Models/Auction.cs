﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkPal_BackEnd.Models.DAL;

namespace ParkPal_BackEnd.Models
{
    public class Auction : DataBaseObject
    {
        // ----------------------------------------------------------------------------------------
        // Fields
        // ----------------------------------------------------------------------------------------

        // Seller seller;
        Bidder highestBidder;
        ParkingArrangement soldArrangement;
        int startingPrice;
        int? currBid;

        // ----------------------------------------------------------------------------------------
        // Props
        // ----------------------------------------------------------------------------------------

        // public Seller Seller { get => seller; set => seller = value; }
        public Bidder HighestBidder { get => highestBidder; set => highestBidder = value; }
        public ParkingArrangement SoldArrangement { get => soldArrangement; set => soldArrangement = value; }
        public int? CurrBid { get => currBid; set => currBid = value; }
        public int StartingPrice { get => startingPrice; set => startingPrice = value; }

        // ----------------------------------------------------------------------------------------
        // Constructors
        // ----------------------------------------------------------------------------------------
       
        public Auction(Bidder highestBidder, ParkingArrangement soldArrangement, int startingPrice, int? currBid)
        {
            HighestBidder = highestBidder;
            SoldArrangement = soldArrangement;
            StartingPrice = startingPrice;
            CurrBid = currBid;
        }

        public Auction() { }

        //--------------------------------------------------------------------------------------------------
        // Methods 
        //--------------------------------------------------------------------------------------------------

        // Inserts the arrangement, gets it's id, inserts auction.
        public override int Insert()
        {
            soldArrangement.Id = soldArrangement.Insert();
            if (soldArrangement.Id == 0)
                throw new Exception("Could not insert the arrangement to the DB.");
            return base.Insert();
        }

        // Get all auctions for a given time period.
        public static List<Auction> Get(int parkingLotId, DateTime startTime, DateTime endTime)
        {
            return DataServices.GetParkingArrangementsAuctions(parkingLotId, startTime, endTime);
        }

    } // End of class - Auction.

} // End of nameSpace - ParkPal_BackEnd.Models.