// VARIABLES GLOBALES
let tablaMotivosGuia;

// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    inicializarTabla();
    cargarMotivosGuia();
    cargarComportamientosSelect();
    inicializarEventos();
    
    document.addEventListener('motivo:guardado', () => {
        cargarMotivosGuia();
    })

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
async function cargarComportamientosSelect() {
    try {
        const response = await fetch('/api/TipoComportamientoApi');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar los comportamientos');
        }

        if (resultado.exito) {
            const comportamientos = resultado.datos;
            const selects = [
                document.getElementById('slcComportamiento'),
                document.getElementById('slcComportamientoEditar')
            ];

            selects.forEach(select => {
                // Limpiar opciones anteriores
                select.innerHTML = '<option value="0">Ninguno</option>';

                // Recorrer los almacenes
                comportamientos.forEach(comportamiento => {
                    const option = document.createElement('option');

                    option.value = comportamiento.idTipoComportamiento;
                    option.textContent = comportamiento.nombre;

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
    document.getElementById('txtBuscarMotivoGuia').addEventListener('input', filtrarMotivosGuia);
    document.getElementById('slcOperacion').addEventListener('change', filtrarMotivosGuia);
    document.getElementById('slcEstado').addEventListener('change', filtrarMotivosGuia);
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);

    validarCheckDatos();
}

// Validacion de checks
function validarCheckDatos() {
    const checksDatosGuia = document.querySelectorAll('.check-datos-guia');

    checksDatosGuia.forEach(check => {
        check.addEventListener('change', validarCantidadChecks);
    });
}
function validarCantidadChecks() {
    const seleccionados = document.querySelectorAll('.check-datos-guia:checked');

    if (seleccionados.length > 2) {
        this.checked = false;
        //alert('Solo puedes seleccionar un máximo de 2 opciones.')
    }
}

// FILTROS
function filtrarMotivosGuia() {
    const nombre = document.getElementById('txtBuscarMotivoGuia').value.trim();
    const operacion = document.getElementById('slcOperacion').value;
    const estado = document.getElementById('slcEstado').value;

    const filtros = [];

    if (nombre !== "") {
        filtros.push({
            field: 'nombreMotivo',
            type: 'like',
            value: nombre
        });
    }

    if (operacion !== "0") {
        filtros.push({
            field: 'idTipoOperacionGuia',
            type: '=',
            value: Number(operacion)
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
    document.getElementById('slcOperacion').value = 0;
    document.getElementById('slcEstado').value = 0;

    tablaMotivosGuia.clearFilter();
}