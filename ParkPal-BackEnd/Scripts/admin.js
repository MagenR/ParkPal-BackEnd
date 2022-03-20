
// ---------------------------------------- Constroller functions--------------------------

$(document).ready(function () {
    $('#AddSellerBtn').click(addSeller);
    $('#AddBidderBtn').click(addBidder);
    $('#UpdateBtn').click(update);
    $('#AddSBtn').click(postSeller);
    $('#AddBBtn').click(postBidder);
    $('#Update').click(updateBidderBid);
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


function postSeller() {

    let newBid = {
        UserName: $('#UserNameSeller').val(),
        minSellingPrice: $('#minPrice').val(),
    }

    let api = "../api/Auctions/postacseller";
    ajaxCall("POST", api, JSON.stringify(newBid), postSellerSuccessCB, postSellerErrorCB);
}

function postSellerSuccessCB(msg) {
    getAuction();
}

function postSellerErrorCB(err) {
    console.log(err.status + " " + err.responseJSON.Message);
    swal("Error!", err.responseJSON.Message, "error");
}

function postBidder() {

    let newBid = {
        UserName: $('#UserNameBidder').val(),
        BidLimit: $('#maxPrice').val(),
    }

    let api = "../api/Auctions/postacbidder";
    ajaxCall("POST", api, JSON.stringify(newBid), postBidderSuccessCB, postBidderErrorCB);
}

function postBidderSuccessCB(msg) {
    getAuction();
}

function postBidderErrorCB(err) {
    console.log(err.status + " " + err.responseJSON.Message);
    swal("Error!", err.responseJSON.Message, "error");
}

function updateBidderBid() {
    let updateBid = {
        username: $('#UserNameBidderUpdate').val(),
        BidLimit: $('#newPrice').val(),
    }
    let api = "../api/Auctions/putacbidder";
    ajaxCall("PUT", api, JSON.stringify(updateBid), putBidderSuccessCB, putBidderErrorCB);
}

function putBidderSuccessCB(msg) {
    getAuction();
}

function putBidderErrorCB(err) {
    console.log(err.status + " " + err.responseJSON.Message);
    swal("Error!", err.responseJSON.Message, "error");
}

// ---------------------------------------------- List Renders ----------------------------------------------


function renderAuction(auction) {
    renderBidders(auction.Bidders);
    renderSellers(auction.Sellers);
       (auction.Auctions);
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
// ---------------------------------------------- Insert buttons ----------------------------------------------

function addSeller() {
    $('#AddSeller').modal('show');
}

function addBidder() {
    $('#AddBidder').modal('show');
}

function update() {
    $('#UpdateModal').modal('show');
}

