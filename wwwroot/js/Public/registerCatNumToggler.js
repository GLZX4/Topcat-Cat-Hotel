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
            $(relatedInput).show();
        } else {
            $(relatedInput).hide();
        }
    }

    // Initially hide related input fields if the checkboxes are not checked
    $(".microchippedCheckbox").each(function () {
        const relatedInput = $(this).closest(".catNameGroup").find(".microchipNumberInput");
        toggleRelatedInput(this, relatedInput);
    });

    $(".insuredCheckbox").each(function () {
        const relatedInput = $(this).closest(".catNameGroup").find(".insuranceDetailsInput");
        toggleRelatedInput(this, relatedInput);
    });

    // Attach change event handlers to the checkboxes
    $(document).on("change", ".microchippedCheckbox", function () {
        const relatedInput = $(this).closest(".catNameGroup").find(".microchipNumberInput");
        toggleRelatedInput(this, relatedInput);
    });

    $(document).on("change", ".insuredCheckbox", function () {
        const relatedInput = $(this).closest(".catNameGroup").find(".insuranceDetailsInput");
        toggleRelatedInput(this, relatedInput);
    });
});
