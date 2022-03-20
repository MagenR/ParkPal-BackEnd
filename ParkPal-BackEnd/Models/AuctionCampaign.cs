using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkPal_BackEnd.Models.DAL;

namespace ParkPal_BackEnd.Models
{
    public class AuctionCampaign
    {
        // ----------------------------------------------------------------------------------------
        // Fields
        // ----------------------------------------------------------------------------------------

        List<Auction> auctions;
        List<Bidder> bidders;
        List<Seller> sellers;
        List<string> bidHistory;

        // ----------------------------------------------------------------------------------------
        // Props
        // ----------------------------------------------------------------------------------------

        public List<Auction> Auctions { get => auctions; set => auctions = value; }
        public List<Bidder> Bidders { get => bidders; set => bidders = value; }
        public List<Seller> Sellers { get => sellers; set => sellers = value; }
        public List<string> BidHistory { get => bidHistory; set => bidHistory = value; }

        // ----------------------------------------------------------------------------------------
        // Constructors
        // ----------------------------------------------------------------------------------------

        //public AuctionCampaign(List<Auction> auctions, List<Bidder> bidders, List<Seller> sellers)
        //{
        //    Auctions = auctions;
        //    Bidders = bidders;
        //    Sellers = sellers;
        //}

        public AuctionCampaign(List<Bidder> bidders, List<Seller> sellers)
        {
            Bidders = bidders;
            Sellers = sellers;
            BidHistory = new List<string>();
            Auctions = initAuctions(sellers);         
        }

        //public AuctionCampaign(List<Seller> sellers) 
        //{
        //    Auctions = initAuctions(sellers);
        //}

        //public AuctionCampaign() { }

        // ----------------------------------------------------------------------------------------
        // Methods
        // ----------------------------------------------------------------------------------------

        public void runAuctionCompute()
        {
            autoBid(Auctions, Bidders);
        }

        // Reinit after bidder and seller insert.
        public int reInit()
        {
            BidHistory.Clear();
            Auctions = initAuctions(Sellers); 
            return 1;
        }

        // Initialize auctions for this campaign with given seller.
        public List<Auction> initAuctions(List<Seller> sellers)
        {
            List<Auction> auctions = new List<Auction>();
            foreach (Seller seller in sellers)
            {
                auctions.Add(new Auction(seller.MinSellingPrice, seller));
                string h = seller.UserName + "'s auction was initialized with starting price: " + seller.MinSellingPrice;
                BidHistory.Add(h);
            }
               
            return auctions;
        }

        // Place a bid on a given auction.
        public bool placeNewBid(Auction auction, Bidder bidder)
        {
            if (bidder.BidLimit > auction.CurrBid)
            {
                if (auction.HighestBidder != null)
                    auction.CurrBid++;
                string h = bidder.UserName + " bid on " + auction.Seller.UserName + "'s Auction, " + auction.CurrBid + " money.";
                BidHistory.Add(h);
                auction.HighestBidder = bidder;
                //bidder.CurrentLead = auction;
                //bidder.CurrentBid = auction.CurrBid;
                return true;
            }
            return false;
        }

        // Find auction with lowest bid.
        public Auction findLowestAuction(List<Auction> auctions)
        {
            Auction lowestAuc = auctions.First();
            int lowestBid = lowestAuc.CurrBid;
            foreach (var auction in Auctions)
                if (auction.CurrBid < lowestBid || (auction.CurrBid == lowestBid && auction.HighestBidder == null))
                {
                    lowestAuc = auction;
                    lowestBid = auction.CurrBid;
                }
            return lowestAuc;
        }

        // Checks if user was outbid in campaign.
        public bool outbid(Bidder bidder, List<Auction> auctions)
        {
            foreach (Auction auction in auctions)
                if (auction.HighestBidder == bidder)
                    return false;
            return true;
        }

        // Run over bidders list and assign them for an auction if possible.
        public void autoBid(List<Auction> auctions, List<Bidder> bidders)
        {
            bool bidUpdated = true, bidWasPlaced = false;
            while (bidUpdated)
            {
                bidUpdated = false;
                foreach (Bidder bidder in bidders)
                {
                    if (outbid(bidder, auctions))
                    {
                        //bidder.CurrentLead = null;
                        bidWasPlaced = placeNewBid(findLowestAuction(auctions), bidder);
                        if (bidUpdated == false && bidWasPlaced == true)
                            bidUpdated = true;
                    }

                }
            }
        }

        // Returns this campaign upon controller request.
        public AuctionCampaign Get()
        {
            return this;
        }

        // Adds bidder/seller to the campaign
        public int Insert(object o)
        {
            if (o is Bidder)
                Bidders.Add((Bidder)o);
            else if (o is Seller)
            {
                Seller s = (Seller)o;
                Sellers.Add(s);
                auctions.Add(new Auction(s.MinSellingPrice, s));
            }
            reInit();
            return 1; // No reason to fail.
        }

        // Update bidder bids list by name - WORK IN PROGRESS!!!!
        public int UpdateBidderListBids(List<Bidder> bl)
        {
            foreach (Bidder bidder in bl)
            {
                Bidder currBidder = Bidders.First(testedBidder => testedBidder.UserName == bidder.UserName);
                currBidder.BidLimit = bidder.BidLimit;
            }
            return 1;
        }

        public int UpdateBidderBids(Bidder b)
        {
            Bidder foundBidder = Bidders.First(testedBidder => testedBidder.UserName == b.UserName);
            foundBidder.BidLimit = b.BidLimit;
            return 1;
        }


        public int Update(object o)
        {
            if (o is Bidder)
                return UpdateBidderBids((Bidder)o);
            return 1;
        }

    }  // End of class - AuctionCampaign.

} // End of nameSpace - ParkPal_BackEnd.Models.