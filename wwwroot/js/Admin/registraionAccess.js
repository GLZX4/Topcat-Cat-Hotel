$(document).ready(function () {
    const secretButton = $('#secretButton');
    const registrationForm = $('.body--register--container');
    $(document).on('keydown', function (event) {
        if (event.ctrlKey && event.key === '0') { // Ctrl + R to toggle the form
            registrationForm.toggleClass('active');
        }
    });
});