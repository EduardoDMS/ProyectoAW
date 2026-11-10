// VARIABLES GLOBALES: Instancias y variables compartidas
let tablaAlmacenes;

// INICIALIZACION: Punto de entrada del módulo
document.addEventListener('DOMContentLoaded', () => {
    inicializarTabla();
    cargarAlmacenes();
    inicializarEventos();

    // Escucha el evento desde almacen-form
    document.addEventListener('almacen:creado', () => {
        cargarAlmacenes();
    });
});

// CONFIGURACIÓN DE TABULATOR: Tabulator y sus columnas
function inicializarTabla() {
    tablaAlmacenes = new Tabulator('#tblAlmacenes', {
        layout: 'fitColumns',
        placeholder: 'No hay almacenes registrados',

        columnDefaults: {
            headerHozAlign: 'center',
            hozAlign:'center'
        },

        columns: [
            { title: 'Código', field: 'codigo' },
            { title: 'Almacén', field: 'nombre' },
            { title: 'Fecha de registro', field: 'fchRegistro' },
            {
                title: 'Estado',
                field: 'activo',
                formatter: (cell) => cell.getValue()
                    ? '<span class="badge bg-success">Activo</span>'
                    : '<span class="badge bg-danger">Inactivo</span>'
            },
            {
                title: 'Acciones',
                field: 'id',
                headerSort: false,
                formatter: function (cell) {
                    return `
                        <div class="d-flex justify-content-center gap-2">
                            <button class="btn btn-sm btn-primary btn-editar" title="Editar">
                                <i class="ti ti-edit"></i>
                            </button>

                            <button class="btn btn-sm btn-danger btn-eliminar" title="Eliminar">
                                <i class="ti ti-trash"></i>
                            </button>
                        </div>
                    `;
                },
                cellClick: function (e, cell) {
                    const fila = cell.getRow().getData();

                    if (e.target.closest('.btn-editar')) {
                        editarAlmacen(fila);
                    }

                    if (e.target.closest('.btn-eliminar')) {
                        eliminarAlmacen(fila);
                    }
                }
            }
        ]
    });
}

// CARGO DE DATOS: Consumo de endpoints mediante Fetch

async function cargarAlmacenes() {
    try {
        const response = await fetch('/api/AlmacenApi');
        //console.log(response);

        if (!response.ok) {
            throw new Error('Error al cargar los almacenes');
        }

        const datos = await response.json();
        //console.log(datos);

        tablaAlmacenes.setData(datos);
    } catch (error) {
        console.error(error);
    }
}

// EVENTOS: Interacciones del usuario
function inicializarEventos() {
    document.getElementById('txtBuscarAlmacen').addEventListener('input', filtrarAlmacenes);
    document.getElementById('slcEstado').addEventListener('change', filtrarAlmacenes);
}

// FILTROS: Interacciones del usuario
function filtrarAlmacenes() {
    const nombre = document.getElementById('txtBuscarAlmacen').value.trim();
    const estado = document.getElementById('slcEstado').value;

    const filtros = [];

    if (nombre !== "") {
        filtros.push({
            field: 'nombre',
            type: 'like',
            value: nombre
        });
    }

    if (estado !== '0') {
        filtros.push({
            field: 'activo',
            type: '=',
            value: estado === '1'
        });
    }

    tablaAlmacenes.setFilter(filtros);
}