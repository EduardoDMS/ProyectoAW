// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('btnGuardarGuia').addEventListener('click', guardarGuia);

    //document.getElementById('btnActualizarGuia').addEventListener('click', actualizarGuia);
});

// GUARDAR GUIA
async function guardarGuia() {
    const idTipoOperacion = parseInt(document.getElementById('slcOperación').value);
    const idMotivoGuia = parseInt(document.getElementById('slcMotivoGuia').value);

    const idProveedor = parseInt(document.getElementById('slcProveedor').value);
    const idAlmacenOrigen = parseInt(document.getElementById('slcAlmacenOrigen').value);
    const idAlmacenDestino = parseInt(document.getElementById('slcAlmacenDestino').value);
    const idCliente = parseInt(document.getElementById('slcCliente').value);

    const fechaProgramada = document.getElementById('txtFechaProgramada').value;

    const tipoDocumento = document.getElementById('txtTipoDocumento').value.trim();
    const numeroDocumento = document.getElementById('txtNumeroDocumento').value.trim();

    const observacion = document.getElementById('txtObservacion').value.trim();

    const detalles = obtenerDetallesGuia();

    // Validaciones
    if (!idTipoOperacion == 0 || !idMotivoGuia == 0 || !idProveedor == 0 || !idAlmacenOrigen == 0 || !idAlmacenDestino == 0 || !idCliente == 0) {
        alert("Complete los campos obligatorios");
        return;
    }

    const request = {
        idTipoOperacion: nombre,
        idMotivoGuia: idTipoOperacion,
        idProveedor: idTipoComportamiento,
        idAlmacenOrigen: requiereProveedor,
        idAlmacenDestino: requiereAlmacenOrigen,
        idCliente: requiereAlmacenDestino,
        fechaProgramada: requiereCliente,
        tipoDocumento: activo,
        numeroDocumento: numeroDocumento,
        observacion: observacion,
        // Productos
    };

    try {
        const response = await fetch('/api/GuiaApi', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        if (!response.ok) {
            throw new Error("No se pudo crear la guia");
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
function obtenerDetallesGuia() {
    const detalles = [];

    document.querySelectorAll('#tbodyProductosGuia tr').forEach(fila => {
        const idProducto = Number(fila.querySelector('#slcProducto').value);
        const cantidad = Number(fila.querySelector('#txtCantidad').value);

        detalles.push({
            idProducto=idProducto,
            cantidad=cantidad
        });
    });

    return detalles;
}

// LIMPIAR FORMULARIO
function limpiarFormularioGuia() {

}
// EDITAR GUIA

// ACTUALIZAR GUIA

// ELIMINAR GUIA