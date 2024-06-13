$(document).ready(function () {
    $('.expandIcon').click(function () {
        // Toggle the visibility of the expandedDetails section
        $(this).closest('.approveContainer').find('.expandedDetails').slideToggle();
        $(this).toggleClass('expanded');
    });

    if ($('.validation-summary-errors').children().length > 0) {
        $('.validation-summary-errors').fadeIn().delay(4000).fadeOut();
    }
});