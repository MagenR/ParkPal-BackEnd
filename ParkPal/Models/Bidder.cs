using System;
using System.Collections.Generic;
using System.Text;

namespace ParkPal_BackEnd.Models
{
    public class Bidder : AppUser, IComparable
    {
        // ----------------------------------------------------------------------------------------
        // Fields
        // ----------------------------------------------------------------------------------------

        int bidLimit;
        int? currentBid;
        DateTime bidTime, forStartTime, forEndTime;
        Auction currentLead;
        ParkingLot biddedLot;

        // ----------------------------------------------------------------------------------------
        // Props
        // ----------------------------------------------------------------------------------------

        public int BidLimit { get => bidLimit; set => bidLimit = value; }
        public int? CurrentBid { get => currentBid; set => currentBid = value; }
        public Auction CurrentLead { get => currentLead; set => currentLead = value; }
        public ParkingLot BiddedLot { get => biddedLot; set => biddedLot = value; }
        public DateTime BidTime { get => bidTime; set => bidTime = value; }
        public DateTime ForStartTime { get => forStartTime; set => forStartTime = value; }
        public DateTime ForEndTime { get => forEndTime; set => forEndTime = value; }

        // ----------------------------------------------------------------------------------------
        // Constructors
        // ----------------------------------------------------------------------------------------

        // Full 
        public Bidder(int id, string username, int bidLimit, ParkingLot biddedLot, DateTime for_start_time, DateTime for_end_time) : base(id, username)
        {
            BidLimit = bidLimit;
            BiddedLot = biddedLot;
            ForStartTime = for_start_time;
            ForEndTime = for_end_time;
        }

        public Bidder(int id, string username, int bidLimit) : base(id, username)
        {
            BidLimit = bidLimit;
        }

        public Bidder(int id, int bidLimit, DateTime bidTime) : base(id)
        {
            BidLimit = bidLimit;
            BidTime = bidTime;
        }

        public Bidder() { }

        // ----------------------------------------------------------------------------------------
        // Methods
        // ----------------------------------------------------------------------------------------

        // Sort the bidders by bid limit desc.
        public int CompareTo(object obj)
        {
            if (obj is Bidder)
            {
                Bidder b = (Bidder)obj;
                return BidLimit.CompareTo(b.BidLimit);
            }
            else
                throw new ArgumentException("Object is not of type Bidder.");
        }

        // Add bidder to DB and run the algorithm.
        public override int Insert()
        {
            if (base.Insert() == 1)
            {
                AuctionCampaign ac = new AuctionCampaign(BiddedLot.Id, ForStartTime, ForEndTime);
                ac.runAuctionCompute();
            }             
            return 1;
        }

        // Update bidder and run the alogrithm.
        public override int Update()
        {
            if(base.Update() == 1)
            {
                AuctionCampaign ac = new AuctionCampaign(BiddedLot.Id, ForStartTime, ForEndTime);
                ac.runAuctionCompute();
            }
            return 1;
        }

    } // End of class - Bidder.

} // End of nameSpace - ParkPal_BackEnd.Models.