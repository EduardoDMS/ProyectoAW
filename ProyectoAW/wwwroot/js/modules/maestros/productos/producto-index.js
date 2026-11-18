// VARIABLES GLOBALES
let tablaProductos;

// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    inicializarTabla();
    cargarProductos();
    cargarCategoriasSelect();
    cargarUnidadMedidaSelect();
    inicializarEventos();

    document.addEventListener('producto:guardado', () => {
        cargarProductos();
    });
});

// CONFIGURACIÓN DE TABULATOR
function inicializarTabla() {
    tablaProductos = new Tabulator('#tblProductos', {
        layout: 'fitColumns',
        placeholder: 'No hay productos registrados',

        columnDefaults: {
            headerHozAlign: 'center',
            hozAlign: 'center'
        },

        columns: [
            { title: 'Código', field: 'codigo' },
            { title: 'Nombre', field: 'nombre' },
            { title: 'Categoría', field: 'idCategoria' },
            { title: 'Unidad de medida', field: 'idUnidadMedida' },
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
                        editarProducto(fila);
                    }

                    if (e.target.closest('.btn-eliminar')) {
                        eliminarProducto(fila);
                    }
                }
            }
        ]
    });
}

// CARGA DE DATOS
async function cargarProductos() {
    try {
        const response = await fetch('/api/ProductoApi');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar los productos');
        }

        if (resultado.exito) {
            const productos = resultado.datos;

            console.log(productos);
            console.log(resultado.mensaje);

            tablaProductos.setData(productos);
            //tablaAlmacenes.replaceData(almacenes);
        }
    } catch (error) {
        console.error('Error: ', error.message);
    }
}

// Select de crear y editar
async function cargarCategoriasSelect() {
    try {
        const response = await fetch('/api/CategoriaApi');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar las categorías');
        }

        if (resultado.exito) {
            const categorias = resultado.datos;
            const selects = [
                document.getElementById('slcCategoria'),
                document.getElementById('slcCategoriaEditar')
            ];

            selects.forEach(select => {
                // Limpiar opciones anteriores
                select.innerHTML = '<option value="0">Ninguno</option>';

                // Recorrer los almacenes
                categorias.forEach(categoria => {
                    const option = document.createElement('option');

                    option.value = categoria.idCategoria;
                    option.textContent = categoria.nombre;

                    select.appendChild(option);
                });
            });
        }

    } catch (error) {
        console.error('Error: ', error.message);
    }
}

async function cargarUnidadMedidaSelect() {
    try {
        const response = await fetch('/api/UnidadMedidaApi');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar las unidades de medida');
        }

        if (resultado.exito) {
            const unidadesMedida = resultado.datos;
            const selects = [
                document.getElementById('slcUnidadMedida'),
                document.getElementById('slcUnidadMedidaEditar')
            ];

            selects.forEach(select => {
                // Limpiar opciones anteriores
                select.innerHTML = '<option value="0">Ninguno</option>';

                // Recorrer los almacenes
                unidadesMedida.forEach(unidadMedida => {
                    const option = document.createElement('option');

                    option.value = unidadMedida.idUnidadMedida;
                    option.textContent = unidadMedida.nombre;

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
    document.getElementById('txtBuscarProducto').addEventListener('input', filtrarProductos);
    document.getElementById('slcEstado').addEventListener('change', filtrarProductos);
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);
}

// FILTROS
function filtrarProductos() {
    const nombre = document.getElementById('txtBuscarProducto').value.trim();
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

    tablaProductos.setFilter(filtros);
}

function limpiarFiltros() {
    document.getElementById('txtBuscarProducto').value = "";
    document.getElementById('slcEstado').value = 0;

    tablaProductos.clearFilter();
}