$(document).ready(function () {
    $('.dropdownIcon').click(function () {
        var clickedContainer = $(this).closest('.catDue--container');
        var catList = clickedContainer.find('.catList');

        if (!catList.length) {
            console.error('catList not found in the clicked container');
            return;
        }

        // Collapse any expanded container
        $('.catDue--container.expanded').not(clickedContainer).each(function () {
            var otherContainer = $(this);
            otherContainer.removeClass('expanded');
            otherContainer.find('.catList').slideUp();
            otherContainer.css('max-height', '80px'); // Reset to initial height

            // Reset the positions of the containers
            $('.body--buttons--container').css('top', '490px');
            $('.body--income--container').css('top', '560px');
        });

        // Toggle the clicked container
        clickedContainer.toggleClass('expanded');
        catList.slideToggle();

        // Adjust the position of the next containers
        var isExpanded = clickedContainer.hasClass('expanded');
        if (isExpanded) {
            clickedContainer.css('max-height', clickedContainer.get(0).scrollHeight + 'px');
            $('.body--buttons--container').css('top', '690px');
            $('.body--income--container').css('top', '800px');
        } else {
            clickedContainer.css('max-height', '80px');
            $('.body--buttons--container').css('top', '440px');
            $('.body--income--container').css('top', '560px');
        }
    });
});
