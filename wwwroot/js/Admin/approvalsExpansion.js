$(document).ready(function () {
    $('.expandIcon').click(function () {
        // Toggle the visibility of the expandedDetails section
        $(this).closest('.approveContainer').find('.expandedDetails').slideToggle();
        $(this).toggleClass('expanded');
    });
});