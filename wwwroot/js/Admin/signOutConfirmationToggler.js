$(document).ready(function () {
    $('.signOutBtn').click(function () {
        $(this).siblings('.signOut--confirmation').slideToggle();
    });

    $('#decline').click(function () {
        $(this).closest('.signOut--confirmation').slideUp();
    });
});