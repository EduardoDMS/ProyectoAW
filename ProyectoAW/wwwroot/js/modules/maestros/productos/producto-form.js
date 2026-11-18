// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('btnGuardarProducto').addEventListener('click', guardarProducto);

    document.getElementById('btnActualizarProducto').addEventListener('click', actualizarProducto);
});

// GUARDAR PRODUCTO
async function guardarProducto() {
    const codigo = document.getElementById('txtCodigo').value.trim();
    const nombre = document.getElementById('txtNombre').value.trim();
    const idCategoria = parseInt(document.getElementById('slcCategoria').value);
    const idUnidadMedida = parseInt(document.getElementById('slcUnidadMedida').value);
    const lote = document.getElementById('txtLote').value.trim();
    const serie = document.getElementById('txtSerie').value.trim();
    const activo = document.getElementById('chkActivo').checked;

    // Validaciones
    if (!codigo || !nombre || idCategoria == 0 || idUnidadMedida == 0) {
        alert("Complete los campos obligatorios");
        return;
    }

    const request = {
        codigo: codigo,
        nombre: nombre,
        idCategoria: idCategoria,
        idUnidadMedida: idUnidadMedida,
        lote: lote,
        serie:serie,
        activo: activo
    };

    try {
        const response = await fetch('/api/ProductoApi', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        if (!response.ok) {
            throw new Error("No se pudo crear el producto");
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalCrearProducto');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Limpiar formulario
        limpiarFormularioProducto();

        // Notificar que se creo un almacen
        document.dispatchEvent(new CustomEvent('producto:guardado'));
    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// LIMPIAR FORMULARIO
function limpiarFormularioProducto() {
    document.getElementById('txtCodigo').value = "";
    document.getElementById('txtNombre').value = "";
    document.getElementById('slcCategoria').value = "0";
    document.getElementById('slcUnidadMedida').value = "0";
    document.getElementById('txtLote').value = "";
    document.getElementById('txtSerie').value = "";
    document.getElementById('chkActivo').checked = true;
}

// EDITAR PRODUCTO
async function editarProducto(producto) {
    try {
        // Consultar los datos actualizados por ID
        const response = await fetch(`/api/ProductoApi/${producto.idProducto}`);

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'No se pudo obtener el producto');
        }

        if (resultado.exito) {
            const producto = resultado.datos;

            console.log(producto);
            console.log(resultado.mensaje);

            // Cargar datos en el formulario
            document.getElementById('txtIdProductoEditar').value = producto.idProducto;
            document.getElementById('txtCodigoEditar').value = producto.codigo;
            document.getElementById('txtNombreEditar').value = producto.nombre;
            document.getElementById('slcCategoriaEditar').value = String(producto.idCategoria);
            document.getElementById('slcUnidadMedidaEditar').value = String(producto.idUnidadMedida);
            document.getElementById('chkActivoEditar').checked = producto.activo;

            // Abrir modal
            const modalElement = document.getElementById('modalEditarProducto');
            const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
            modal.show();
        }

    } catch (error) {
        console.error('Error: ', error.message);
        alert(error.message);
    }
}

// ACTUALIZAR PRODUCTO
async function actualizarProducto() {
    const id = document.getElementById('txtIdProductoEditar').value;

    const request = {
        codigo: document.getElementById('txtCodigoEditar').value.trim(),
        nombre: document.getElementById('txtNombreEditar').value.trim(),
        idCategoria: parseInt(document.getElementById('slcCategoriaEditar').value),
        idUnidadMedida: parseInt(document.getElementById('slcUnidadMedidaEditar').value),
        activo: document.getElementById('chkActivoEditar').checked
    };

    // Validaciones
    if (!request.codigo || !request.nombre || request.idCategoria == 0 || request.idUnidadMedida == 0) {
        alert("Complete los campos obligatorios.");
        return;
    }

    try {
        const response = await fetch(`/api/ProductoApi/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        const resultado = await response.json();

        if (!response.ok || !resultado.exito) {
            throw new Error(resultado.mensaje || 'No se pudo actualizar el producto');
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalEditarProducto');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Notificar que se actualizo un almacen
        document.dispatchEvent(new CustomEvent('producto:guardado'));

        // alert(resultado.mensaje);

    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// ELIMINAR PRODUCTO
function eliminarProducto(producto) {
    console.log("Eliminar producto: ", producto);

    // confirmacion de eliminar
}