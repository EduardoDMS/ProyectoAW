// VARIABLES GLOBALES
let tablaUnidadesMedida;

// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    inicializarTabla();
    cargarUnidadesMedida();
    inicializarEventos();

    // Escucha el evento desde almacen-form
    document.addEventListener('unidadMedida:guardado', () => {
        cargarUnidadesMedida();
    });
});

// CONFIGURACIÓN DE TABULATOR
function inicializarTabla() {
    tablaUnidadesMedida = new Tabulator('#tblUnidadesMedida', {
        layout: 'fitColumns',
        placeholder: 'No hay unidades de medida registradas',

        columnDefaults: {
            headerHozAlign: 'center',
            hozAlign: 'center'
        },

        columns: [
            { title: 'Código', field: 'codigo' },
            { title: 'Nombre', field: 'nombre' },
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
                        editarUnidadMedida(fila);
                    }

                    if (e.target.closest('.btn-eliminar')) {
                        eliminarUnidadMedida(fila);
                    }
                }
            }
        ]
    });
}

// CARGA DE DATOS

async function cargarUnidadesMedida() {
    try {
        const response = await fetch('/api/UnidadMedidaApi');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar las unidades de medida');
        }

        if (resultado.exito) {
            const unidadesMedida = resultado.datos;

            console.log(unidadesMedida);
            console.log(resultado.mensaje);

            tablaUnidadesMedida.setData(unidadesMedida);

        }
    } catch (error) {
        console.error('Error: ', error.message);
    }
}

// EVENTOS
function inicializarEventos() {
    document.getElementById('txtBuscarUnidadMedida').addEventListener('input', filtrarUnidadesMedida);
    document.getElementById('slcEstado').addEventListener('change', filtrarUnidadesMedida);
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);
}

// FILTROS

function filtrarUnidadesMedida() {
    const nombre = document.getElementById('txtBuscarUnidadMedida').value.trim();
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

    tablaUnidadesMedida.setFilter(filtros);
}

function limpiarFiltros() {
    document.getElementById('txtBuscarUnidadMedida').value = "";
    document.getElementById('slcEstado').value = 0;

    tablaUnidadesMedida.clearFilter();
}