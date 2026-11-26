// VARIABLES GLOBALES
let tablaGuias;

// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    inicializarTabla();
    cargarGuias();

    cargarMotivosGuiaSelect();
    cargarProveedoresSelect();
    cargarAlmacenesSelect();
    cargarClientesSelect();
    cargarProductosGuia();

    inicializarEventos();

    document.addEventListener('guia:guardado', () => {
        cargarGuias();
    });
});

// CONFIGURACIÓN DE TABULATOR
function inicializarTabla() {
    tablaGuias = new Tabulator('#tblGuias', {
        layout: 'fitColumns',
        placeholder: 'No hay guías registrados',

        columnDefaults: {
            headerHozAlign: 'center',
            hozAlign: 'center'
        },

        columns: [
            { title: 'Guía', field: 'numeroGuia' },
            { title: 'Operación', field: 'tipoOperacion' },
            { title: 'Motivo', field: 'motivo' },
            { title: 'Fecha programada', field: 'fechaProgramada' },
            {
                title: 'Estado',
                field: 'estado',
                formatter: function (cell) {
                    const estado = cell.getValue();

                    const estados = {
                        'Pendiente': '<span class="badge bg-warning"> Pendiente </span>',
                        'En proceso': '<span class="badge bg-primary"> En proceso </span>',
                        'Atendida': '<span class="badge bg-success"> Atendido </span>',
                        'Cancelada': '<span class="badge bg-danger"> Cancelado </span>'
                    };

                    return estados[estado] || '<span class="badge bg-secondary">Desconocido</span>'
                }
            },
            {
                title: 'Acciones',
                field: 'id',
                headerSort: false,
                formatter: function (cell) {
                    return `
                        <div class="d-flex justify-content-center gap-2">
                            <button class="btn btn-sm btn-primary btn-ver" title="Ver">
                                <i class="ti ti-eye"></i>
                            </button>

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

                    if (e.target.closest('.btn-ver')) {
                        verGuia(fila);
                    }

                    if (e.target.closest('.btn-editar')) {
                        editarGuia(fila);
                    }

                    if (e.target.closest('.btn-eliminar')) {
                        eliminarGuia(fila);
                    }
                }
            }
        ]
    });
}

// CARGA DE DATOS
async function cargarGuias() {
    try {
        const response = await fetch('/api/guias');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar los productos');
        }

        if (resultado.exito) {
            const guias = resultado.datos;

            // console.log(guias);
            // console.log(resultado.mensaje);

            guias.sort((a, b) => b.idGuia - a.idGuia);
            tablaGuias.setData(guias);
        }

    } catch (error) {
        console.error('Error: ', error.message);
    }
}

// Select de crear y editar
let motivosGuia = [];
async function cargarMotivosGuiaSelect() {
    try {
        const response = await fetch('/api/MotivoGuiaApi');

        const resultado = await response.json();
        
        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar los motivos de guía');
        }

        if (resultado.exito) {
            motivosGuia = resultado.datos;
            const selects = [
                document.getElementById('slcMotivoGuia'),
                document.getElementById('slcMotivoGuiaEditar')
            ];

            selects.forEach(select => {
                // Limpiar opciones anteriores
                select.innerHTML = '<option value="0">Ninguno</option>';

                // Recorrer los motivos
                motivosGuia
                    .filter(motivo=>motivo.activo)
                    .forEach(motivoGuia => {
                        const option = document.createElement('option');

                        option.value = motivoGuia.idMotivoGuia;
                        option.textContent = motivoGuia.nombreMotivo;

                        select.appendChild(option);
                    });
            });
            
        }

    } catch (error) {
        console.error('Error: ', error.message);
    }
}

async function cargarProveedoresSelect() {
    try {
        const response = await fetch('/api/ProveedorApi');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar los proveedores');
        }

        if (resultado.exito) {
            const proveedores = resultado.datos;
            const selects = [
                document.getElementById('slcProveedor'),
                document.getElementById('slcProveedorEditar')
            ];

            selects.forEach(select => {
                // Limpiar opciones anteriores
                select.innerHTML = '<option value="0">Ninguno</option>';

                // Recorrer los almacenes
                proveedores.forEach(proveedor => {
                    const option = document.createElement('option');

                    option.value = proveedor.idProveedor;
                    option.textContent = proveedor.razonSocial;

                    select.appendChild(option);
                });
            });
        }

    } catch (error) {
        console.error('Error: ', error.message);
    }
}

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
                document.getElementById('slcAlmacenOrigen'),
                document.getElementById('slcAlmacenDestino'),
                document.getElementById('slcAlmacenOrigenEditar'),
                document.getElementById('slcAlmacenDestinoEditar')
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

async function cargarClientesSelect() {
    try {
        const response = await fetch('/api/ClienteApi');

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar los clientes');
        }

        if (resultado.exito) {
            const clientes = resultado.datos;
            const selects = [
                document.getElementById('slcCliente'),
                document.getElementById('slcClienteEditar')
            ];

            selects.forEach(select => {
                // Limpiar opciones anteriores
                select.innerHTML = '<option value="0">Ninguno</option>';

                // Recorrer los almacenes
                clientes.forEach(cliente => {
                    const option = document.createElement('option');

                    option.value = cliente.idCliente;
                    option.textContent = cliente.razonSocial;

                    select.appendChild(option);
                });
            });
        }

    } catch (error) {
        console.error('Error: ', error.message);
    }
}

let productosGuia = [];
async function cargarProductosGuia() {
    try {
        const response = await fetch('api/ProductoApi');
        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error al cargar productos');
        }

        if (resultado.exito) {
            productosGuia = resultado.datos;
            //console.log(productosGuia);
        }

    } catch (error) {
        console.error(error);
    }
}
function cargarProductosSelect(fila) {
    const select = fila.querySelector('.slcProducto');

    productosGuia.forEach(producto => {
        const option = document.createElement('option');

        option.value = producto.idProducto;
        option.textContent = `${producto.codigo} - ${producto.nombre}`;

        select.appendChild(option);
    });
}

// EVENTOS
function inicializarEventos() {
    document.getElementById('txtBuscarGuia').addEventListener('input', filtrarGuia);
    document.getElementById('slcOperacion').addEventListener('change', filtrarGuia);
    document.getElementById('slcEstado').addEventListener('change', filtrarGuia);
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);
    document.getElementById('slcMotivoGuia').addEventListener('change', actualizarCamposMotivo);

    document.getElementById('btnAgregarProducto').addEventListener('click', () => {
        agregarProductoGuia('tbodyProductosGuia');
    });
    document.getElementById('tbodyProductosGuia').addEventListener('click', eliminarProductoGuia);

    document.getElementById('btnAgregarProductoEditar').addEventListener('click', () => {
        agregarProductoGuia('tbodyProductosGuiaEditar');
    });
    document.getElementById('tbodyProductosGuiaEditar').addEventListener('click', eliminarProductoGuia);
}

function actualizarCamposMotivo() {
    const idMotivo = Number(document.getElementById('slcMotivoGuia').value);
    
    const motivo = motivosGuia.find(item => item.idMotivoGuia == idMotivo);

    const campos = [
        { div: 'divProveedor', select: 'slcProveedor', requerido: motivo?.requiereProveedor },
        { div: 'divAlmacenDestino', select: 'slcAlmacenDestino', requerido: motivo?.requiereAlmacenDestino },
        { div: 'divAlmacenOrigen', select: 'slcAlmacenOrigen', requerido: motivo?.requiereAlmacenOrigen },
        { div: 'divCliente', select: 'slcCliente', requerido: motivo?.requiereCliente }
    ];

    campos.forEach(campo => {
        const contenedor = document.getElementById(campo.div);
        const select = document.getElementById(campo.select);
        const mostrar = campo.requerido === true;

        contenedor.classList.toggle('d-none', !mostrar);
        select.required = mostrar;

        if (!mostrar) {
            select.value = '0';
        }
    });
}

// Para crear y editar
function agregarProductoGuia(idTbody, detalle=null) {
    const tbody = document.getElementById(idTbody);
    const fila = document.createElement('tr');

    fila.innerHTML = `
        <td>
            <select class="form-select slcProducto">
                <option value="0">Seleccione un producto</option>
            </select>
        </td>

        <td>
            <input type="number" class="form-control txtCantidad" min="1" step="1" placeholder="Ingrese cantidad">
        </td>

        <td class="text-center">
            <button type="button" class="btn btn-outline-danger btnEliminarProducto" title="Eliminar producto">
                <i class="ti ti-trash fs-5"></i>
            </button>
        </td>
    `;

    tbody.appendChild(fila);

    cargarProductosSelect(fila);

    if (detalle) {
        const select = fila.querySelector('.slcProducto');
        const input = fila.querySelector('.txtCantidad');

        select.value = detalle.idProducto;
        input.value = detalle.cantidad;
    }
}

function eliminarProductoGuia(event) {
    const boton = event.target.closest('.btnEliminarProducto');

    if (!boton) return;

    boton.closest('tr').remove();
}

// FILTROS
function filtrarGuia() {
    const numeroGuia = document.getElementById('txtBuscarGuia').value.trim();
    const operacion = document.getElementById('slcOperacionFiltro').value;
    const estado = document.getElementById('slcEstado').value;

    const filtros = [];

    if (numeroGuia !== "") {
        filtros.push({
            field: 'numeroGuia',
            type: 'like',
            value: numeroGuia
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
            field: 'estado',
            type: '=',
            value: estado
        });
    }

    tablaGuias.setFilter(filtros);
}

function limpiarFiltros() {
    document.getElementById('txtBuscarGuia').value = "";
    document.getElementById('slcOperacionFiltro').value = 0;
    document.getElementById('slcEstado').value = 0;

    tablaGuias.clearFilter();
}