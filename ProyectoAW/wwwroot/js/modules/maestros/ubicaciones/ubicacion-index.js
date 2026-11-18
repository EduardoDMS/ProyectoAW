// VARIABLES GLOBALES: Instancias y variables compartidas
let tablaUbicaciones;

// INICIALIZACION: Punto de entrada del módulo
document.addEventListener('DOMContentLoaded', () => {
    inicializarTabla();
    cargarUbicaciones();
    cargarAlmacenesSelect();
    inicializarEventos();

    // Escucha el evento desde almacen-form
    document.addEventListener('ubicacion:guardado', () => {
        cargarUbicaciones();
    });
});

// CONFIGURACIÓN DE TABULATOR: Tabulator y sus columnas
function inicializarTabla() {
    tablaUbicaciones = new Tabulator('#tblUbicaciones', {
        layout: 'fitColumns',
        placeholder: 'No hay ubicaciones registradas',

        columnDefaults: {
            headerHozAlign: 'center',
            hozAlign: 'center'
        },

        columns: [
            { title: 'Código', field: 'codigo' },
            { title: 'Ubicación', field: 'nombre' },
            { title: 'Almacen', field: 'nombreAlmacen' },
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
                        editarUbicacion(fila);
                    }

                    if (e.target.closest('.btn-eliminar')) {
                        eliminarUbicacion(fila);
                    }
                }
            }
        ]
    });
}
// CARGA DE DATOS: Consumo de endpoints mediante Fetch
async function cargarUbicaciones() {
    try {
        const response = await fetch('/api/UbicacionApi');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resulado.mensaje || 'Error al cargar las ubicaciones');
        }

        if (resultado.exito) {
            const ubicaciones = resultado.datos;

            console.log(ubicaciones);
            console.log(resultado.mensaje);

            tablaUbicaciones.setData(ubicaciones);
        }
    } catch (error) {
        console.error('Error: ', error.message);
    }
}

// Select de crear y editar
async function cargarAlmacenesSelect() {
    try {
        const response = await fetch('/api/AlmacenApi');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar los almacenes');
        }

        if (resultado.exito) {
            const almacenes = resultado.datos;
            const selects = [
                document.getElementById('slcAlmacen'),
                document.getElementById('slcAlmacenEditar')
            ];

            selects.forEach(select => {
                // Limpiar opciones anteriores
                select.innerHTML = '<option value="0">Ninguno</option>';

                // Recorrer los almacenes
                almacenes.forEach(almacen => {
                    const option = document.createElement('option');

                    option.value = almacen.idAlmacen;
                    option.textContent = almacen.nombre;

                    select.appendChild(option);
                });
            });  
        }

    } catch (error) {
        console.error('Error: ', error.message);
    }
}

// EVENTOS: Interacciones del usuario
function inicializarEventos() {
    document.getElementById('txtBuscarUbicacion').addEventListener('input', filtrarUbicaciones);
    document.getElementById('slcEstado').addEventListener('change', filtrarUbicaciones);
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);
}

// FILTROS: Interacciones del usuario
function filtrarUbicaciones() {
    const nombre = document.getElementById('txtBuscarUbicacion').value.trim();
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

    tablaUbicaciones.setFilter(filtros);
}

function limpiarFiltros() {
    document.getElementById('txtBuscarUbicacion').value = "";
    document.getElementById('slcEstado').value = 0;

    tablaUbicaciones.clearFilter();
}