window.Componente = {
    UrlControlador: "/Individuo/"
};



//Variables Globales
var table;



PoblarTablaIndividuo();


function LimpiarFormulario() {
    $('#DivAgregarIndividuos').hide();


    $('#DivTablaIndividuos').show();

    $('#FormAgregarIndividuos')[0].reset();
}


function PoblarTablaIndividuo() {
    if ($.fn.DataTable.isDataTable('#TablaIndividuos')) {
        $('#TablaIndividuos').DataTable().destroy();
    }

    $.ajax({
        url: Componente.UrlControlador + "ObtenerListaIndividuos",
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



            table = $('#TablaIndividuos').DataTable({
                data: response.data,
                columns: [
                    { data: 'nombre' },
                    { data: 'apellido' },
                    { data: 'telefono' },
                    { data: 'direccion' },
                    { data: 'email' },
                    {
                        data: 'idDepartamento',
                        render: function (data, type, row) {
                            return `
                                <div class="text-center">
                                    <div class="btn-group" role="group">
                                        <button class="btn btn-primary editar-btn" data-idIndividuos="${row.idIndividuos}" title="Editar">
                                            <i class="fa-solid fa-pen-to-square"></i>
                                        </button>
                                        <button class="btn btn-danger eliminar-btn" data-idIndividuos="${row.idIndividuos}" title="Eliminar">
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
                            $('#DivAgregarIndividuos').show();
                            $('#DivTablaIndividuos').hide();
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

$('#cancelButton').on('click', function () {
    LimpiarFormulario();
});




$('#GuardarDatosDepartamento').on('click', function () {
    // Validaciones iniciales
    var NombreDepartamento = $('#NombreDepartamento').val();

    if (!NombreDepartamento || NombreDepartamento.trim() === '') {
        Swal.fire({
            icon: 'warning',
            title: 'Campo requerido',
            text: 'El nombre de la bodega es obligatorio'
        });
        $('#NombreDepartamento').trigger('focus');
        return;
    }



    var datosEnvio = {
        NombreDepartamento: NombreDepartamento.trim(),
    };

    Swal.fire({
        title: '¿Confirmar guardado?',
        html: `Va a crear El Departamento <strong>"${NombreDepartamento.trim()}"</strong><br>, ¿Desea continuar?`,
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

function enviarDatosAlServidor(datos) {
    // Determinar la acción según si hay IdDepartamento
    const esEdicion = datos.IdDepartamento && datos.IdDepartamento > 0;
    const accion = esEdicion ? 'EditarDepartamento' : 'AgregarDepartamento';

    $.ajax({
        url: Componente.UrlControlador + accion,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(datos),
        success: function (response) {
            if (response.success) {
                const mensaje = esEdicion
                    ? 'El Departamento se modificó exitosamente'
                    : 'El Departamento se creó exitosamente';

                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: mensaje,
                    showConfirmButton: true,
                }).then(function () {
                    esEdicion ? RetornarAIndexDesdeEditar() : LimpiarFormulario();
                    PoblarTablaIndividuo();
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


$('#TablaIndividuos').on('click', '.eliminar-btn', function () {
    const idDepartamento = $(this).data('iddepartamento');
    Swal.fire({
        title: '¿Eliminar Departamento?',
        text: "Esta acción eliminará el Departamento.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar',
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: Componente.UrlControlador + 'EliminarDepartamento',
                type: 'POST',
                data: { IdDepartamento: idDepartamento },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Eliminada',
                            text: 'El departamento fue eliminado correctamente.'
                        });

                        PoblarTablaIndividuo();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.error || 'No se pudo eliminar el Departamento.'
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

$('#TablaIndividuos').on('click', '.editar-btn', function () {
    const idDepartamento = $(this).data('iddepartamento');

    $.get(Componente.UrlControlador + "FormularioEditar", { IdDepartamento: idDepartamento }, function (html) {
        $('#DivModificarDepartamento').show().html(html);
        $('#DivTablaDepartamentos').hide();
    }).fail(function () {
        Swal.fire("Error", "No se pudo cargar el formulario de edición.", "error");
    });
});


function RetornarAIndexDesdeEditar() {
    // Ocultar y limpiar completamente el div de edición
    $('#DivModificarDepartamento').hide().empty();

    // Mostrar nuevamente la tabla principal
    $('#DivTablaDepartamentos').show();
}

$('#DivModificarDepartamento').on('click', '#cancelButtonEditarDepartamento', function () {
    RetornarAIndexDesdeEditar();
});

$('#DivModificarDepartamento').on('click', '#EditarDepartamento', function () {
    // Buscar el campo dentro del div de modificación
    var NuevoNombreDepartamento = $('#DivModificarDepartamento #NombreDepartamento').val();
    var IdDepartamento = $('#IdDepartamento').val();
    if (!NuevoNombreDepartamento || NuevoNombreDepartamento.trim() === '') {
        Swal.fire({
            icon: 'warning',
            title: 'Campo requerido',
            text: 'El nombre del departamento es obligatorio'
        });
        $('#DivModificarDepartamento #NombreDepartamento').trigger('focus');
        return;
    }

    var datosEnvio = {
        IdDepartamento: IdDepartamento,
        NombreDepartamento: NuevoNombreDepartamento.trim(),
    };

    Swal.fire({
        title: '¿Confirmar guardado?',
        html: `Va a modificar el nombre del departamento <strong>"${NuevoNombreDepartamento.trim()}"</strong><br>¿Desea continuar?`,
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

