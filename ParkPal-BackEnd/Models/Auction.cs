using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkPal_BackEnd.Models
{
    internal class Auction
    {
        // Fields
        Seller seller;
        Bidder highestBidder;
        int currBid;

        // Props
        public int CurrBid { get => currBid; set => currBid = value; }
        internal Seller Seller { get => seller; set => seller = value; }
        internal Bidder HighestBidder { get => highestBidder; set => highestBidder = value; }

        // Constructors
        public Auction(int currBid, Seller seller, Bidder highestBidder)
        {
            CurrBid = currBid;
            Seller = seller;
            HighestBidder = highestBidder;
        }

        public Auction(int currBid, Seller seller)
        {
            CurrBid = currBid;
            Seller = seller;
            //Console.WriteLine("auction of " + seller.UserName + " was init with starting bid price of " + CurrBid);
        }

    } // End of class - Auction.

} // End of nameSpace - ParkPal_BackEnd.Models.