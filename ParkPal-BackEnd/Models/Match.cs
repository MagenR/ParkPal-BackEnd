using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkPal_BackEnd.Models
{
    internal class Match
    {
        Seller seller;
        Bidder buyers;
        int finalPrice;

        public Match(Seller seller, Bidder buyers)
        {
            Seller = seller;
            Buyers = buyers;
        }

        internal Seller Seller { get => seller; set => seller = value; }
        internal Bidder Buyers { get => buyers; set => buyers = value; }
    }
}
