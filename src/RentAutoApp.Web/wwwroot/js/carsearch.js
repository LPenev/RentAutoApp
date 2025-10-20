(function () {
    const startDateInput = document.getElementById('StartDate');
    const endDateInput = document.getElementById('EndDate');
    if (!startDateInput || !endDateInput) return;

    // format and parse
    function fmt(d) {
        const y = d.getFullYear();
        const m = String(d.getMonth() + 1).padStart(2, '0');
        const day = String(d.getDate()).padStart(2, '0');
        return `${y}-${m}-${day}`;
    }
    function parseLocalDate(yyyyMMdd) {
        const [y, m, d] = yyyyMMdd.split('-').map(Number);
        return new Date(y, m - 1, d, 0, 0, 0, 0);
    }

    function updateEndDateMin(assignIfInvalid) {
        let minStr;

        if (startDateInput.value) {
            const start = parseLocalDate(startDateInput.value);
            const minEnd = new Date(start);
            minEnd.setDate(minEnd.getDate() + 1);
            minStr = fmt(minEnd);
        } else {
            const t = new Date(); t.setHours(0, 0, 0, 0);
            t.setDate(t.getDate() + 1);
            minStr = fmt(t);
        }

        endDateInput.min = minStr;

        // check EndDate valid is
        if (assignIfInvalid) {
            if (!endDateInput.value || endDateInput.value < minStr) {
                endDateInput.value = minStr;
            }
        } else {
            if (endDateInput.value && endDateInput.value < minStr) {
                endDateInput.value = minStr;
            }
        }
    }

    // Change value by StartDate Change
    startDateInput.addEventListener('change', () => updateEndDateMin(true));
    startDateInput.addEventListener('input', () => updateEndDateMin(true));

    // sync when endData false is.
    updateEndDateMin(false);
})();
