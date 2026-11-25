// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('btnGuardarMotivoGuia').addEventListener('click', guardarMotivoGuia);

    document.getElementById('btnActualizarMotivoGuia').addEventListener('click', actualizarMotivoGuia);
});

// GUARDAR MOTIVO
async function guardarMotivoGuia() {
    const nombre = document.getElementById('txtNombre').value.trim();
    const idTipoOperacion = parseInt(document.getElementById('slcOperacion').value);
    const idTipoComportamiento = parseInt(document.getElementById('slcComportamiento').value);
    const requiereProveedor = document.getElementById('requiereProveedor').checked;
    const requiereAlmacenOrigen = document.getElementById('requiereAlmacenOrigen').checked;
    const requiereAlmacenDestino = document.getElementById('requiereAlmacenDestino').checked;
    const requiereCliente = document.getElementById('requiereCliente').checked;
    const activo = document.getElementById('chkActivo').checked;

    // Validaciones
    if (!nombre || idTipoOperacion == 0 || idTipoComportamiento == 0) {
        alert("Complete los campos obligatorios");
        return;
    }

    const request = {
        nombreMotivo: nombre,
        idTipoOperacionGuia: idTipoOperacion,
        idTipoComportamiento: idTipoComportamiento,
        requiereProveedor: requiereProveedor,
        requiereAlmacenOrigen: requiereAlmacenOrigen,
        requiereAlmacenDestino: requiereAlmacenDestino,
        requiereCliente: requiereCliente,
        activo: activo
    };

    try {
        const response = await fetch('/api/MotivoGuiaApi', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        if (!response.ok) {
            throw new Error("No se pudo crear el motivo");
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalCrearMotivoGuia');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Limpiar formulario
        limpiarFormularioMotivo();

        document.dispatchEvent(new CustomEvent('motivo:guardado'));
    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// LIMPIAR FORMULARIO
function limpiarFormularioMotivo() {
    document.getElementById('txtNombre').value = "";
    document.getElementById('slcOperacion').value = "0";
    document.getElementById('slcComportamiento').value = "0";
    document.getElementById('requiereProveedor').checked = false;
    document.getElementById('requiereAlmacenOrigen').checked = false;
    document.getElementById('requiereAlmacenDestino').checked = false;
    document.getElementById('requiereCliente').checked = false;
    document.getElementById('chkActivo').checked = true;
}

// EDITAR MOTIVO
async function editarMotivoGuia(motivo) {
    try {
        // Consultar los datos actualizados por ID
        const response = await fetch(`/api/MotivoGuiaApi/${motivo.idMotivoGuia}`);

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'No se pudo obtener el motivo');
        }

        if (resultado.exito) {
            const motivo = resultado.datos;

            console.log(motivo);
            console.log(resultado.mensaje);

            // Cargar datos en el formulario
            document.getElementById('txtIdMotivoGuia').value=motivo.idMotivoGuia
            document.getElementById('txtNombreEditar').value = motivo.nombreMotivo;
            document.getElementById('slcOperacionEditar').value = String(motivo.idTipoOperacionGuia);
            document.getElementById('slcComportamientoEditar').value = String(motivo.idTipoComportamiento);
            document.getElementById('requiereProveedorEditar').checked = motivo.requiereProveedor;
            document.getElementById('requiereAlmacenDestinoEditar').checked = motivo.requiereAlmacenDestino;
            document.getElementById('requiereAlmacenOrigenEditar').checked = motivo.requiereAlmacenOrigen;
            document.getElementById('requiereClienteEditar').checked = motivo.requiereCliente;
            document.getElementById('chkActivoEditar').checked = motivo.activo;

            // Abrir modal
            const modalElement = document.getElementById('modalEditarMotivoGuia');
            const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
            modal.show();
        }

    } catch (error) {
        console.error('Error: ', error.message);
        alert(error.message);
    }
}

// ACTUALIZAR MOTIVO
async function actualizarMotivoGuia() {
    const id = document.getElementById('txtIdMotivoGuia').value;

    const request = {
        nombreMotivo: document.getElementById('txtNombreEditar').value.trim(),
        idTipoOperacionGuia: parseInt(document.getElementById('slcOperacionEditar').value),
        idTipoComportamiento: parseInt(document.getElementById('slcComportamientoEditar').value),
        requiereProveedor: document.getElementById('requiereProveedorEditar').checked,
        requiereAlmacenOrigen: document.getElementById('requiereAlmacenOrigenEditar').checked,
        requiereAlmacenDestino: document.getElementById('requiereAlmacenDestinoEditar').checked,
        requiereCliente: document.getElementById('requiereClienteEditar').checked,
        activo: document.getElementById('chkActivoEditar').checked
    };

    // Validaciones
    if (!request.nombreMotivo || request.idTipoOperacionGuia == 0 || request.idTipoComportamiento == 0) {
        alert("Complete los campos obligatorios");
        return;
    }

    try {
        const response = await fetch(`/api/MotivoGuiaApi/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        const resultado = await response.json();

        if (!response.ok || !resultado.exito) {
            throw new Error(resultado.mensaje || 'No se pudo actualizar el motivo');
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalEditarMotivoGuia');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Notificar que se actualizo un almacen
        document.dispatchEvent(new CustomEvent('motivo:guardado'));

        // alert(resultado.mensaje);

    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// ELIMINAR MOTIVO
function eliminarMotivo(motivo) {
    console.log("Eliminar motivo: ", motivo);

    // confirmacion de eliminar
}