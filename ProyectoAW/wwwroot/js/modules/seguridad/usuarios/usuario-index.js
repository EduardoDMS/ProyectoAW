// VARIABLES GLOBALES
let tablaUsuarios;
let perfilesDisponibles = [];
let perfilesCargados = false;

// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    inicializarTabla();
    cargarUsuarios();
    cargarRolesSelect();
    cargarPerfilesSelect();
    inicializarEventos();

    document.addEventListener('usuario:guardado', () => {
        cargarUsuarios();
    });
});

// CONFIGURACIÓN DE TABULATOR
function inicializarTabla() {
    tablaUsuarios = new Tabulator('#tblUsuarios', {
        layout: 'fitColumns',
        placeholder: 'No hay usuarios registrados',

        columnDefaults: {
            headerHozAlign: 'center',
            hozAlign: 'center'
        },

        columns: [
            { title: 'Usuario', field: 'username' },
            { title: 'Nombre', field: 'nombre' },
            { title: 'Apellido', field: 'apellido' },
            { title: 'Rol', field: 'nombreRol' },
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
                        editarUsuario(fila);
                    }

                    if (e.target.closest('.btn-eliminar')) {
                        eliminarUsuario(fila);
                    }
                }
            }
        ]
    });
}

// CARGA DE DATOS
async function cargarUsuarios() {
    try {
        const response = await fetch('/api/UsuarioApi');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar los usuarios');
        }
        
        if (resultado.exito) {
            const usuarios = resultado.datos;

            console.log(usuarios);
            console.log(resultado.mensaje);

            tablaUsuarios.setData(usuarios);
            //tablaAlmacenes.replaceData(almacenes);
        }
    } catch (error) {
        console.error('Error: ', error.message);
    }
}

// Select de crear y editar
async function cargarRolesSelect() {
    try {
        const response = await fetch('/api/RolApi');

        const resultado = await response.json();
        console.log(resultado);
        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar los roles');
        }

        if (resultado.exito) {
            const roles = resultado.datos;
            const selects = [
                document.getElementById('slcRol'),
                document.getElementById('slcRolEditar')
            ];

            selects.forEach(select => {
                // Limpiar opciones anteriores
                select.innerHTML = '<option value="0">Ninguno</option>';

                // Recorrer los almacenes
                roles.forEach(rol => {
                    const option = document.createElement('option');

                    option.value = rol.idRol;
                    option.textContent = rol.nombre;

                    select.appendChild(option);
                });
            });
            actualizarPerfilesSelect(document.getElementById('slcRol').value, 'slcPerfil');
            actualizarPerfilesSelect(document.getElementById('slcRolEditar').value, 'slcPerfilEditar');
        }

    } catch (error) {
        console.error('Error: ', error.message);
    }
}

async function cargarPerfilesSelect() {
    if (!perfilesCargados) {
        const response = await fetch('/api/perfiles');
        const resultado = await response.json();
        if (!response.ok || !resultado.exito)
            throw new Error(resultado.mensaje || 'Error al cargar los perfiles');
        perfilesDisponibles = resultado.datos || [];
        perfilesCargados = true;
    }

    actualizarPerfilesSelect(document.getElementById('slcRol').value, 'slcPerfil');
    actualizarPerfilesSelect(document.getElementById('slcRolEditar').value, 'slcPerfilEditar');
}

function actualizarPerfilesSelect(idRol, idSelect, idPerfilSeleccionado = 0) {
    const select = document.getElementById(idSelect);
    if (!select) return;

    select.innerHTML = '<option value="0">Selecciona un perfil</option>';
    const perfilesRol = perfilesDisponibles.filter(perfil => Number(perfil.idRol) === Number(idRol));

    perfilesRol.forEach(perfil => {
        const option = document.createElement('option');
        option.value = perfil.idPerfil;
        option.textContent = perfil.nombrePerfil;
        select.appendChild(option);
    });

    if (idPerfilSeleccionado && perfilesRol.some(perfil => Number(perfil.idPerfil) === Number(idPerfilSeleccionado)))
        select.value = String(idPerfilSeleccionado);
}

// EVENTOS
function inicializarEventos() {
    document.getElementById('txtBuscarUsuario').addEventListener('input', filtrarUsuarios);
    document.getElementById('slcEstado').addEventListener('change', filtrarUsuarios);
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);
    document.getElementById('slcRol').addEventListener('change', event =>
        actualizarPerfilesSelect(event.target.value, 'slcPerfil'));
    document.getElementById('slcRolEditar').addEventListener('change', event =>
        actualizarPerfilesSelect(event.target.value, 'slcPerfilEditar'));
}

// FILTROS
function filtrarUsuarios() {
    const nombre = document.getElementById('txtBuscarUsuarios').value.trim();
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

    tablaUsuarios.setFilter(filtros);
}

function limpiarFiltros() {
    document.getElementById('txtBuscarUsuario').value = "";
    document.getElementById('slcEstado').value = 0;

    tablaUsuarios.clearFilter();
}
