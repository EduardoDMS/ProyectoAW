// VARIABLES GLOBALES
let tablaMotivosGuia;

// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    inicializarTabla();
    cargarMotivosGuia();
    inicializarEventos();
});

// CONFIGURACIÓN DE TABULATOR
function inicializarTabla() {
    tablaMotivosGuia = new Tabulator('#tblMotivosGuia', {
        layout: 'fitColumns',
        placeholder: 'No hay motivos registrados',

        columnDefaults: {
            headerHozAlign: 'center',
            hozAlign: 'center'
        },

        columns: [
            { title: 'Motivo', field: 'nombreMotivo' },
            { title: 'Tipo de operación', field: 'tipoOperacion' },
            { title: 'Comportamiento', field: 'tipoComportamiento' },
            {
                title: 'Datos solicitados',
                field: 'datosSolicitados',
                // variableHeight:true,
                headerSort:false,
                formatter: function (cell) {
                    const motivo = cell.getRow().getData();

                    const datos = [
                        { campo: 'requiereProveedor', nombre: 'Proveedor' },
                        { campo: 'requiereCliente', nombre: 'Cliente' },
                        { campo: 'requiereAlmacenOrigen', nombre: 'Almacen origen' },
                        { campo: 'requiereAlmacenDestino', nombre: 'Almacen destino' }
                    ];

                    const badges = datos
                        .filter(item => motivo[item.campo])
                        .map(item => `
                            <span class="badge rounded-pill bg-primary-subtle text-primary me-1">
                                ${item.nombre}
                            </span>
                        `);

                    return badges.length > 0
                        ? `<div class="d-flex justify-content-center flex-wrap gap-1">${badges.join('')}</div>`
                        : '<span class="text-muted">Ninguno</span>';
                }
            },
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
                        editarMotivoGuia(fila);
                    }

                    if (e.target.closest('.btn-eliminar')) {
                        eliminarMotivoGuia(fila);
                    }
                }
            }
        ]
    });
}

// CARGA DE DATOS
async function cargarMotivosGuia() {
    try {
        const response = await fetch('/api/MotivoGuiaApi');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar los motivos');
        }

        if (resultado.exito) {
            const motivosGuia = resultado.datos;

            console.log(motivosGuia);
            console.log(resultado.mensaje);

            tablaMotivosGuia.setData(motivosGuia);
        }

    } catch (error) {
        console.error('Error: ', error.message);
    }
}

// Select de crear y editar

// EVENTOS
function inicializarEventos() {
    document.getElementById('txtBuscarMotivoGuia').addEventListener('input', filtrarMotivosGuia);
    document.getElementById('slcEstado').addEventListener('change', filtrarMotivosGuia);
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);
}
// FILTROS
function filtrarMotivosGuia() {
    const nombre = document.getElementById('txtBuscarMotivoGuia').value.trim();
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

    tablaMotivosGuia.setFilter(filtros);
}

function limpiarFiltros() {
    document.getElementById('txtBuscarMotivoGuia').value = "";
    document.getElementById('slcEstado').value = 0;

    tablaMotivosGuia.clearFilter();
}