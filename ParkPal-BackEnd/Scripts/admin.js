
// ---------------------------------------- Constroller functions--------------------------

$(document).ready(function () {

    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    var urlParam = 'addBuyer'; // Set Default param to auction.
    urlParam = urlParams.get('category');
    renderText(urlParam);
    //$('#makeAbidModal').on('click', '#MakeAbidBtn', function () {
    //    postAuction();
    //});
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
        User_Name: $('#username').val(),
        Bid: $('#currentBid').val(),
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
/*    renderAuction(auction.auction);*/

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

//function renderAuction(auctions) {

//    $().html(
//        '<thead>' +
//        '<tr>' +
//        '<th>currBid</th>' +
//        '</tr>' +
//        '</thead>' +
//        '<tbody>' +
//        '</tbody>' +
//        '<tfoot>' +
//        '<tr>' +
//        '<th>currBid</th>' +
//        '</tr>' +
//        '</tfoot>'
//    );

//    try {
//        tbl = $('#auctionsTable').DataTable({
//            data: auctions,
//            pageLength: 10,
//            columns: [
//                { data: "currBid" },
//            ],
//        });
//    }
//    catch (err) {
//        alert(err);
//    }
//}

function showModal() {
    $('#makeAbidModal').modal('show');
}

// ---------------------------------------------- Dynamic text input ----------------------------------------------

function renderText(choice) {

    var MainHeading;
    var MainText;
    var TableName;

    switch (choice) {
        case 'addBuyer':
            MainHeading = 'Auction Data Base';
            MainText = 'All the bidding for this parking lot.';
            biddersTableName = 'Bidders';
            sellersTableName = 'Sellers';
            auctionTableName = 'Auctions';
            showModal();
            break;
        case 'auction':
        default:
            MainHeading = 'Auction Data Base';
            MainText = 'All the bidding for this parking lot.';
            biddersTableName = 'Bidders';
            sellersTableName = 'Sellers';
            auctionTableName = 'Auctions';
            getAuction();
    }

    $('#MainHeading').html(MainHeading);
    $('#MainText').html(MainText);
    $('#biddersTableName').html('<i class="fas fa-table me-1"></i>' + biddersTableName);
    $('#sellersTableName').html('<i class="fas fa-table me-1"></i>' + sellersTableName);
    $('#auctionTableName').html('<i class="fas fa-table me-1"></i>' + auctionTableName);
}
