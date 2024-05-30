$(document).ready(function () {
    const date = new Date();
    const daysContainer = $('#days');
    const monthYearDisplay = $('#month-year');

    function renderCalendar() {
        date.setDate(1);
        const month = date.getMonth();
        const year = date.getFullYear();

        const firstDayIndex = date.getDay();
        const lastDay = new Date(year, month + 1, 0).getDate();
        const prevLastDay = new Date(year, month, 0).getDate();
        const lastDayIndex = new Date(year, month + 1, 0).getDay();
        const nextDays = 7 - lastDayIndex - 1;

        const months = [
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December",
        ];

        monthYearDisplay.text(`${months[month]} ${year}`);

        let days = "";

        for (let x = firstDayIndex; x > 0; x--) {
            days += `<div class="prev-date">${prevLastDay - x + 1}</div>`;
        }

        for (let i = 1; i <= lastDay; i++) {
            const fullDate = new Date(year, month, i).toISOString().split('T')[0];
            const isAvailable = availability[fullDate] !== undefined ? availability[fullDate] : true;
            const className = isAvailable ? "available" : "unavailable";

            days += `<div class="${className}" id="day-${i}">${i}</div>`;
        }

        for (let j = 1; j <= nextDays; j++) {
            days += `<div class="next-date">${j}</div>`;
        }

        daysContainer.html(days);
    }

    function navigateMonth(direction) {
        if (direction === 'prev') {
            date.setMonth(date.getMonth() - 1);
        } else if (direction === 'next') {
            date.setMonth(date.getMonth() + 1);
        }
        renderCalendar();
    }

    $('#prev').on('click', function () {
        navigateMonth('prev');
    });

    $('#next').on('click', function () {
        navigateMonth('next');
    });

    renderCalendar();
});
