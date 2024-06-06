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

    const $startDateInput = $("input[name='registerCat.StartDate']");
    const $endDateInput = $("input[name='registerCat.EndDate']");

    // Event listener for start date changes
    $startDateInput.on("change", function () {
        const startDateValue = $(this).val();
        const startDate = new Date(startDateValue);
        startDate.setDate(startDate.getDate() + 1);
        const minEndDate = startDate.toISOString().split("T")[0];
        $endDateInput.attr("min", minEndDate);
        const endDateValue = $endDateInput.val();
        if (new Date(endDateValue) <= new Date(startDateValue)) {
            $endDateInput.val(minEndDate);
        }
    });

    // Initialize min value for end date based on the initial start date value
    const initialStartDateValue = $startDateInput.val();
    const initialStartDate = new Date(initialStartDateValue);
    initialStartDate.setDate(initialStartDate.getDate() + 1);
    const initialMinEndDate = initialStartDate.toISOString().split("T")[0];

    $endDateInput.attr("min", initialMinEndDate);
});