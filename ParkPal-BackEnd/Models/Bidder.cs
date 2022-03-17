using System;
using System.Collections.Generic;
using System.Text;

namespace ParkPal_BackEnd.Models
{
    class Bidder : AppUser, IComparable
    {
        // Fields
        int bidLimit;
        List<Auction> biddingAuctions;
        Auction currentLead;

        // Props
        public int BidLimit { get => bidLimit; set => bidLimit = value; }

        // Constructors
        public Bidder(int bidLimit, string firstName, string lastName) : base(firstName, lastName)
        {
            BidLimit = bidLimit;
        }

        // Methods

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

    }
}
