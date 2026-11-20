// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('btnGuardarProveedor').addEventListener('click', guardarProveedor);

    document.getElementById('btnActualizarProveedor').addEventListener('click', actualizarProveedor);
});

// GUARDAR CLIENTE
async function guardarProveedor() {
    const razonSocial = document.getElementById('txtRazonSocial').value.trim();
    const tipoDocumento = parseInt(document.getElementById('slcTipoDocumento').value);
    const numeroDocumento = document.getElementById('txtNumeroDocumento').value.trim();
    const direccion = document.getElementById('txtDireccion').value.trim();
    const nombreContacto = document.getElementById('txtNombreContacto').value.trim();
    const correoContacto = document.getElementById('txtCorreoContacto').value.trim();
    const telefonoContacto = document.getElementById('txtTelefonoContacto').value.trim();
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
        direccion: direccion,
        nombreContacto: nombreContacto,
        correoContacto: correoContacto,
        telefonoContacto: telefonoContacto,
        activo: activo
    };

    try {
        const response = await fetch('/api/ProveedorApi', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        if (!response.ok) {
            throw new Error("No se pudo crear el proveedor");
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalCrearProveedor');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Limpiar formulario
        limpiarFormularioProveedor();

        // Notificar que se creo un almacen
        document.dispatchEvent(new CustomEvent('proveedor:guardado'));
    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// LIMPIAR FORMULARIO
function limpiarFormularioProveedor() {
    document.getElementById('txtRazonSocial').value = "";
    document.getElementById('slcTipoDocumento').value = "0";
    document.getElementById('txtNumeroDocumento').value = "";
    document.getElementById('txtDireccion').value = "";
    document.getElementById('txtNombreContacto').value = "";
    document.getElementById('txtCorreoContacto').value = "";
    document.getElementById('txtTelefonoContacto').value = "";  
    document.getElementById('chkActivo').checked = true;
}

// EDITAR PROVEEDOR
async function editarProveedor(proveedor) {
    try {
        // Consultar los datos actualizados por ID
        const response = await fetch(`/api/ProveedorApi/${proveedor.idProveedor}`);

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'No se pudo obtener el proveedor');
        }

        if (resultado.exito) {
            const proveedor = resultado.datos;

            console.log(proveedor);
            console.log(resultado.mensaje);

            // Cargar datos en el formulario
            document.getElementById('txtIdProveedorEditar').value = proveedor.idProveedor;
            document.getElementById('txtRazonSocialEditar').value = proveedor.razonSocial;
            document.getElementById('slcTipoDocumentoEditar').value = String(proveedor.idTipoDocumento);
            document.getElementById('txtNumeroDocumentoEditar').value = proveedor.numeroDocumento;
            document.getElementById('txtDireccionEditar').value = proveedor.direccion;
            document.getElementById('txtNombreContactoEditar').value = proveedor.nombreContacto;
            document.getElementById('txtCorreoContactoEditar').value = proveedor.correoContacto;
            document.getElementById('txtTelefonoContactoEditar').value = proveedor.telefonoContacto;      
            document.getElementById('chkActivoEditar').checked = proveedor.activo;

            // Abrir modal
            const modalElement = document.getElementById('modalEditarProveedor');
            const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
            modal.show();
        }

    } catch (error) {
        console.error('Error: ', error.message);
        alert(error.message);
    }
}

// ACTUALIZAR CLIENTE
async function actualizarProveedor() {
    const id = document.getElementById('txtIdProveedorEditar').value;

    const request = {
        razonSocial: document.getElementById('txtRazonSocialEditar').value.trim(),
        idTipoDocumento: parseInt(document.getElementById('slcTipoDocumentoEditar').value),
        numeroDocumento: document.getElementById('txtNumeroDocumentoEditar').value.trim(),
        direccion: document.getElementById('txtDireccionEditar').value.trim(),
        nombreContacto: document.getElementById('txtNombreContactoEditar').value.trim(),
        correoContacto: document.getElementById('txtCorreoContactoEditar').value.trim(),
        telefonoContacto: document.getElementById('txtTelefonoContactoEditar').value.trim(),       
        activo: document.getElementById('chkActivoEditar').checked
    };

    // Validaciones
    if (!request.razonSocial || request.tipoDocumento == 0 || !request.numeroDocumento) {
        alert("Complete los campos obligatorios.");
        return;
    }

    try {
        const response = await fetch(`/api/ProveedorApi/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        const resultado = await response.json();

        if (!response.ok || !resultado.exito) {
            throw new Error(resultado.mensaje || 'No se pudo actualizar el proveedor');
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalEditarProveedor');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Notificar que se actualizo un almacen
        document.dispatchEvent(new CustomEvent('proveedor:guardado'));

        // alert(resultado.mensaje);

    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// ELIMINAR CLIENTE
function eliminarProveedor(proveedor) {
    console.log("Eliminar proveedor: ", proveedor);

    // confirmacion de eliminar
}