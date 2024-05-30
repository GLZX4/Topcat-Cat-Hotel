$(document).ready(function () {
    $(".TopcatBtn.services").click(function () {
        $('html, body').animate({
            scrollTop: $(".body--ourServices--container").offset().top
        }, 2000);
    });
});