// CONSTANTES (visibles tambien para perfil-form.js)
const URL_PERFILES = '/api/perfiles';
const URL_ROLES = '/api/RolApi';          // <-- VERIFICA esta ruta: es la que usa tu formulario de usuarios para listar roles
const PERMISOS = ['ver', 'crear', 'actualizar', 'eliminar'];

// VARIABLES GLOBALES
let tablaPerfiles;
let rolesListos;   // promesa: editarPerfil espera a que los selects de rol esten llenos

// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    inicializarTabla();
    cargarPerfiles();
    rolesListos = cargarRolesSelect();
    inicializarEventos();

    document.addEventListener('perfil:guardado', () => {
        cargarPerfiles();
    });
});

// HELPER: llama a la API y devuelve resultado.datos; si algo falla lanza Error con el mensaje del servidor
async function llamarApi(url, opciones = {}) {
    const response = await fetch(url, {
        headers: { 'Content-Type': 'application/json' },
        ...opciones
    });

    const resultado = await response.json().catch(() => null);

    if (!response.ok || !resultado?.exito) {
        throw new Error(resultado?.mensaje || `Error ${response.status}`);
    }

    return resultado.datos;
}

// CONFIGURACION DE TABULATOR
function inicializarTabla() {
    tablaPerfiles = new Tabulator('#tblPerfiles', {
        layout: 'fitColumns',
        placeholder: 'No hay perfiles registrados',

        columnDefaults: {
            headerHozAlign: 'center',
            hozAlign: 'center'
        },

        columns: [
            { title: 'Nombre', field: 'nombrePerfil' },
            { title: 'Rol', field: 'nombreRol' },
            {
                title: 'Módulos',
                field: 'permisos',
                headerSort: false,
                formatter: (cell) => cell.getValue().length
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
                field: 'idPerfil',
                headerSort: false,
                formatter: function () {
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
                        editarPerfil(fila);
                    }

                    if (e.target.closest('.btn-eliminar')) {
                        eliminarPerfil(fila);
                    }
                }
            }
        ]
    });
}

// CARGA DE DATOS
async function cargarPerfiles() {
    try {
        const perfiles = await llamarApi(URL_PERFILES);
        tablaPerfiles.setData(perfiles);
    } catch (error) {
        console.error('Error: ', error.message);
    }
}

// Select de roles (crear y editar)
async function cargarRolesSelect() {
    try {
        const roles = await llamarApi(URL_ROLES);

        ['slcRol', 'slcRolEditar'].forEach(id => {
            const select = document.getElementById(id);
            select.innerHTML = '<option value="0">Seleccione un rol</option>';

            roles.forEach(rol => select.add(new Option(rol.nombre, rol.idRol)));
        });
    } catch (error) {
        console.error('Error: ', error.message);
    }
}

// EVENTOS
function inicializarEventos() {
    document.getElementById('txtBuscarPerfil').addEventListener('input', filtrarPerfiles);
    document.getElementById('slcEstado').addEventListener('change', filtrarPerfiles);
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);
}

// FILTROS
function filtrarPerfiles() {
    const nombre = document.getElementById('txtBuscarPerfil').value.trim();
    const estado = document.getElementById('slcEstado').value;

    const filtros = [];

    if (nombre !== '') {
        filtros.push({ field: 'nombrePerfil', type: 'like', value: nombre });
    }

    if (estado !== '0') {
        filtros.push({ field: 'activo', type: '=', value: estado === '1' });
    }

    tablaPerfiles.setFilter(filtros);
}

function limpiarFiltros() {
    document.getElementById('txtBuscarPerfil').value = '';
    document.getElementById('slcEstado').value = '0';

    tablaPerfiles.clearFilter();
}