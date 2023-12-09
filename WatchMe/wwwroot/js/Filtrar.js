
let filterMethod = function (text) {
    let renglones = Array.from(this.querySelectorAll("tbody tr"));

    // let ocultar = renglones.forEach(x =>
    //     x.hidden = !Array.from(x.childNodes).some(y => y.textContent.toLowerCase().includes(text.toLowerCase())));
    let ocultar = renglones.forEach(row => {
        let cellsToCheck = Array.from(row.querySelectorAll('td:not(:last-child)')); // Selecciona todas las celdas excepto la última
        row.hidden = !cellsToCheck.some(cell => cell.textContent.toLowerCase().includes(text.toLowerCase()));
    });
}

let paginateMethod = function (tamanoPagina) {
    let totalTr = this.querySelectorAll("tbody tr").length;
    let totalPaginas = Math.ceil(totalTr / tamanoPagina);
    let tfoot = document.createElement("tfoot");
    let tfootTr = tfoot.insertRow();
    let tFootTd = tfootTr.insertCell();

    for (var i = 1; i <= totalPaginas; i++) {
        let a = document.createElement("a");
        a.textContent = i;
        a.href = "#";
        a.dataset.inicio = (i - 1) * tamanoPagina;
        a.dataset.fin = parseInt(a.dataset.inicio) + tamanoPagina - 1;
        a.onclick = mostrarTr;
        tFootTd.append(a);
    }
    tFootTd.colSpan = this.tBodies[0].rows[0].cells.length;

    this.append(tfoot);
    if (tFootTd.firstElementChild != null) {
        tFootTd.firstElementChild.click();
    }
};

var todosLosTr;
var trVisibles;

function mostrarTr(event) {
    let a = event.target;
    let inicio = parseInt(a.dataset.inicio);
    let fin = parseInt(a.dataset.fin);

    if (!todosLosTr) {
        todosLosTr = Array.from(a.closest("table").querySelectorAll("tbody tr"));
    }

    if (trVisibles) {
        for (let tr of trVisibles) {
            tr.hidden = true;
        }
    }

    trVisibles = todosLosTr.slice(inicio, fin + 1);

    for (let tr of trVisibles) {
        tr.hidden = false;
    }
};


document.querySelectorAll("table").forEach(x => {
    x.filter = filterMethod;
    x.paginate = paginateMethod;
});
