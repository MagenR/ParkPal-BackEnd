using System;
using System.Collections.Generic;
using System.Text;

namespace ParkPal_BackEnd.Models
{
    class Seller : AppUser, IComparable
    {
        // Fields
        int minSellingPrice;
        Auction owningAuction;

        // Props
        public int MinSellingPrice { get => minSellingPrice; set => minSellingPrice = value; }

        // Constructors
        public Seller(int minSellingPrice, string firstName, string lastName) : base( firstName,  lastName)
        {
            MinSellingPrice = minSellingPrice;
        }

        // Methods

        // Sort the bidders by bid limit desc.
        public int CompareTo(object obj) //עבור המיון מהקטן לגדול
        {
            if (obj is Seller)
            {
                Seller s = (Seller)obj;
                return minSellingPrice.CompareTo(s.minSellingPrice);
            }
            else
                throw new ArgumentException("Object is not of type Seller.");
        }

    }
}
