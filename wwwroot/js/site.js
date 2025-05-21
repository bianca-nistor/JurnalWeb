function applySavedPreferences() {
    const savedTextColor = localStorage.getItem('textColor');
    const savedBgColor = localStorage.getItem('bgColor');
    const noteArea = document.getElementById('noteContainer'); // <- containerul cu notițele

    if (!noteArea) return; // dacă nu suntem pe pagina cu notițe, ieșim

    if (savedTextColor) {
        noteArea.style.color = savedTextColor;
        const colorInput = document.getElementById('textColor');
        if (colorInput) colorInput.value = savedTextColor;
    }

    if (savedBgColor) {
        noteArea.style.backgroundColor = savedBgColor;
        const bgInput = document.getElementById('bgColor');
        if (bgInput) bgInput.value = savedBgColor;
    }
}

function savePreferences() {
    const textColor = document.getElementById('textColor')?.value;
    const bgColor = document.getElementById('bgColor')?.value;
    const noteArea = document.getElementById('noteContainer');

    if (!noteArea) return;

    if (textColor) {
        localStorage.setItem('textColor', textColor);
        noteArea.style.color = textColor;
    }

    if (bgColor) {
        localStorage.setItem('bgColor', bgColor);
        noteArea.style.backgroundColor = bgColor;
    }

    alert('Preferințele au fost salvate!');
}

function resetPreferences() {
    const noteArea = document.getElementById('noteContainer');
    if (!noteArea) return;

    localStorage.removeItem('textColor');
    localStorage.removeItem('bgColor');

    noteArea.style.color = '';
    noteArea.style.backgroundColor = '';
}

window.onload = function () {
    applySavedPreferences();

    const saveBtn = document.getElementById('savePreferences');
    if (saveBtn) saveBtn.addEventListener('click', savePreferences);

    const resetBtn = document.getElementById('resetPreferences');
    if (resetBtn) resetBtn.addEventListener('click', resetPreferences);
};
