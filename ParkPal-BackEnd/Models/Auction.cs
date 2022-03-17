using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkPal_BackEnd.Models
{
    internal class Auction
    {
        Seller seller;
        Bidder highestBidder;
        int currBid;

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
            Console.WriteLine("auction of " + seller.FirstName + seller.LastName + " was init with starting bid price of " + CurrBid);
        }

        public int CurrBid { get => currBid; set => currBid = value; }
        internal Seller Seller { get => seller; set => seller = value; }
        internal Bidder HighestBidder { get => highestBidder; set => highestBidder = value; }
    }


}
