$(document).ready(function () {
    const maxCats = 3;
    const customMessage = `You can only enter a maximum of 3 Cats per Booking`;

    function validateCatNumber(input) {
        const value = parseInt(input.val());
        if (value > maxCats) {
            input[0].setCustomValidity(customMessage);
        } else {
            input[0].setCustomValidity('');
        }
    }

    $('#NumOfCats').on('input', function () {
        validateCatNumber($(this));
    });

    $('form').on('submit', function (event) {
        validateCatNumber($('#NumOfCats'));
        if (!this.checkValidity()) {
            event.preventDefault();
        }
    });
});