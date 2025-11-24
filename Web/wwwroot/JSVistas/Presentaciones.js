window.Componente = {
    UrlControlador: "/Presentaciones/"
};

//Variables Globales
var table;

PoblarTablaPresentacion();

function LimpiarFormulario() {
    $('#DivAgregarPresentacion').hide();
    $('#DivTablaPresentacion').show();
    $('#FormAgregarPresentacion')[0].reset();

    $('.invalid-feedback').text('');
    $('.form-control').removeClass('is-invalid');
}

function PoblarTablaPresentacion() {
    if ($.fn.DataTable.isDataTable('#TablaPresentacion')) {
        $('#TablaPresentacion').DataTable().destroy();
    }

    $.ajax({
        url: Componente.UrlControlador + "ObtenerListaPresentaciones",
        type: 'POST',
        success: function (response) {
            if (response.error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: response.error
                });
                return;
            }

            table = $('#TablaPresentacion').DataTable({
                data: response.data,
                columns: [
                    { data: 'nombre' },
                    { data: 'descripcion' },
                    {
                        data: 'idPresentaciones',
                        render: function (data, type, row) {
                            return `
                                <div class="text-center">
                                    <div class="btn-group" role="group">
                                        <button class="btn btn-primary editar-btn" data-idpresentaciones="${row.idPresentaciones}" title="Editar">
                                            <i class="fa-solid fa-pen-to-square"></i>
                                        </button>
                                        <button class="btn btn-danger eliminar-btn" data-idpresentaciones="${row.idPresentaciones}" title="Eliminar">
                                            <i class="fa-solid fa-trash"></i>
                                        </button>
                                    </div>
                                </div>
                            `;
                        }
                    }
                ],
                order: [[0, 'asc']],
                responsive: true,

                dom:
                    "<'row mb-3'<'col-sm-6 d-flex align-items-center'B><'col-sm-6 d-flex justify-content-end'f>>" +
                    "rt" +
                    "<'row mt-3'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",

                buttons: [
                    {
                        text: 'Nuevo',
                        className: 'btn btn-primary',
                        action: function () {
                            $('#DivAgregarPresentacion').show();
                            $('#DivTablaPresentacion').hide();
                        }
                    },
                    { extend: 'excel', text: '<i class="fa-solid fa-file-excel"></i> Excel', className: 'btn btn-success' },
                    { extend: 'pdf', text: '<i class="fa-solid fa-file-pdf"></i> PDF', className: 'btn btn-danger' },
                    { extend: 'print', text: '<i class="fa-solid fa-print"></i> Imprimir', className: 'btn btn-secondary' }
                ]
            });
        },
        error: function (xhr, status, error) {
            Swal.fire({
                icon: 'error',
                title: 'Error en la solicitud AJAX',
                text: error
            });
        }
    });
}

$('#cancelButtonPresentacion').on('click', function () {
    LimpiarFormulario();
});

$('#GuardarDatosPresentacion').on('click', function () {
    // Obtener valores de los campos
    var nombre = $('#NombrePresentacion').val();
    var descripcion = $('#DescripcionPresentacion').val();

    // Validar campos obligatorios
    if (!nombre || nombre.trim() === '') {
        mostrarError('NombrePresentacion', 'El nombre de la presentación es obligatorio');
        $('#NombrePresentacion').trigger('focus');
        return;
    }

    // Preparar datos para enviar según el modelo Presentacion_VM
    var datosEnvio = {
        Nombre: nombre.trim(),
        Descripcion: descripcion ? descripcion.trim() : null
    };

    // Confirmación antes de enviar
    Swal.fire({
        title: '¿Confirmar guardado?',
        html: `Va a crear la presentación: <strong>${nombre.trim()}</strong><br>¿Desea continuar?`,
        icon: 'question',
        showCancelButton: true,
        cancelButtonText: 'Cancelar',
        confirmButtonText: 'Sí, guardar',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            enviarDatosAlServidor(datosEnvio);
        }
    });
});

function mostrarError(campoId, mensaje) {
    $('#' + campoId).addClass('is-invalid');
    $('#' + campoId).siblings('.invalid-feedback').text(mensaje);
}

function enviarDatosAlServidor(datos) {
    // Determinar la acción según si hay IdPresentaciones
    const esEdicion = datos.IdPresentaciones &&
        datos.IdPresentaciones !== '' &&
        datos.IdPresentaciones !== '00000000-0000-0000-0000-000000000000';
    const accion = esEdicion ? 'ModificarPresentacion' : 'AgregarPresentacion';

    $.ajax({
        url: Componente.UrlControlador + accion,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(datos),
        success: function (response) {
            if (response.success) {
                const mensaje = esEdicion
                    ? 'La Presentación se modificó exitosamente'
                    : 'La Presentación se creó exitosamente';

                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: mensaje,
                    showConfirmButton: true,
                }).then(function () {
                    esEdicion ? RetornarAIndexDesdeEditar() : LimpiarFormulario();
                    PoblarTablaPresentacion();
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: response.error || 'Hubo un error al procesar la solicitud.'
                });
            }
        },
        error: function (xhr, status, error) {
            let errorMsg = 'Hubo un error al enviar el formulario';
            if (xhr.responseJSON && xhr.responseJSON.error) {
                errorMsg = xhr.responseJSON.error;
            }

            Swal.fire({
                icon: 'error',
                title: 'Error de conexión',
                text: errorMsg
            });
        }
    });
}

$('#TablaPresentacion').on('click', '.eliminar-btn', function () {
    const id = $(this).data('idpresentaciones');

    // Verificar que el GUID sea válido
    if (!id || id === '00000000-0000-0000-0000-000000000000') {
        Swal.fire('Error', 'ID de Presentación no válido', 'error');
        return;
    }
    Swal.fire({
        title: '¿Eliminar Presentación?',
        text: "Esta acción eliminará la presentación.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar',
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: Componente.UrlControlador + 'EliminarPresentacion',
                type: 'POST',
                data: { IdPresentacion: id },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Eliminada',
                            text: 'La presentación fue eliminada correctamente.'
                        });

                        PoblarTablaPresentacion();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.error || 'No se pudo eliminar la presentación.'
                        });
                    }
                },
                error: function (xhr, status, error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error de conexión',
                        text: 'No se pudo procesar la solicitud: ' + error
                    });
                }
            });
        }
    });
});

$('#TablaPresentacion').on('click', '.editar-btn', function () {
    const id = $(this).data('idpresentaciones');
    $.get(Componente.UrlControlador + "EditarPresentaciones", { IdPresentacion: id }, function (html) {
        $('#DivModificarPresentacion').show().html(html);
        $('#DivTablaPresentacion').hide();
    }).fail(function () {
        Swal.fire("Error", "No se pudo cargar el formulario de edición.", "error");
    });
});

function RetornarAIndexDesdeEditar() {
    // Ocultar y limpiar completamente el div de edición
    $('#DivModificarPresentacion').hide().empty();

    // Mostrar nuevamente la tabla principal
    $('#DivTablaPresentacion').show();
}

$('#DivModificarPresentacion').on('click', '#cancelButtonModificar', function () {
    RetornarAIndexDesdeEditar();
});

$('#DivModificarPresentacion').on('click', '#ModificarPresentacion', function () {
    // Buscar los campos dentro del formulario de modificación
    var IdPresentacion = $('#IdPresentaciones').val();
    var Nombre = $('#DivModificarPresentacion #Nombre').val();
    var Descripcion = $('#DivModificarPresentacion #Descripcion').val();

    // Validaciones
    if (!Nombre || Nombre.trim() === '') {
        Swal.fire({
            icon: 'warning',
            title: 'Campo requerido',
            text: 'El nombre de la presentación es obligatorio'
        });
        $('#Nombre').trigger('focus');
        return;
    }

    if (!IdPresentacion) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo identificar la presentación a modificar'
        });
        return;
    }

    var datosEnvio = {
        IdPresentaciones: IdPresentacion,
        Nombre: Nombre.trim(),
        Descripcion: Descripcion ? Descripcion.trim() : null
    };

    Swal.fire({
        title: '¿Confirmar modificación?',
        html: `Va a modificar los datos de la presentación:<br>
               <strong>${Nombre.trim()}</strong><br>
               ¿Desea continuar?`,
        icon: 'question',
        showCancelButton: true,
        cancelButtonText: 'Cancelar',
        confirmButtonText: 'Sí, guardar',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            enviarDatosAlServidor(datosEnvio);
        }
    });
});