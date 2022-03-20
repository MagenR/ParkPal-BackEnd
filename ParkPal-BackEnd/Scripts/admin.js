
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
// ---------------------------------------------- Insert buttons ----------------------------------------------
$('#AddSellerBtn').click(addSeller);

function addSeller() {
    $('#AddSeller').modal('show');
}

$('#AddSBtn').click(alertMsg);

$('#AddBidderBtn').click(addBidder);


function alertMsg(id) {
    alert("Added successfully");
    $("#AddSeller").modal('hide');
    renderSellers();
}

function addBidder() {
    $('#AddBidder').modal('show');
}

$('#UpdateBtn').click(update);

function update() {
    $('#Update').modal('show');
}

function postSeller() {

    let newSeller = {
        User_Name: $('#UserNameSeller').val(),
        Min_Price: $('#minPrice').val(),
    }

    let api = "../api/postacseller";
    ajaxCall("POST", api, JSON.stringify(newSeller), postSellerSuccessCB, postSellerErrorCB);
}

function postSellerSuccessCB(seller) {
    console.log(seller);
}

function postSellerErrorCB(err) {
    console.log(err.status + " " + err.responseJSON.Message);
    if (err.status == '404')
        swal("Error!", "404: " + err.responseJSON.Message, "error");
    else
        swal("Error!", err.responseJSON.Message, "error");
}