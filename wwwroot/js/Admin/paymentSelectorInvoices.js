$(document).ready(function () {

    $(".expandedContent").hide();

    $('#cashButton').click(function () {
        $('#paymentMethod').val('Cash');
        $('#cashButton').toggleClass("selected");
    });

    $('#cardButton').click(function () {
        $('#paymentMethod').val('Card');
        $('#cardButton').toggleClass("selected");
    });

    $(".expandIcon").click(function () {
        var invoiceContainer = $(this).closest(".invoice--container");
        var expandedContent = invoiceContainer.find(".expandedContent");

        // Toggle the enabled class for rotation animation
        invoiceContainer.toggleClass("enabled");

        // Slide toggle the expanded content
        expandedContent.stop(true, true).slideToggle(300);
    });
});