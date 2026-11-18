// VARIABLES GLOBALES
let tablaClientes;

// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    inicializarTabla();
    cargarClientes();
    inicializarEventos();

    document.addEventListener('cliente:guardado', () => {
        cargarClientes();
    });
});

// CONFIGURACIÓN DE TABULATOR
function inicializarTabla() {
    tablaClientes = new Tabulator('#tblClientes', {
        layout: 'fitColumns',
        placeholder: 'No hay clientes registrados',

        columnDefaults: {
            headerHozAlign: 'center',
            hozAlign: 'center'
        },

        columns: [
            { title: 'Razón social/Nombre', field: 'razonSocial' },
            { title: 'Tipo de documento', field: 'idTipoDocumento' },
            { title: 'Número', field: 'numeroDocumento' },
            { title: 'Correo', field: 'correo' },
            { title: 'Teléfono', field: 'telefono' },
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
                        editarCliente(fila);
                    }

                    if (e.target.closest('.btn-eliminar')) {
                        eliminarCliente(fila);
                    }
                }
            }
        ]
    });
}

// CARGA DE DATOS
async function cargarClientes() {
    try {
        const response = await fetch('/api/ClienteApi');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar los clientes');
        }

        if (resultado.exito) {
            const clientes = resultado.datos;

            console.log(clientes);
            console.log(resultado.mensaje);

            tablaClientes.setData(clientes);
        }

    } catch (error) {
        console.error('Error: ', error.message)
    }
}

// EVENTOS
function inicializarEventos() {
    document.getElementById('txtBuscarCliente').addEventListener('input', filtrarClientes);
    document.getElementById('slcEstado').addEventListener('change', filtrarClientes);
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);
}

// FILTROS
function filtrarClientes() {
    const nombre = document.getElementById('txtBuscarCliente').value.trim();
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

    tablaClientes.setFilter(filtros);
}

function limpiarFiltros() {
    document.getElementById('txtBuscarCliente').value = "";
    document.getElementById('slcEstado').value = 0;

    tablaClientes.clearFilter();
}