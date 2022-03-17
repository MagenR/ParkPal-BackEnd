using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkPal_BackEnd.Models
{
    internal class AuctionCampaign
    {
        // Fields
        List<Auction> auctions;
        List<Bidder> bidders;
        List<Seller> sellers;

        // Constructors
        public AuctionCampaign(List<Auction> auctions, List<Bidder> bidders, List<Seller> sellers)
        {
            Auctions = auctions;
            Bidders = bidders;
            Sellers = sellers;
        }

        public AuctionCampaign(List<Bidder> bidders, List<Seller> sellers)
        {
            Bidders = bidders;
            Sellers = sellers;
            Auctions = initAuctions(sellers);
        }

        public AuctionCampaign(List<Seller> sellers) 
        {
            Auctions = initAuctions(sellers);
        }

        public AuctionCampaign() { }

        // Props
        internal List<Auction> Auctions { get => auctions; set => auctions = value; }
        internal List<Bidder> Bidders { get => bidders; set => bidders = value; }
        internal List<Seller> Sellers { get => sellers; set => sellers = value; }

        // Methods --------------------------------------------------------------------------------
        public void run()
        {
            autoBid(Auctions, Bidders);
        }


        // Initialize auctions.
        public List<Auction> initAuctions(List<Seller> sellers)
        {
            List<Auction> auctions = new List<Auction>();
            foreach (Seller seller in sellers)
            {
                auctions.Add(new Auction(seller.MinSellingPrice, seller));
            }
                
            return auctions;
        }

        // Place a bid on the lowest auction.
        public void placeNewBid(Auction auction, Bidder bidder)
        {
            if(bidder.BidLimit > auction.CurrBid)
            {
                if(auction.HighestBidder != null)
                {
                    auction.CurrBid++;
                }
                Console.WriteLine(bidder.FirstName + " " + bidder.LastName + " bid on " + auction.Seller.FirstName + " " + auction.Seller.LastName + "'s Auction " + auction.CurrBid + " money.");
                auction.HighestBidder = bidder;

            }

        }

        // run over bidders list and auto increment if possible.
        public void autoBid(List<Auction> auctions, List<Bidder> bidders)
        {
            bool noBidMade = false;
            while (!noBidMade)
            {
                foreach (Bidder bidder in bidders)
                {
                    if(outbid(bidder, auctions))
                        placeNewBid(findLowestAuction(auctions), bidder);
                }
            }

        }

        // Checks if user was outbid in campagin.
        public bool outbid(Bidder bidder, List<Auction> auctions)
        {
            foreach (Auction auction in auctions)
                if (auction.HighestBidder == bidder)
                    return false;
            return true;
        }

        // Find auction with lowest bid.
        public Auction findLowestAuction(List<Auction> auctions)
        {
            Auction lowestAuc = auctions.First();
            int lowestBid = lowestAuc.CurrBid;
            foreach (var auction in Auctions)
            {
                if (auction.CurrBid < lowestBid || (auction.CurrBid == lowestBid && auction.HighestBidder == null))
                {
                    lowestAuc = auction;
                    lowestBid = auction.CurrBid;
                }
            }
            return lowestAuc;
        }

    }
}
