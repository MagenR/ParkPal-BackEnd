
window.addEventListener('DOMContentLoaded', event => {

    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        // Uncomment Below to persist sidebar toggle between refreshes
        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
        //     document.body.classList.toggle('sb-sidenav-toggled');
        // }
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }

});

// ---------------------------------------- Constroller functions--------------------------

$(document).ready(function () {

    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    var urlParam = 'auction'; // Set Default param to users.
    urlParam = urlParams.get('category');
    renderText(urlParam);

});

// ---------------------------------------------- Getters ----------------------------------------------

// Get users list for admin use.
function getAuction() {
    let api = "../api/Auction";

    ajaxCall("GET", api, "", getAuctionSuccessCB, getErrorCB);
}

function getAuctionSuccessCB(auction) {
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

// ---------------------------------------------- List Renders ----------------------------------------------

function renderAuction(auction) {

    $('#dataTableInsert').html(
        '<thead>' +
        '<tr>' +
        '<th>User Name</th>' +
        '<th>Bid</th>' +
        '<th>Max Bid</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        '</tbody>' +
        '<tfoot>' +
        '<tr>' +
        '<th>User Name</th>' +
        '<th>Bid</th>' +
        '<th>Max Bid</th>' +
        '</tr>' +
        '</tfoot>'
    );

    try {
        tbl = $('#dataTableInsert').DataTable({
            data: auction,
            pageLength: 10,
            columns: [
                { data: "User Name" },
                { data: "Bid" },
                { data: "Max Bid" },
            ],
        });
    }
    catch (err) {
        alert(err);
    }
}

// ---------------------------------------------- Dynamic text input ----------------------------------------------

function renderText(choice) {

    var MainHeading;
    var MainText;
    var TableName;

    switch (choice) {
        case 'addBuyer':
            MainHeading = 'Episodes Data Base';
            MainText = 'All favourited episodes by series data and their count.';
            TableName = 'Episodes';
            getEpisodes();
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
