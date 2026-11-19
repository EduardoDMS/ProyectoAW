// VARIABLES GLOBALES
let tablaProveedores;

// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    inicializarTabla();
    cargarProveedores();
    cargarTipoDocumentosSelect();
    inicializarEventos();

    document.addEventListener('proveedor:guardado', () => {
        cargarProveedores();
    });
});

// CONFIGURACIÓN DE TABULATOR
function inicializarTabla() {
    tablaProveedores = new Tabulator('#tblProveedores', {
        layout: 'fitColumns',
        placeholder: 'No hay proveedores registrados',

        columnDefaults: {
            headerHozAlign: 'center',
            hozAlign: 'center'
        },

        columns: [
            { title: 'Razón social/Nombre', field: 'razonSocial' },
            { title: 'Tipo de documento', field: 'nombreDocumento' },
            { title: 'Número', field: 'numeroDocumento' },
            { title: 'Correo', field: 'correoContacto' },
            { title: 'Teléfono', field: 'telefonoContacto' },
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
                        editarProveedor(fila);
                    }

                    if (e.target.closest('.btn-eliminar')) {
                        eliminarProveedor(fila);
                    }
                }
            }
        ]
    });
}

// CARGA DE DATOS
async function cargarProveedores() {
    try {
        const response = await fetch('/api/ProveedorApi');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar los proveedores');
        }

        if (resultado.exito) {
            const proveedores = resultado.datos;

            console.log(proveedores);
            console.log(resultado.mensaje);

            tablaProveedores.setData(proveedores);
        }

    } catch (error) {
        console.error('Error: ', error.message)
    }
}

// Select de crear y editar
async function cargarTipoDocumentosSelect() {
    try {
        const response = await fetch('/api/TipoDocumentoApi');

        const resultado = await response.json();
        console.log(resultado);
        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar los tipos de documento');
        }

        if (resultado.exito) {
            const tipoDocumentos = resultado.datos;
            const selects = [
                document.getElementById('slcTipoDocumento'),
                document.getElementById('slcTipoDocumentoEditar')
            ];

            selects.forEach(select => {
                // Limpiar opciones anteriores
                select.innerHTML = '<option value="0">Ninguno</option>';

                // Recorrer los almacenes
                tipoDocumentos.forEach(tipoDocumento => {
                    const option = document.createElement('option');

                    option.value = tipoDocumento.idTipoDoc;
                    option.textContent = tipoDocumento.documento;

                    select.appendChild(option);
                });
            });
        }

    } catch (error) {
        console.error('Error: ', error.message);
    }
}

// EVENTOS
function inicializarEventos() {
    document.getElementById('txtBuscarProveedor').addEventListener('input', filtrarProveedores);
    document.getElementById('slcEstado').addEventListener('change', filtrarProveedores);
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);
}

// FILTROS
function filtrarProveedores() {
    const nombre = document.getElementById('txtBuscarProveedor').value.trim();
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

    tablaProveedores.setFilter(filtros);
}

function limpiarFiltros() {
    document.getElementById('txtBuscarProveedor').value = "";
    document.getElementById('slcEstado').value = 0;

    tablaProveedores.clearFilter();
}