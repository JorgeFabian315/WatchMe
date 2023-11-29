
let filterMethod = function (text) {
    let renglones = Array.from(this.querySelectorAll("tbody tr"));

    // let ocultar = renglones.forEach(x =>
    //     x.hidden = !Array.from(x.childNodes).some(y => y.textContent.toLowerCase().includes(text.toLowerCase())));
    let ocultar = renglones.forEach(row => {
        let cellsToCheck = Array.from(row.querySelectorAll('td:not(:last-child)')); // Selecciona todas las celdas excepto la última
        row.hidden = !cellsToCheck.some(cell => cell.textContent.toLowerCase().includes(text.toLowerCase()));
    });
}


document.querySelectorAll("table").forEach(x => {
    x.filter = filterMethod;
});
