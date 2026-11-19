// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('btnGuardarCliente').addEventListener('click', guardarCliente);

    document.getElementById('btnActualizarCliente').addEventListener('click', actualizarCliente);
});

// GUARDAR CLIENTE
async function guardarCliente() {
    const razonSocial = document.getElementById('txtRazonSocial').value.trim();
    const tipoDocumento = parseInt(document.getElementById('slcTipoDocumento').value);
    const numeroDocumento = document.getElementById('txtNumeroDocumento').value.trim();
    const correo = document.getElementById('txtCorreo').value.trim();
    const telefono = document.getElementById('txtTelefono').value.trim();
    const direccion = document.getElementById('txtDireccion').value.trim();
    const activo = document.getElementById('chkActivo').checked;

    // Validaciones
    if (!razonSocial || tipoDocumento == 0 || !numeroDocumento) {
        alert("Complete los campos obligatorios");
        return;
    }

    const request = {
        razonSocial: razonSocial,
        idTipoDocumento: tipoDocumento,
        numeroDocumento: numeroDocumento,
        correo: correo,
        telefono: telefono,
        direccion: direccion,
        activo:activo
    };

    try {
        const response = await fetch('/api/ClienteApi', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        if (!response.ok) {
            throw new Error("No se pudo crear el cliente");
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalCrearCliente');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Limpiar formulario
        limpiarFormularioCliente();

        // Notificar que se creo un almacen
        document.dispatchEvent(new CustomEvent('cliente:guardado'));
    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// LIMPIAR FORMULARIO
function limpiarFormularioCliente() {
    document.getElementById('txtRazonSocial').value = "";
    document.getElementById('slcTipoDocumento').value = "0";
    document.getElementById('txtNumeroDocumento').value = "";
    document.getElementById('txtCorreo').value = "";
    document.getElementById('txtTelefono').value = "";
    document.getElementById('txtDireccion').value = "";
    document.getElementById('chkActivo').checked = true;
}

// EDITAR CLIENTE
async function editarCliente(cliente) {
    try {
        // Consultar los datos actualizados por ID
        const response = await fetch(`/api/ClienteApi/${cliente.idCliente}`);

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'No se pudo obtener el cliente');
        }

        if (resultado.exito) {
            const cliente = resultado.datos;

            console.log(cliente);
            console.log(resultado.mensaje);

            // Cargar datos en el formulario
            document.getElementById('txtIdClienteEditar').value = cliente.idCliente;
            document.getElementById('txtRazonSocialEditar').value = cliente.razonSocial;
            document.getElementById('slcTipoDocumentoEditar').value = String(cliente.idTipoDocumento);
            document.getElementById('txtNumeroDocumentoEditar').value = cliente.numeroDocumento;
            document.getElementById('txtCorreoEditar').value = cliente.correo;
            document.getElementById('txtTelefonoEditar').value = cliente.telefono;
            document.getElementById('txtDireccionEditar').value = cliente.direccion;
            document.getElementById('chkActivoEditar').checked = cliente.activo;

            // Abrir modal
            const modalElement = document.getElementById('modalEditarCliente');
            const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
            modal.show();
        }

    } catch (error) {
        console.error('Error: ', error.message);
        alert(error.message);
    }
}

// ACTUALIZAR CLIENTE
async function actualizarCliente() {
    const id = document.getElementById('txtIdClienteEditar').value;

    const request = {
        razonSocial: document.getElementById('txtRazonSocialEditar').value.trim(),
        idTipoDocumento: parseInt(document.getElementById('slcTipoDocumentoEditar').value),
        numeroDocumento: document.getElementById('txtNumeroDocumentoEditar').value.trim(),
        correo: document.getElementById('txtCorreoEditar').value.trim(),
        telefono: document.getElementById('txtTelefonoEditar').value.trim(),
        direccion: document.getElementById('txtDireccionEditar').value.trim(),
        activo: document.getElementById('chkActivoEditar').checked
    };

    // Validaciones
    if (!request.razonSocial || request.tipoDocumento == 0 || !request.numeroDocumento) {
        alert("Complete los campos obligatorios.");
        return;
    }

    try {
        const response = await fetch(`/api/ClienteApi/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        const resultado = await response.json();

        if (!response.ok || !resultado.exito) {
            throw new Error(resultado.mensaje || 'No se pudo actualizar el cliente');
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalEditarCliente');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Notificar que se actualizo un almacen
        document.dispatchEvent(new CustomEvent('cliente:guardado'));

        // alert(resultado.mensaje);

    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// ELIMINAR CLIENTE
function eliminarCliente(cliente) {
    console.log("Eliminar cliente: ", cliente);

    // confirmacion de eliminar
}