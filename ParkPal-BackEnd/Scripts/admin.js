
// ---------------------------------------- Constroller functions--------------------------

$(document).ready(function () {

    renderText();
    getAuction();

});

// ---------------------------------------------- Get --------------------------------------------------

// Get users list for admin use.
function getAuction() {
    let api = "../api/Auctions/auctioncampaigns";

    ajaxCall("GET", api, "", getAuctionSuccessCB, getErrorCB);
}

function getAuctionSuccessCB(auction) {
    console.log(auction);
    renderAuction(auction);
}

// Error function for getters, returns swal message.
function getErrorCB(err) {
    console.log(err.status + " " + err.responseJSON.Message);
    if (err.status == '404')
        swal("Error!", "404: " + err.responseJSON.Message, "error");
    else
        swal("Error!", err.responseJSON.Message, "error");
}

// ---------------------------------------------- Post -------------------------------------------------


function postAuction() {

    let newBid = {
        User_Name: $('#userName').val(),
        Bid: $('#bid').val(),
        Max_Bid: $('#maxBid').val(),
    }

    let api = "../api/Auction";
    ajaxCall("POST", api, JSON.stringify(newBid), postAuctionSuccessCB, postAuctionErrorCB);
}

function postAuctionSuccessCB(msg) {
    getAuction();
}

function postAuctionErrorCB(err) {
    console.log(err.status + " " + err.responseJSON.Message);
    swal("Error!", err.responseJSON.Message, "error");
}


// ---------------------------------------------- List Renders ----------------------------------------------

function renderAuction(auction) {
    renderBidders(auction.Bidders);
    renderSellers(auction.Sellers);
    renderLeaders(auction.Auctions);
}

function renderBidders(bidders) {

    $().html(
        '<thead>' +
        '<tr>' +
        '<th>UserName</th>' +
        '<th>BidLimit</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        '</tbody>' +
        '<tfoot>' +
        '<tr>' +
        '<th>UserName</th>' +
        '<th>BidLimit</th>' +
        '</tr>' +
        '</tfoot>'
    );

    try {
        tbl = $('#biddersTable').DataTable({
            data: bidders,
            pageLength: 10,
            columns: [
                { data: "UserName" },
                { data: "BidLimit" }
            ],
        });
    }
    catch (err) {
        alert(err);
    }
}

function renderSellers(sellers) {

    $().html(
        '<thead>' +
        '<tr>' +
        '<th>UserName</th>' +
        '<th>MinSellingPrice</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        '</tbody>' +
        '<tfoot>' +
        '<tr>' +
        '<th>UserName</th>' +
        '<th>MinSellingPrice</th>' +
        '</tr>' +
        '</tfoot>'
    );

    try {
        tbl = $('#sellersTable').DataTable({
            data: sellers,
            pageLength: 10,
            columns: [
                { data: "UserName" },
                { data: "MinSellingPrice" }
            ],
        });
    }
    catch (err) {
        alert(err);
    }
}

function renderLeaders(auctions) {

    $().html(
        '<thead>' +
        '<tr>' +
        '<th>Seller Name</th>' +
        '<th>Highest Bidder</th>' +
        '<th>CurrentBid</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        '</tbody>' +
        '<tfoot>' +
        '<tr>' +
        '<th>Seller Name</th>' +
        '<th>Highest Bidder</th>' +
        '<th>CurrentBid</th>' +
        '</tr>' +
        '</tfoot>'
    );

    try {
        tbl = $('#LeadingRecords').DataTable({
            data: auctions,
            pageLength: 10,
            columns: [
                { data: "Seller.UserName" },
                { data: "HighestBidder.UserName" },
                { data: "CurrBid" }
            ],
        });
    }
    catch (err) {
        alert(err);
    }
}

// ---------------------------------------------- Dynamic text input ----------------------------------------------

function renderText() {

    var MainHeading = 'Auction Data Base';
    var MainText = 'All the bidding for this parking lot.';
    $('#MainHeading').html(MainHeading);
    $('#MainText').html(MainText);
 
}
