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
        int currentBid;
        Auction currentLead;

        // ----------------------------------------------------------------------------------------
        // Props
        // ----------------------------------------------------------------------------------------

        public int BidLimit { get => bidLimit; set => bidLimit = value; }
        public int CurrentBid { get => currentBid; set => currentBid = value; }
        public Auction CurrentLead { get => currentLead; set => currentLead = value; }

        // ----------------------------------------------------------------------------------------
        // Constructors
        // ----------------------------------------------------------------------------------------

        public Bidder(int bidLimit, string username) : base(username)
        {
            BidLimit = bidLimit;
        }

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

    } // End of class - Bidder.

} // End of nameSpace - ParkPal_BackEnd.Models.