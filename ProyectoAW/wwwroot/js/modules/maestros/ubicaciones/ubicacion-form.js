// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('btnGuardarUbicacion').addEventListener('click', guardarUbicacion);

    document.getElementById('btnActualizarUbicacion').addEventListener('click', actualizarUbicacion);
});

// INICIALIZACION
async function guardarUbicacion() {
    const codigo = document.getElementById('txtCodigo').value.trim();
    const nombre = document.getElementById('txtNombre').value.trim();
    const idAlmacen = parseInt(document.getElementById('slcAlmacen').value);
    const activo = document.getElementById('chkActivo').checked;

    console.log(idAlmacen);
    // Validaciones
    if (!codigo || !nombre || idAlmacen == 0) {
        alert("Complete los campos obligatorios");
        return;
    }

    const request = {
        codigo: codigo,
        nombre: nombre,
        idAlmacen: idAlmacen,
        activo: activo
    };

    try {
        const response = await fetch('/api/UbicacionApi', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        if (!response.ok) {
            throw new Error("No se pudo crear la ubicación");
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalCrearUbicacion');
        const modal = bootstrap.Modal.getInstance(modalElement);

        modal.hide();

        // Limpiar formulario
        limpiarFormularioUbicacion();

        // Notificar que se creo un almacen
        document.dispatchEvent(new CustomEvent('ubicacion:guardado'));
    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// LIMPIAR FORMULARIO
function limpiarFormularioUbicacion() {
    document.getElementById('txtCodigo').value = "";
    document.getElementById('txtNombre').value = "";
    document.getElementById('slcAlmacen').value = "0";
    document.getElementById('chkActivo').checked = true;
}

// EDITAR UBICACION
async function editarUbicacion(ubicacion) {
    try {
        // Consultar los datos actualizados por ID
        const response = await fetch(`/api/UbicacionApi/${ubicacion.idUbicacion}`);

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'No se pudo obtener la ubicación');
        }

        if (resultado.exito) {
            const ubicacion = resultado.datos;

            console.log(ubicacion);
            console.log(resultado.mensaje);

            // Cargar datos en el formulario
            document.getElementById('txtIdUbicacionEditar').value = ubicacion.idUbicacion;
            document.getElementById('txtCodigoEditar').value = ubicacion.codigo;
            document.getElementById('txtNombreEditar').value = ubicacion.nombre;
            document.getElementById('slcAlmacenEditar').value = String(ubicacion.idAlmacen);
            document.getElementById('chkActivoEditar').value = ubicacion.activo;

            // Abrir modal
            const modalElement = document.getElementById('modalEditarUbicacion');
            const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
            modal.show();
        }
    } catch (error) {
        console.error('Error: ', error.message);
        alert(error.message);
    }
}

// ACTUALIZAR UBICACION
async function actualizarUbicacion() {
    const id = document.getElementById('txtIdUbicacionEditar').value;

    const request = {
        codigo: document.getElementById('txtCodigoEditar').value.trim(),
        nombre: document.getElementById('txtNombreEditar').value.trim(),
        idAlmacen: parseInt(document.getElementById('slcAlmacenEditar').value),
        activo: document.getElementById('chkActivoEditar').checked
    };

    // Validaciones
    if (!request.codigo || !request.nombre || request.idAlmacen == 0) {
        alert("Complete los campos obligatorios");
        return;
    }

    try {
        const response = await fetch(`/api/UbicacionApi/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        const resultado = await response.json();

        if (!response.ok || !resultado.exito) {
            throw new Error(resultado.mensaje || 'No se pudo actualizar la ubicación');
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalEditarUbicacion');
        const modal = bootstrap.Modal.getInstance(modalElement);

        modal.hide();

        // Notificar que se actualizo una ubicacion
        document.dispatchEvent(new CustomEvent('ubicacion:guardado'));

    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}
function eliminarUbicacion(ubicacion) {
    console.log("Eliminar almacen: ", ubicacion);

    // confirmacion de eliminar
}