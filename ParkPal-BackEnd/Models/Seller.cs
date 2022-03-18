using System;
using System.Collections.Generic;
using System.Text;

namespace ParkPal_BackEnd.Models
{
    class Seller : AppUser, IComparable
    {
        // Fields
        int minSellingPrice;
        //Auction owningAuction;

        // Props
        public int MinSellingPrice { get => minSellingPrice; set => minSellingPrice = value; }

        // Constructors
        public Seller(int minSellingPrice, string username) : base(username)
        {
            MinSellingPrice = minSellingPrice;
        }

        // Methods --------------------------------------------------------------------------------

        // Sort the bidders by bid limit desc.
        public int CompareTo(object obj) // Sort Asc.
        {
            if (obj is Seller)
            {
                Seller s = (Seller)obj;
                return minSellingPrice.CompareTo(s.minSellingPrice);
            }
            else
                throw new ArgumentException("Object is not of type Seller.");
        }

    } // End of class - Seller.

} // End of nameSpace - ParkPal_BackEnd.Models.