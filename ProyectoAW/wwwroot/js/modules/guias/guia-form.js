// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('btnGuardarGuia').addEventListener('click', guardarGuia);

    //document.getElementById('btnActualizarGuia').addEventListener('click', actualizarGuia);
});

// GUARDAR GUIA
async function guardarGuia() {
    const idTipoOperacion = parseInt(document.getElementById('slcOperación').value);
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

    const detalles = obtenerDetallesGuia();

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
function obtenerDetallesGuia() {
    const detalles = [];

    document.querySelectorAll('#tbodyProductosGuia tr').forEach(fila => {
        const idProducto = Number(fila.querySelector('.slcProducto').value);
        const cantidad = Number(fila.querySelector('.txtCantidad').value);

        detalles.push({
            idProducto: idProducto,
            cantidad: cantidad
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