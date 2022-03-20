
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
    //getAuction();

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
        tbl = $().DataTable({
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
            TableName = 'Auction';
            showModal();
            break;
        case 'auction':
        default:
            MainHeading = 'Auction Data Base';
            MainText = 'All the bidding for this parking lot.';
            TableName = 'Auction';
            getAuction();
    }

    $('#MainHeading').html(MainHeading);
    $('#MainText').html(MainText);
    $('#TableName').html('<i class="fas fa-table me-1"></i>' + TableName);
}
