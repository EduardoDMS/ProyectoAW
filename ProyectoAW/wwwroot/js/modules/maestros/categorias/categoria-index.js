// VARIABLES GLOBALES
let tablaCategorias;

// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {

});

// CONFIGURACIÓN DE TABULATOR
function inicializarTabla() {
    tablaCategorias = new Tabulator('#tblCategorias', {
        layout: 'fitColumns',
        placeholder: 'No hay categorías registradas',

        columnDefaults: {
            headerHozAlign: 'center',
            hozAlign:'center'
        },

        columns: [
            { title: 'Nombre', field: 'nombre' },
            { title: 'Descripción', field: 'descripcion' },
            {
                title: 'Estado',
                field: 'activo',
                formatter: (cell) => cell.getValue()
                    ? '<span class="badge bg-success">Activo</span>'
                    : '<span class="badge bg-danger">Inactivo</span'
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
                        editarCategoria(fila);
                    }

                    if (e.target.closest('.btn-eliminar')) {
                        eliminarCategoria(fila);
                    }
                }
            }
        ]
    });
}

// CARGA DE DATOS
async function cargarCategorias() {
    try {
        const response = await fetch('/api/CategoriaApi');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar las categorías');
        }

        if (resultado.exito) {
            const categorias = resultado.datos;

            console.log(categorias);
            console.log(resultado.datos);

            tablaCategorias.setData(categorias);
        }
    } catch (error) {
        console.error('Error: ', error.message);
    }
}

// EVENTOS
function inicializarEventos() {
    document.getElementById('txtBuscarCategoria').addEventListener('input', filtrarCategorias);
    document.getElementById('slcEstado').addEventListener('change', filtrarCategorias);
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);
}

// FILTROS
function filtrarCategorias() {
    const nombre = document.getElementById('txtBuscarCategoria').value.trim();
    const estado = document.getElementById('slcEstado').value;

    const filtros = [];

    if (nombre != "") {
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
            value:estado=='1'
        });
    }

    tablaCategorias.setFilter(filtros);
}

function limpiarFiltros() {
    document.getElementById('txtBuscarCategoria').value = "";
    document.getElementById('slcEstado').value = 0;

    tablaCategorias.clearFilter();
}
