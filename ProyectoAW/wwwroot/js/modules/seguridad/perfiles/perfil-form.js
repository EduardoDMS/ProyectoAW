// INICIALIZACION
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('btnGuardarPerfil').addEventListener('click', guardarPerfil);
    document.getElementById('btnActualizarPerfil').addEventListener('click', actualizarPerfil);

    // Al cambiar de rol se vuelve a pedir la lista de modulos que ese rol puede tener
    document.getElementById('slcRol').addEventListener('change', () =>
        cambiarRol('slcRol', 'tbodyPermisos'));
    document.getElementById('slcRolEditar').addEventListener('change', () =>
        cambiarRol('slcRolEditar', 'tbodyPermisosEditar'));

    // Casilla de cabecera: marca o desmarca toda la columna de esa tabla
    document.addEventListener('change', (e) => {
        if (!e.target.matches('.chk-columna')) return;

        const tabla = e.target.closest('table');
        tabla.querySelectorAll(`tbody .chk-permiso[data-permiso="${e.target.dataset.permiso}"]`)
            .forEach(chk => chk.checked = e.target.checked);
    });

    // Al cerrar el modal de crear se limpia el formulario
    document.getElementById('modalCrearPerfil')
        .addEventListener('hidden.bs.modal', limpiarFormularioPerfil);
});


// ======================== GRILLA DE PERMISOS ========================

function escaparHtml(texto) {
    const div = document.createElement('div');
    div.textContent = texto;
    return div.innerHTML;
}

// Dibuja una fila por modulo. "marcados" es un Map(modulo en minusculas -> permiso) para precargar casillas.
function renderGrilla(idTbody, modulos, marcados = new Map()) {
    const tbody = document.getElementById(idTbody);

    if (!modulos.length) {
        tbody.innerHTML = '<tr><td colspan="5" class="text-muted py-3">Selecciona un rol para ver los módulos disponibles.</td></tr>';
        return;
    }

    tbody.innerHTML = modulos.map(modulo => {
        const actual = marcados.get(modulo.toLowerCase()) || {};

        const celdas = PERMISOS.map(permiso => `
            <td>
                <input type="checkbox" class="form-check-input chk-permiso"
                       data-permiso="${permiso}" ${actual[permiso] ? 'checked' : ''} />
            </td>`).join('');

        return `<tr data-modulo="${escaparHtml(modulo)}">
                    <td class="text-start">${escaparHtml(modulo)}</td>${celdas}
                </tr>`;
    }).join('');
}

// Lee la grilla y devuelve solo los modulos con al menos un permiso marcado
function leerGrilla(idTbody) {
    return [...document.querySelectorAll(`#${idTbody} tr[data-modulo]`)]
        .map(fila => {
            const permiso = { modulo: fila.dataset.modulo };
            PERMISOS.forEach(p => {
                permiso[p] = fila.querySelector(`.chk-permiso[data-permiso="${p}"]`).checked;
            });
            return permiso;
        })
        .filter(permiso => PERMISOS.some(p => permiso[p]));
}

// Pide los modulos del rol y dibuja la grilla
async function cargarGrilla(idRol, idTbody, marcados = new Map()) {
    const modulos = idRol > 0
        ? await llamarApi(`${URL_PERFILES}/modulos?idRol=${idRol}`)
        : [];

    renderGrilla(idTbody, modulos, marcados);
}

// Cambio de rol: se conservan las casillas de los modulos que sigan existiendo en el nuevo rol
async function cambiarRol(idSelect, idTbody) {
    const idRol = parseInt(document.getElementById(idSelect).value);

    const marcados = new Map(leerGrilla(idTbody).map(p => [p.modulo.toLowerCase(), p]));

    try {
        await cargarGrilla(idRol, idTbody, marcados);
    } catch (error) {
        console.error('Error: ', error.message);
        alert(error.message);
    }
}


// ======================== GUARDAR ========================

async function guardarPerfil() {
    const nombrePerfil = document.getElementById('txtNombre').value.trim();
    const idRol = parseInt(document.getElementById('slcRol').value);
    const permisos = leerGrilla('tbodyPermisos');

    // Validaciones
    if (!nombrePerfil || idRol === 0) {
        alert('Complete los campos obligatorios');
        return;
    }

    if (permisos.length === 0) {
        alert('Marque al menos un permiso');
        return;
    }

    const request = { nombrePerfil, idRol, permisos };

    try {
        await llamarApi(URL_PERFILES, {
            method: 'POST',
            body: JSON.stringify(request)
        });

        // Cerrar modal (el formulario se limpia en el evento hidden.bs.modal)
        bootstrap.Modal.getInstance(document.getElementById('modalCrearPerfil')).hide();

        // Notificar que se creo un perfil
        document.dispatchEvent(new CustomEvent('perfil:guardado'));
    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}

function limpiarFormularioPerfil() {
    document.getElementById('txtNombre').value = '';
    document.getElementById('slcRol').value = '0';
    renderGrilla('tbodyPermisos', []);
}


// ======================== EDITAR ========================

async function editarPerfil(fila) {
    try {
        await rolesListos;   // los selects de rol deben estar llenos antes de asignarles un valor

        // Consultar los datos actualizados por ID
        const perfil = await llamarApi(`${URL_PERFILES}/${fila.idPerfil}`);

        document.getElementById('txtIdPerfilEditar').value = perfil.idPerfil;
        document.getElementById('txtNombreEditar').value = perfil.nombrePerfil;
        document.getElementById('slcRolEditar').value = String(perfil.idRol);
        document.getElementById('chkActivoEditar').checked = perfil.activo;

        // Grilla del rol con los permisos actuales del perfil marcados
        const marcados = new Map(perfil.permisos.map(p => [p.modulo.toLowerCase(), p]));
        await cargarGrilla(perfil.idRol, 'tbodyPermisosEditar', marcados);

        // Abrir modal
        bootstrap.Modal.getOrCreateInstance(document.getElementById('modalEditarPerfil')).show();
    } catch (error) {
        console.error('Error: ', error.message);
        alert(error.message);
    }
}

async function actualizarPerfil() {
    const id = document.getElementById('txtIdPerfilEditar').value;

    const request = {
        nombrePerfil: document.getElementById('txtNombreEditar').value.trim(),
        idRol: parseInt(document.getElementById('slcRolEditar').value),
        activo: document.getElementById('chkActivoEditar').checked,
        permisos: leerGrilla('tbodyPermisosEditar')
    };

    // Validaciones
    if (!request.nombrePerfil || request.idRol === 0) {
        alert('Complete los campos obligatorios.');
        return;
    }

    if (request.permisos.length === 0) {
        alert('Marque al menos un permiso');
        return;
    }

    try {
        await llamarApi(`${URL_PERFILES}/${id}`, {
            method: 'PUT',
            body: JSON.stringify(request)
        });

        bootstrap.Modal.getInstance(document.getElementById('modalEditarPerfil')).hide();

        document.dispatchEvent(new CustomEvent('perfil:guardado'));
    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}


// ======================== ELIMINAR ========================

// El servidor rechaza el borrado si hay usuarios con este perfil y devuelve el mensaje, que se muestra tal cual
async function eliminarPerfil(perfil) {
    if (!confirm(`¿Eliminar el perfil "${perfil.nombrePerfil}"?`)) return;

    try {
        await llamarApi(`${URL_PERFILES}/${perfil.idPerfil}`, { method: 'DELETE' });

        document.dispatchEvent(new CustomEvent('perfil:guardado'));
    } catch (error) {
        console.error(error);
        alert(error.message);
    }
}