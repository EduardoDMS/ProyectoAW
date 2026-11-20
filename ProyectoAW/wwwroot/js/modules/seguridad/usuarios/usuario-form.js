// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('btnGuardarUsuario').addEventListener('click', guardarUsuario);

    document.getElementById('btnActualizarUsuario').addEventListener('click', actualizarUsuario);
});

// GUARDAR USUARIO
async function guardarUsuario() {
    const nombre = document.getElementById('txtNombre').value.trim();
    const apellido = document.getElementById('txtApellido').value.trim();
    const usuario = document.getElementById('txtUsuario').value.trim();
    const contrasena = document.getElementById('txtContrasena').value.trim();
    const idRol = parseInt(document.getElementById('slcRol').value);
    const activo = document.getElementById('chkActivo').checked;

    // Validaciones
    if (!nombre || !apellido || !usuario || !contrasena || idRol == 0) {
        alert("Complete los campos obligatorios");
        return;
    }

    const request = {
        nombre: nombre,
        apellido: apellido,
        username: usuario,
        password: contrasena,
        idRol: idRol,
        activo: activo
    };

    try {
        const response = await fetch('/api/UsuarioApi', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        if (!response.ok) {
            throw new Error("No se pudo crear el usuario");
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalCrearUsuario');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Limpiar formulario
        limpiarFormularioUsuario();

        // Notificar que se creo un almacen
        document.dispatchEvent(new CustomEvent('usuario:guardado'));
    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// LIMPIAR FORMULARIO
function limpiarFormularioUsuario() {
    document.getElementById('txtNombre').value = "";
    document.getElementById('txtApellido').value = "";
    document.getElementById('txtUsuario').value = "";
    document.getElementById('txtContrasena').value = "";
    document.getElementById('slcRol').value = "0";
    document.getElementById('chkActivo').checked = true;
}

// EDITAR USUARIO
async function editarUsuario(usuario) {
    try {
        // Consultar los datos actualizados por ID
        const response = await fetch(`/api/UsuarioApi/${usuario.idUsuario}`);

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'No se pudo obtener el usuario');
        }

        if (resultado.exito) {
            const usuario = resultado.datos;

            console.log(usuario);
            console.log(resultado.mensaje);

            // Cargar datos en el formulario
            document.getElementById('txtIdUsuarioEditar').value = usuario.idUsuario;
            document.getElementById('txtNombreEditar').value = usuario.nombre;
            document.getElementById('txtApellidoEditar').value = usuario.apellido;
            document.getElementById('txtUsuarioEditar').value = usuario.username;
            document.getElementById('txtContrasenaEditar').value = "*******";
            document.getElementById('slcRolEditar').value = String(usuario.idRol);
            document.getElementById('chkActivoEditar').checked = usuario.activo;

            // Abrir modal
            const modalElement = document.getElementById('modalEditarUsuario');
            const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
            modal.show();
        }

    } catch (error) {
        console.error('Error: ', error.message);
        alert(error.message);
    }
}

// ACTUALIZAR USUARIO -- FALTAAA
async function actualizarUsuario() {
    const id = document.getElementById('txtIdUsuarioEditar').value;

    const request = {
        nombre: document.getElementById('txtNombreEditar').value.trim(),
        apellido: document.getElementById('txtApellidoEditar').value.trim(),
        username: document.getElementById('txtUsuarioEditar').value.trim(),
        password: document.getElementById('txtContrasenaEditar').value.trim(),
        idRol: parseInt(document.getElementById('slcRolEditar').value),
        activo: document.getElementById('chkActivoEditar').checked
    };
    console.log(request);
    // Validaciones
    if (!request.nombre || !request.apellido || !request.username || !request.password || request.idRol == 0) {
        alert("Complete los campos obligatorios");
        return;
    }

    try {
        const response = await fetch(`/api/UsuarioApi/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        const resultado = await response.json();

        if (!response.ok || !resultado.exito) {
            throw new Error(resultado.mensaje || 'No se pudo actualizar el usuario');
        }

        // Cerrar modal
        const modalElement = document.getElementById('modalEditarUsuario');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();

        // Notificar que se actualizo un almacen
        document.dispatchEvent(new CustomEvent('usuario:guardado'));

        // alert(resultado.mensaje);

    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

// ELIMINAR USUARIO
function eliminarUsuario(producto) {
    console.log("Eliminar producto: ", producto);

    // confirmacion de eliminar
}