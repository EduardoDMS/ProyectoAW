// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('btnGuardarCategoria').addEventListener('click', guardarCategoria);

    document.getElementById('btnActualizarCategoria').addEventListener('click', actualizarCategoria);
});

// GUARDAR CATEGORIA
async function guardarCategoria() {
    const nombre = document.getElementById('txtNombre').value.trim();
    const descripcion = document.getElementById('txtDescripcion').value.trim();
    const activo = document.getElementById('chkActivo').checked;

    // Validaciones
    if (!nombre || !descripcion) {
        alert("Complete los campos obligatorios");
        return;
    }

    const request = {
        nombre: nombre,
        descripcion: descripcion,
        activo:activo
    };

    try {
        const response = await fetch('/api/CategoriaApi', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        if (!response.ok) {
            throw new Error("No se pudo crear la categoría");
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalCrearCategoria');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Limpiar formulario
        limpiarFormularioCategoria();

        // Notificar que se creo un almacen
        document.dispatchEvent(new CustomEvent('categoria:guardado'));

    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// LIMPIAR FORMULARIO
function limpiarFormularioCategoria() {
    document.getElementById('txtNombre').value = "";
    document.getElementById('txtDescripcion').value = "";
    document.getElementById('chkActivo').checked = true;
}

// EDITAR CATEGORIA
async function editarCategoria(categoria) {
    try {
        // Consultar los datos actualizados por ID
        const response = await fetch(`/api/CategoriaApi/${categoria.idCategoria}`);

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'No se pudo obtener la categoría');
        }

        if (resultado.exito) {
            const categoria = resultado.datos;

            console.log(categoria);
            console.log(resultado.mensaje);

            // Cargar datos en el formulario
            document.getElementById('txtIdCategoriaEditar').value = categoria.idCategoria;
            document.getElementById('txtNombreEditar').value = categoria.nombre;
            document.getElementById('txtDescripcionEditar').value = categoria.descripcion;
            document.getElementById('chkActivoEditar').checked = categoria.activo;

            // Abrir modal
            const modalElement = document.getElementById('modalEditarCategoria');
            const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
            modal.show();
        }

    } catch (error) {
        console.error('Error: ', error.message);
        alert(error.message);
    }
}

// ACTUALIZAR CATEGORIA
async function actualizarCategoria() {
    const id = document.getElementById('txtIdCategoriaEditar').value;

    const request = {
        nombre: document.getElementById('txtNombreEditar').value.trim(),
        descripcion: document.getElementById('txtDescripcionEditar').value.trim(),
        activo: document.getElementById('chkActivoEditar').checked
    };

    // Validaciones
    if (!request.nombre || !request.descripcion) {
        alert("Complete los campos obligatorios.");
        return;
    }

    try {
        const response = await fetch(`/api/CategoriaApi/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        const resultado = await response.json();

        if (!response.ok || !resultado.exito) {
            throw new Error(resultado.mensaje || 'No se pudo actualizar la categoría');
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalEditarCategoria');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Notificar que se actualizo un almacen
        document.dispatchEvent(new CustomEvent('categoria:guardado'));

        // alert(resultado.mensaje);

    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// ELIMINAR CATEGORIA
function eliminarCategoria(categoria) {
    console.log("Eliminar categoria: ", categoria);
}