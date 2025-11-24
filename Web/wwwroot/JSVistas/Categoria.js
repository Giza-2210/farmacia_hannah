window.Componente = {
    UrlControlador: "/Categoria/"
};

//Variables Globales
var table;

PoblarTablaCategoria();

function LimpiarFormulario() {
    $('#DivAgregarCategoria').hide();
    $('#DivTablaCategoria').show();
    $('#FormAgregarCategoria')[0].reset();

    $('.invalid-feedback').text('');
    $('.form-control').removeClass('is-invalid');
}

function PoblarTablaCategoria() {
    if ($.fn.DataTable.isDataTable('#TablaCategoria')) {
        $('#TablaCategoria').DataTable().destroy();
    }

    $.ajax({
        url: Componente.UrlControlador + "ObtenerListaCategorias",
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

            table = $('#TablaCategoria').DataTable({
                data: response.data,
                columns: [
                    { data: 'nombre' },
                    { data: 'descripcion' },
                    {
                        data: 'idCategorias',
                        render: function (data, type, row) {
                            return `
                                <div class="text-center">
                                    <div class="btn-group" role="group">
                                        <button class="btn btn-primary editar-btn" data-idcategorias="${row.idCategorias}" title="Editar">
                                            <i class="fa-solid fa-pen-to-square"></i>
                                        </button>
                                        <button class="btn btn-danger eliminar-btn" data-idcategorias="${row.idCategorias}" title="Eliminar">
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
                            $('#DivAgregarCategoria').show();
                            $('#DivTablaCategoria').hide();
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

$('#cancelButtonCategoria').on('click', function () {
    LimpiarFormulario();
});

$('#GuardarDatosCategoria').on('click', function () {
    // Obtener valores de los campos
    var nombre = $('#NombreCategoria').val();
    var descripcion = $('#DescripcionCategoria').val();

    // Validar campos obligatorios
    if (!nombre || nombre.trim() === '') {
        mostrarError('NombreCategoria', 'El nombre de la categoría es obligatorio');
        $('#NombreCategoria').trigger('focus');
        return;
    }

    // Preparar datos para enviar según el modelo Categoria_VM
    var datosEnvio = {
        Nombre: nombre.trim(),
        Descripcion: descripcion ? descripcion.trim() : null
    };

    // Confirmación antes de enviar
    Swal.fire({
        title: '¿Confirmar guardado?',
        html: `Va a crear la categoría: <strong>${nombre.trim()}</strong><br>¿Desea continuar?`,
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
    // Determinar la acción según si hay IdCategorias
    const esEdicion = datos.IdCategorias &&
        datos.IdCategorias !== '' &&
        datos.IdCategorias !== '00000000-0000-0000-0000-000000000000';
    const accion = esEdicion ? 'EditarCategoria' : 'AgregarCategoria';

    $.ajax({
        url: Componente.UrlControlador + accion,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(datos),
        success: function (response) {
            if (response.success) {
                const mensaje = esEdicion
                    ? 'La Categoría se modificó exitosamente'
                    : 'La Categoría se creó exitosamente';

                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: mensaje,
                    showConfirmButton: true,
                }).then(function () {
                    esEdicion ? RetornarAIndexDesdeEditar() : LimpiarFormulario();
                    PoblarTablaCategoria();
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

$('#TablaCategoria').on('click', '.eliminar-btn', function () {
    const id = $(this).data('idcategorias');

    // Verificar que el GUID sea válido
    if (!id || id === '00000000-0000-0000-0000-000000000000') {
        Swal.fire('Error', 'ID de Categoría no válido', 'error');
        return;
    }
    Swal.fire({
        title: '¿Eliminar Categoría?',
        text: "Esta acción eliminará la categoría.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar',
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: Componente.UrlControlador + 'EliminarCategoria',
                type: 'POST',
                data: { IdCategoria: id },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Eliminada',
                            text: 'La categoría fue eliminada correctamente.'
                        });

                        PoblarTablaCategoria();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.error || 'No se pudo eliminar la categoría.'
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

$('#TablaCategoria').on('click', '.editar-btn', function () {
    const id = $(this).data('idcategorias');
    $.get(Componente.UrlControlador + "EditarCategoria", { IdCategoria: id }, function (html) {
        $('#DivModificarCategoria').show().html(html);
        $('#DivTablaCategoria').hide();
    }).fail(function () {
        Swal.fire("Error", "No se pudo cargar el formulario de edición.", "error");
    });
});

function RetornarAIndexDesdeEditar() {
    // Ocultar y limpiar completamente el div de edición
    $('#DivModificarCategoria').hide().empty();

    // Mostrar nuevamente la tabla principal
    $('#DivTablaCategoria').show();
}

$('#DivModificarCategoria').on('click', '#cancelButtonModificar', function () {
    RetornarAIndexDesdeEditar();
});

$('#DivModificarCategoria').on('click', '#ModificarCategoria', function () {
    // Buscar los campos dentro del formulario de modificación
    var IdCategoria = $('#IdCategorias').val();
    var Nombre = $('#DivModificarCategoria #Nombre').val();
    var Descripcion = $('#DivModificarCategoria #Descripcion').val();

    // Validaciones
    if (!Nombre || Nombre.trim() === '') {
        Swal.fire({
            icon: 'warning',
            title: 'Campo requerido',
            text: 'El nombre de la categoría es obligatorio'
        });
        $('#Nombre').trigger('focus');
        return;
    }

    if (!IdCategoria) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo identificar la categoría a modificar'
        });
        return;
    }

    var datosEnvio = {
        IdCategorias: IdCategoria,
        Nombre: Nombre.trim(),
        Descripcion: Descripcion ? Descripcion.trim() : null
    };

    Swal.fire({
        title: '¿Confirmar modificación?',
        html: `Va a modificar los datos de la categoría:<br>
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