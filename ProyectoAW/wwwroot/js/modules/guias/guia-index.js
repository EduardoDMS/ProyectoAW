// VARIABLES GLOBALES
let tablaGuias;

// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    inicializarTabla();
    cargarGuias();
});

// CONFIGURACIÓN DE TABULATOR
function inicializarTabla() {
    tablaGuias = new Tabulator('#tblGuias', {
        layout: 'fitColumns',
        placeholder: 'No hay guías registrados',

        columnDefaults: {
            headerHozAlign: 'center',
            hozAlign: 'center'
        },

        columns: [
            { title: 'Guía', field: 'numeroGuia' },
            { title: 'Operación', field: 'tipoOperacion' },
            { title: 'Motivo', field: 'motivo' },
            { title: 'Fecha programada', field: 'fechaProgramada' },
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
                            <button class="btn btn-sm btn-primary btn-ver" title="Ver">
                                <i class="ti ti-eye-search"></i>
                            </button>

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

                    if (e.target.closest('.btn-ver')) {
                        verGuia(fila);
                    }

                    if (e.target.closest('.btn-editar')) {
                        editarGuia(fila);
                    }

                    if (e.target.closest('.btn-eliminar')) {
                        eliminarGuia(fila);
                    }
                }
            }
        ]
    });
}

// CARGA DE DATOS
async function cargarGuias() {
    try {
        const response = await fetch('/api/guias');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar los productos');
        }

        if (resultado.exito) {
            const guias = resultado.datos;

            console.log(guias);
            console.log(resultado.mensaje);

            tablaGuias.setData(guias);
        }

    } catch (error) {
        console.error('Error: ', error.message);
    }
}

// Select de crear y editar

// EVENTOS

// FILTROS