$(document).ready(function () {
    $('#cashButton').click(function () {
        $('#paymentMethod').val('Cash');
    });

    $('#cardButton').click(function () {
        $('#paymentMethod').val('Card');
    });
});