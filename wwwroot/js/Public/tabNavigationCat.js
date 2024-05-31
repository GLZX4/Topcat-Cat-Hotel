$(document).ready(function () {
    var errorMsg = $(".codeErrorMsg").text().trim();
    if (errorMsg.length > 0) {
        $(".codeErrorMsg").addClass("error");
    }
    // Function to open cat details tab
    window.openCatDetails = function (evt, catName) {
        var i, tabcontent, tablinks;

        // Hide all tabcontent by default
        tabcontent = $(".tabcontent");
        tabcontent.hide();

        // Remove the active class from all tablinks
        tablinks = $(".tablinks");
        tablinks.removeClass("active");

        // Show the specific tab content
        $("#" + catName).show();

        // Add an "active" class to the button that opened the tab
        $(evt.currentTarget).addClass("active");
    };

    // Attach click event to dynamically created tablinks
    $(document).on("click", ".tablinks", function (event) {
        var catName = $(this).data("catname");
        openCatDetails(event, catName);
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

    // Initially open the first tab if there are any tabs
    if ($(".tablinks").length > 0) {
        $(".tablinks:first").click();
    }
});
