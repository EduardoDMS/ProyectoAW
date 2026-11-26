// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('btnGuardarGuia').addEventListener('click', guardarGuia);

    document.getElementById('btnActualizarGuia').addEventListener('click', actualizarGuia);
});

// GUARDAR GUIA
async function guardarGuia() {
    const idTipoOperacion = parseInt(document.getElementById('slcOperacion').value);
    const idMotivoGuia = parseInt(document.getElementById('slcMotivoGuia').value);
    const motivo = motivosGuia.find(item => item.idMotivoGuia == idMotivoGuia);

    const idProveedor = parseInt(document.getElementById('slcProveedor').value);
    const idAlmacenOrigen = parseInt(document.getElementById('slcAlmacenOrigen').value);
    const idAlmacenDestino = parseInt(document.getElementById('slcAlmacenDestino').value);
    const idCliente = parseInt(document.getElementById('slcCliente').value);

    const fechaProgramada = document.getElementById('txtFechaProgramada').value;

    const tipoDocumentoReferencia = document.getElementById('txtTipoDocumento').value.trim();
    const numeroDocumentoReferencia = document.getElementById('txtNumeroDocumento').value.trim();

    const observacion = document.getElementById('txtObservacion').value.trim();

    const detalles = obtenerDetallesGuia('tbodyProductosGuia');

    // Validaciones
    if (idTipoOperacion == 0) {
        alert("Seleccione un tipo de operación para la guía");
        return;
    }

    if (!motivo) {
        alert("Seleccione un motivo para la guía");
        return;
    }

    if (motivo.requiereProveedor && idProveedor == 0) {
        alert("Seleccione un proveedor");
        return;
    }

    if (motivo.requiereAlmacenOrigen && idAlmacenOrigen == 0) {
        alert("Seleccione un proveedor");
        return;
    }

    if (motivo.requiereAlmacenDestino && idAlmacenDestino == 0) {
        alert("Seleccione un proveedor");
        return;
    }

    if (motivo.requiereCliente && idCliente == 0) {
        alert("Seleccione un proveedor");
        return;
    }

    if (!fechaProgramada) {
        alert("Ingrese la fecha programada");
        return;
    }

    if (detalles.length === 0) {
        alert("Agregue al menos un producto a la guía");
        return;
    }

    const request = {
        idTipoOperacion: idTipoOperacion,
        idMotivoGuia: idMotivoGuia,
        idProveedor: motivo.requiereProveedor ? idProveedor:null,
        idAlmacenOrigen: motivo.requiereAlmacenOrigen ? idAlmacenOrigen:null,
        idAlmacenDestino: motivo.requiereAlmacenDestino ? idAlmacenDestino:null,
        idCliente: motivo.requiereCliente ? idCliente:null,
        fechaProgramada: fechaProgramada,
        tipoDocumentoReferencia: tipoDocumentoReferencia,
        numeroDocumentoReferencia: numeroDocumentoReferencia,
        observacion: observacion,
        detalles: detalles
    };
    console.log(request);
    try {
        const response = await fetch('api/guias', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        const resultado = await response.json();

        if (!response.ok || !resultado.exito) {
            throw new Error(resultado.mensaje || "No se pudo crear la guia");
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalCrearGuia');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Limpiar formulario
        limpiarFormularioGuia();

        document.dispatchEvent(new CustomEvent('guia:guardado'));

    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}
// Para crear y editar
function obtenerDetallesGuia(idTbody) {
    const detalles = [];

    document.querySelectorAll(`#${idTbody} tr`).forEach(fila => {
        const idProducto = Number(fila.querySelector('.slcProducto').value);
        const cantidad = Number(fila.querySelector('.txtCantidad').value);

        detalles.push({
            idProducto: idProducto,
            cantidad: cantidad
        });
    });

    console.log(detalles);
    return detalles;
}

// LIMPIAR FORMULARIO
function limpiarFormularioGuia() {
    document.getElementById('slcOperacion').value = "0";
    document.getElementById('slcMotivoGuia').value = "0";
    document.getElementById('slcProveedor').value = "0";
    document.getElementById('slcAlmacenOrigen').value = "0";
    document.getElementById('slcAlmacenDestino').value = "0";
    document.getElementById('slcCliente').value = "0";
    document.getElementById('txtFechaProgramada').value = "";
    document.getElementById('txtTipoDocumento').value = "";
    document.getElementById('txtNumeroDocumento').value = "";
    document.getElementById('txtObservacion').value = "";
}

// VER GUIA
async function verGuia(guia) {
    try {
        const response = await fetch(`api/guias/${guia.idGuia}`);

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'No se pudo obtener la guia');
        }

        if (resultado.exito) {
            const guia = resultado.datos;

            console.log(guia);
            console.log(resultado.mensaje);

            // Cargar datos en el formulario
            document.getElementById('txtTituloGuia').textContent = `Guía / ${guia.numeroGuia}`;

            establecerOpcionVer('slcOperacionVer', guia.idTipoOperacionGuia, guia.tipoOperacion);
            establecerOpcionVer('slcMotivoGuiaVer', guia.idMotivoGuia, guia.motivo);
            establecerOpcionVer('slcProveedorVer', guia.idProveedor, guia.proveedor);
            establecerOpcionVer('slcAlmacenDestinoVer', guia.idAlmacenDestino, guia.almacenDestino);
            establecerOpcionVer('slcAlmacenOrigenVer', guia.idAlmacenOrigen, guia.almacenOrigen);
            establecerOpcionVer('slcClienteVer', guia.idCliente, guia.cliente);

            document.getElementById('txtFechaProgramadaVer').value = guia.fechaProgramada ?? '';
            document.getElementById('txtTipoDocumentoVer').value = guia.tipoDocumentoReferencia ?? '';
            document.getElementById('txtNumeroDocumentoVer').value = guia.numeroDocumentoReferencia ?? '';
            document.getElementById('txtObservacionVer').value = guia.observacion ?? '';

            actualizarCamposMotivoVer(guia.idMotivoGuia);

            cargarProductosGuiaVer(guia.detalles);

            // Abrir modal
            const modalElement = document.getElementById('modalVerGuia');
            const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
            modal.show();
        }

    } catch (error) {
        console.error('Error: ', error.message);
        alert(error.message);
    }
}
function actualizarCamposMotivoVer(idMotivo) {
    const motivo = motivosGuia.find(item => item.idMotivoGuia == idMotivo);

    const campos = [
        { div: 'divProveedorVer', select: 'slcProveedorVer', requerido: motivo?.requiereProveedor },
        { div: 'divAlmacenDestinoVer', select: 'slcAlmacenDestinoVer', requerido: motivo?.requiereAlmacenDestino },
        { div: 'divAlmacenOrigenVer', select: 'slcAlmacenOrigenVer', requerido: motivo?.requiereAlmacenOrigen },
        { div: 'divClienteVer', select: 'slcClienteVer', requerido: motivo?.requiereCliente }
    ];

    campos.forEach(campo => {
        const contenedor = document.getElementById(campo.div);

        contenedor.classList.toggle('d-none', campo.requerido !== true);
    });
}
function establecerOpcionVer(id, valor, texto) {
    const select = document.getElementById(id);

    select.innerHTML = '';

    const option = document.createElement('option');
    option.value = valor ?? '-1';
    option.textContent = texto ?? 'Ninguna';

    select.appendChild(option);
    select.value = String(valor ?? '-1');
}
function cargarProductosGuiaVer(detalles) {
    const tbody = document.getElementById('tbodyProductosGuiaVer');

    tbody.innerHTML = '';

    detalles.forEach(detalle => {
        const fila = document.createElement('tr');

        const tdProducto = document.createElement('td');
        tdProducto.textContent = `${detalle.codProducto} - ${detalle.producto}`;

        const tdCantidad = document.createElement('td');
        tdCantidad.textContent = detalle.cantidad;

        fila.appendChild(tdProducto);
        fila.appendChild(tdCantidad);

        tbody.appendChild(fila);
    });
}

// EDITAR GUIA
async function editarGuia(guia) {
    try {
        // Consultar los datos actualizados por ID
        const response = await fetch(`api/guias/${guia.idGuia}`);

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'No se pudo obtener la guía');
        }

        if (resultado.exito) {
            const guia = resultado.datos;

            console.log(guia);
            console.log(resultado.mensaje);

            // Cargar datos en el formulario
            document.getElementById('txtIdGuia').value = guia.idGuia;
            document.getElementById('slcOperacionEditar').value = String(guia.idTipoOperacionGuia);
            document.getElementById('slcMotivoGuiaEditar').value = String(guia.idMotivoGuia);
            document.getElementById('slcProveedorEditar').value = String(guia.idProveedor);
            document.getElementById('slcAlmacenDestinoEditar').value = String(guia.idAlmacenDestino);
            document.getElementById('slcAlmacenOrigenEditar').value = String(guia.idAlmacenOrigen);
            document.getElementById('slcClienteEditar').value = String(guia.idCliente);

            document.getElementById('txtFechaProgramadaEditar').value = guia.fechaProgramada ?? '';
            document.getElementById('txtTipoDocumentoEditar').value = guia.tipoDocumentoReferencia ?? '';
            document.getElementById('txtNumeroDocumentoEditar').value = guia.numeroDocumentoReferencia ?? '';
            document.getElementById('txtObservacionEditar').value = guia.observacion ?? '';

            actualizarCamposMotivo(guia.idMotivoGuia);
            cargarProductosGuiaEditar(guia.detalles);

            // Abrir modal
            const modalElement = document.getElementById('modalEditarGuia');
            const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
            modal.show();
        }
    } catch (error) {
        console.error('Error: ', error.message);
        alert(error.message);
    }
}
function cargarProductosGuiaEditar(detalles){
    const tbody = document.getElementById('tbodyProductosGuiaEditar');

    tbody.innerHTML = '';

    detalles.forEach(detalle => {
        agregarProductoGuia('tbodyProductosGuiaEditar', detalle);
    });
}

// ACTUALIZAR GUIA
async function actualizarGuia() {
    const id = document.getElementById('txtIdGuia').value;
    
    const request = {
        idTipoOperacion: parseInt(document.getElementById('slcOperacionEditar').value),
        idMotivoGuia: parseInt(document.getElementById('slcMotivoGuiaEditar').value),
        idProveedor: parseInt(document.getElementById('slcProveedorEditar').value),
        idAlmacenOrigen: parseInt(document.getElementById('slcAlmacenOrigenEditar').value),
        idAlmacenDestino: parseInt(document.getElementById('slcAlmacenDestinoEditar').value),
        idCliente: parseInt(document.getElementById('slcClienteEditar').value),
        fechaProgramada: document.getElementById('txtFechaProgramadaEditar').value,
        tipoDocumentoReferencia: document.getElementById('txtTipoDocumentoEditar').value.trim(),
        numeroDocumentoReferencia: document.getElementById('txtNumeroDocumentoEditar').value.trim(),
        observacion: document.getElementById('txtObservacionEditar').value.trim(),
        detalles: obtenerDetallesGuia('tbodyProductosGuiaEditar')
    };
    console.log(request);

    const motivo = motivosGuia.find(item => item.idMotivoGuia == request.idMotivoGuia);

    // Validaciones
    if (request.idTipoOperacion == 0) {
        alert("Seleccione un tipo de operación para la guía");
        return;
    }

    if (!motivo) {
        alert("Seleccione un motivo para la guía");
        return;
    }

    if (motivo.requiereProveedor && request.idProveedor == 0) {
        alert("Seleccione un proveedor");
        return;
    }

    if (motivo.requiereAlmacenOrigen && request.idAlmacenOrigen == 0) {
        alert("Seleccione un proveedor");
        return;
    }

    if (motivo.requiereAlmacenDestino && request.idAlmacenDestino == 0) {
        alert("Seleccione un proveedor");
        return;
    }

    if (motivo.requiereCliente && request.idCliente == 0) {
        alert("Seleccione un proveedor");
        return;
    }

    if (!request.fechaProgramada) {
        alert("Ingrese la fecha programada");
        return;
    }

    if (request.detalles.length === 0) {
        alert("Agregue al menos un producto a la guía");
        return;
    }

    try {
        const response = await fetch(`api/guias/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        const resultado = await response.json();

        if (!response.ok || !resultado.exito) {
            throw new Error(resultado.mensaje || 'No se pudo actualizar la guía');
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalEditarGuia');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Notificar que se actualizo un almacen
        document.dispatchEvent(new CustomEvent('guia:guardado'));

        // alert(resultado.mensaje);

    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// ELIMINAR GUIA
function eliminarGuia(guia) {
    console.log("Eliminar guía: ", guia);

    // confirmacion de eliminar
}