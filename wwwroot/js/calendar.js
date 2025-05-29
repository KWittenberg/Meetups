window.initializeCalendar = (elementId, options) => {
    const calendarEl = document.getElementById(elementId);

    const calendar = new window.FullCalendar.Calendar(calendarEl, {
        themeSystem: 'bootstrap5',
        height: 'auto',
        initialView: options.initialView || 'dayGridMonth',
        events: Array.isArray(options.events) ? options.events : [],
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            /*right: 'dayGridMonth,timeGridWeek,timeGridDay'*/
            right: 'listMonth,dayGridMonth,timeGridWeek,timeGridDay'
        }
    });
    calendar.render();
    return calendar;
};