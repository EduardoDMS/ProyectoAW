// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('btnGuardarAlmacen').addEventListener('click', guardarAlmacen);

    document.getElementById('btnActualizarAlmacen').addEventListener('click', actualizarAlmacen);
});

// INICIALIZACION
async function guardarAlmacen() {
    const codigo = document.getElementById('txtCodigo').value.trim();
    const nombre = document.getElementById('txtNombre').value.trim();
    const activo = document.getElementById('chkActivo').checked;

    // Validaciones
    if (!codigo || !nombre) {
        alert("Complete los campos obligatorios");
        return;
    }

    const request = {
        codigo: codigo,
        nombre: nombre,
        activo: activo
    };

    try {
        const response = await fetch('/api/AlmacenApi', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        if (!response.ok) {
            throw new Error("No se pudo crear el almacén");
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalCrearAlmacen');
        const modal = bootstrap.Modal.getInstance(modalElement);

        modal.hide();

        // Limpiar formulario
        limpiarFormularioAlmacen();

        // Notificar que se creo un almacen
        document.dispatchEvent(new CustomEvent('almacen:guardado'));
    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// LIMPIAR FORMULARIO
function limpiarFormularioAlmacen() {
    document.getElementById('txtCodigo').value = "";
    document.getElementById('txtNombre').value = "";
    document.getElementById('chkActivo').checked = true;
}

// EDITAR ALMACEN
async function editarAlmacen(almacen) {
    try {
        // Consultar los datos actualizados por ID
        const response = await fetch(`/api/AlmacenApi/${almacen.idAlmacen}`);

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'No se pudo obtener el almacén');
        }

        if (resultado.exito) {
            const almacen = resultado.datos;

            console.log(almacen);
            console.log(resultado.mensaje);

            // Cargar datos en el formulario
            document.getElementById('txtIdAlmacenEditar').value = almacen.idAlmacen;
            document.getElementById('txtCodigoEditar').value = almacen.codigo;
            document.getElementById('txtNombreEditar').value = almacen.nombre;
            document.getElementById('chkActivoEditar').checked = almacen.activo;

            // Abrir modal
            const modalElement = document.getElementById('modalEditarAlmacen');
            const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
            modal.show();
        }
    } catch (error) {
        console.error('Error: ', error.message);
        alert(error.message);
    }
}

// ACTUALIZAR ALMACEN
async function actualizarAlmacen() {
    const id = document.getElementById('txtIdAlmacenEditar').value;

    const request = {
        codigo: document.getElementById('txtCodigoEditar').value.trim(),
        nombre: document.getElementById('txtNombreEditar').value.trim(),
        activo: document.getElementById('chkActivoEditar').checked
    };

    // Validaciones
    if (!request.codigo || !request.nombre) {
        alert("Complete los campos obligatorios.");
        return;
    }

    try {
        const response = await fetch(`/api/AlmacenApi/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        const resultado = await response.json();

        if (!response.ok || !resultado.exito) {
            throw new Error(resultado.mensaje || 'No se pudo actualizar el almacén');
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalEditarAlmacen');
        const modal = bootstrap.Modal.getInstance(modalElement);

        modal.hide();

        // Notificar que se actualizo un almacen
        document.dispatchEvent(new CustomEvent('almacen:guardado'));

        // alert(resultado.mensaje);

    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

function eliminarAlmacen(almacen) {
    console.log("Eliminar almacen: ", almacen)

    // confirmacion de eliminar
}