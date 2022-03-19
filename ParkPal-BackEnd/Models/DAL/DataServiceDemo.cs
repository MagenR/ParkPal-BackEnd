using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkPal_BackEnd.Models.DAL
{
    public class DataServiceDemo
    {
        enum insertType { Bidder, Seller }

        static public AuctionCampaign ac = new AuctionCampaign(
                new List<Bidder> {
                new Bidder(40, "Mahluf"),
                new Bidder(27, "Alfonso"),
                new Bidder(23, "Suren"),
                new Bidder(25, "Nofar")
                },

                new List<Seller> {
                new Seller(15, "Sean"),
                new Seller(18, "Gerry")
                });
    }

\]==========8hj