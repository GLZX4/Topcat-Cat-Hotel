$(document).ready(function () {
    // Get the current URL path
    var path = window.location.pathname;

    // Iterate through each nav button
    var isActiveSet = false;
    $('.navBtn').each(function () {
        var buttonLink = $(this).closest('a').attr('href');

        // Check if the button link matches the current path
        if (path === buttonLink) {
            $(this).addClass('active');
            isActiveSet = true;
        }
    });

    // Default to the Home button if no match is found
    if (!isActiveSet) {
        $('.navBtn').first().addClass('active');
    }

    // Show the nav buttons after the active class is set
    $('.header--navButtons--container').addClass('visible');
});