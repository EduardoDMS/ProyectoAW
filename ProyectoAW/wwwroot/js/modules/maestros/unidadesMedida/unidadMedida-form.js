// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('btnGuardarUnidadMedida').addEventListener('click', guardarUnidadMedida);

    document.getElementById('btnActualizarUnidadMedida').addEventListener('click', actualizarUnidadMedida);
});

// GUARDAR UNIDAD DE MEDIDA
async function guardarUnidadMedida() {
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
        const response = await fetch('/api/UnidadMedidaApi', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        if (!response.ok) {
            throw new Error("No se pudo crear la unidad de medida");
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalCrearUnidadMedida');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Limpiar formulario
        limpiarFormularioUnidadMedida();

        // Notificar que se creo un almacen
        document.dispatchEvent(new CustomEvent('unidadMedida:guardado'));

    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// LIMPIAR FORMULARIO
function limpiarFormularioUnidadMedida() {
    document.getElementById('txtCodigo').value = "";
    document.getElementById('txtNombre').value = "";
    document.getElementById('chkActivo').checked = true;
}

// EDITAR UNIDAD DE MEDIDA
async function editarUnidadMedida(unidadMedida) {
    try {
        // Consultar los datos actualizados por ID
        const response = await fetch(`/api/UnidadMedidaApi/${unidadMedida.idUnidadMedida}`);

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'No se pudo obtener la unidad de medida');
        }

        if (resultado.exito) {
            const unidadMedida = resultado.datos;

            console.log(unidadMedida);
            console.log(resultado.mensaje);

            // Cargar datos en el formulario
            document.getElementById('txtIdUnidadMedidaEditar').value = unidadMedida.idUnidadMedida;
            document.getElementById('txtCodigoEditar').value = unidadMedida.codigo;
            document.getElementById('txtNombreEditar').value = unidadMedida.nombre;
            document.getElementById('chkActivoEditar').checked = unidadMedida.activo;

            // Abrir modal
            const modalElement = document.getElementById('modalEditarUnidadMedida');
            const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
            modal.show();
        }

    } catch (error) {
        console.error('Error: ', error.message);
        alert(error.message);
    }
}

// ACTUALIZAR UNIDAD DE MEDIDA
async function actualizarUnidadMedida() {
    const id = document.getElementById('txtIdUnidadMedidaEditar').value;

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
        const response = await fetch(`/api/UnidadMedidaApi/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        const resultado = await response.json();

        if (!response.ok || !resultado.exito) {
            throw new Error(resultado.mensaje || 'No se pudo actualizar la unidad de medida');
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalEditarUnidadMedida');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Notificar que se actualizo un almacen
        document.dispatchEvent(new CustomEvent('unidadMedida:guardado'));

        // alert(resultado.mensaje);

    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// ELIMINAR UNIDAD DE MEDIDA
function eliminarUnidadMedida(unidadMedida) {
    console.log("Eliminar unidadMedida: ", unidadMedida);

    // confirmacion de eliminar
}