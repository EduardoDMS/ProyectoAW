// VARIABLES GLOBALES
let tablaUsuarios;

// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    inicializarTabla();
    cargarUsuarios();
    cargarRolesSelect();
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
        }

    } catch (error) {
        console.error('Error: ', error.message);
    }
}

// EVENTOS
function inicializarEventos() {
    document.getElementById('txtBuscarUsuario').addEventListener('input', filtrarUsuarios);
    document.getElementById('slcEstado').addEventListener('change', filtrarUsuarios);
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);
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