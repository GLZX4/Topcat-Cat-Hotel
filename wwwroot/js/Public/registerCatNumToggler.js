$(document).ready(function () {
    const $numOfCatsInput = $("#NumOfCats");
    const $catNameGroups = $(".catNameGroup");

    // Hide all cat name groups initially
    $catNameGroups.hide();

    $numOfCatsInput.on("input", function () {
        const numOfCats = parseInt($numOfCatsInput.val());
        $catNameGroups.each(function (index) {
            if (index < numOfCats) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });

    // Function to toggle visibility of related input fields
    function toggleRelatedInput(checkbox, relatedInput) {
        if ($(checkbox).is(":checked")) {
            $(relatedInput).slideDown();
        } else {
            $(relatedInput).slideUp();
        }
    }

    // Initially hide related input fields if the checkboxes are not checked
    $(".microchippedCheckbox").each(function () {
        toggleRelatedInput(this, ".microchipNumberInput");
    });

    $(".insuredCheckbox").each(function () {
        toggleRelatedInput(this, ".insuranceDetailsInput");
    });

    // Attach change event handlers to the checkboxes
    $(document).on("change", ".microchippedCheckbox", function () {
        toggleRelatedInput(this, ".microchipNumberInput");
    });

    $(document).on("change", ".insuredCheckbox", function () {
        toggleRelatedInput(this, ".insuranceDetailsInput");
    });

    $('.aboutIcon').on('click', function (event) {
        event.preventDefault();
        alert('By checking this box, I give my consent to for my cat to be treated by either my own Vet or the catterys vet in case of emergency or illness.');
    });
});
