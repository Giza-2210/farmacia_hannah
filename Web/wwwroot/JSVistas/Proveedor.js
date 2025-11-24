window.Componente = {
    UrlControlador: "/Proveedores/"
};



//Variables Globales
var table;



PoblarTablaProveedores();


$("#TelefonoProveedor").inputmask("9999-9999");

function LimpiarFormulario() {
    $('#DivAgregarProveedor').hide();


    $('#DivTablaProveedores').show();

    $('#FormAgregarProveedor')[0].reset();
}


function PoblarTablaProveedores() {
    if ($.fn.DataTable.isDataTable('#TablaProveedores')) {
        $('#TablaProveedores').DataTable().destroy();
    }

    $.ajax({
        url: Componente.UrlControlador + "ObtenerListaProveedores",
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



            table = $('#TablaProveedores').DataTable({
                data: response.data,
                columns: [
                    { data: 'nombre' },
                    { data: 'nombreContacto' },
                    { data: 'telefono' },
                    { data: 'direccion' },
                    { data: 'email' },
                    {
                        data: 'idProveedores',
                        render: function (data, type, row) {
                            return `
                                <div class="text-center">
                                    <div class="btn-group" role="group">
                                        <button class="btn btn-primary editar-btn" data-idProveedores="${row.idProveedores}" title="Editar">
                                            <i class="fa-solid fa-pen-to-square"></i>
                                        </button>
                                        <button class="btn btn-danger eliminar-btn" data-idProveedores="${row.idProveedores}" title="Eliminar">
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
                            $('#DivAgregarProveedor').show();
                            $('#DivTablaProveedores').hide();
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




$('#GuardarDatosProveedor').on('click', function () {
    // Obtener valores de los campos
    var nombre = $('#NombreProveedor').val();
    var contacto = $('#ContactoProveedor').val();
    var telefono = $('#TelefonoProveedor').val();
    var direccion = $('#DireccionProveedor').val();
    var email = $('#EmailProveedor').val();

    // Validar campos obligatorios
    if (!nombre || nombre.trim() === '') {
        mostrarError('NombreProveedor', 'El nombre del proveedor es obligatorio');
        $('#NombreProveedor').trigger('focus');
        return;
    }

    // Validar formato de email si se proporciona
    if (email && email.trim() !== '') {
        var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email.trim())) {
            mostrarError('EmailProveedor', 'El formato del email no es válido');
            $('#EmailProveedor').trigger('focus');
            return;
        }
    }

    // Preparar datos para enviar según el modelo Proveedores_VM
    var datosEnvio = {
        Nombre: nombre.trim(),
        NombreContacto: contacto ? contacto.trim() : null,
        Telefono: telefono ? telefono.trim() : null,
        Direccion: direccion ? direccion.trim() : null,
        Email: email ? email.trim() : null
    };

    // Confirmación antes de enviar
    Swal.fire({
        title: '¿Confirmar guardado?',
        html: `Va a crear el proveedor: <strong>${nombre.trim()}</strong><br>¿Desea continuar?`,
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

    // Determinar la acción según si hay IdIndividuo
    const esEdicion = datos.IdProveedores &&
        datos.IdProveedores !== '' &&
        datos.IdProveedores !== '00000000-0000-0000-0000-000000000000';
    const accion = esEdicion ? 'EditarProveedor' : 'AgregarProveedor';

    $.ajax({
        url: Componente.UrlControlador + accion,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(datos),
        success: function (response) {
            if (response.success) {
                const mensaje = esEdicion
                    ? 'El Proveedor se modificó exitosamente'
                    : 'El Proveedor se creó exitosamente';

                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: mensaje,
                    showConfirmButton: true,
                }).then(function () {
                    esEdicion ? RetornarAIndexDesdeEditar() : LimpiarFormulario();
                    PoblarTablaProveedores();
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


$('#TablaProveedores').on('click', '.eliminar-btn', function () {
    const id = $(this).data('idproveedores'); 
    // Verificar que el GUID sea válido
    if (!id || id === '00000000-0000-0000-0000-000000000000') {
        Swal.fire('Error', 'ID de Proveedor no válido', 'error');
        return;
    }
    Swal.fire({
        title: '¿Eliminar Proveedor?',
        text: "Esta acción eliminará el Proveedor.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar',
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: Componente.UrlControlador + 'EliminarProveedor',
                type: 'POST',
                data: { IdProveedor: id },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Eliminada',
                            text: 'El Proveedor fue eliminado correctamente.'
                        });

                        PoblarTablaProveedores();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.error || 'No se pudo eliminar el Individuo.'
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

$('#TablaProveedores').on('click', '.editar-btn', function () {
    const id = $(this).data('idproveedores');
    $.get(Componente.UrlControlador + "EditarProveedor", { idproveedor: id }, function (html) {
        $('#DivModificarProveedor').show().html(html);
        $('#DivTablaProveedores').hide();
        $("#Telefono").inputmask("9999-9999");//Inicaliza el input mask del editar
    }).fail(function () {
        Swal.fire("Error", "No se pudo cargar el formulario de edición.", "error");
    });
});


function RetornarAIndexDesdeEditar() {
    // Ocultar y limpiar completamente el div de edición
    $('#DivModificarProveedor').hide().empty();

    // Mostrar nuevamente la tabla principal
    $('#DivTablaProveedores').show();
}

$('#DivModificarProveedor').on('click', '#cancelButton', function () {
    RetornarAIndexDesdeEditar();
});

$('#DivModificarProveedor').on('click', '#ModificarProveedor', function () {
    // Buscar los campos dentro del formulario de modificación
    var IdProveedor = $('#IdProveedores').val();
    var Nombre = $('#Nombre').val();
    var NombreContacto = $('#NombreContacto').val();
    var Telefono = $('#Telefono').val();
    var Direccion = $('#Direccion').val();
    var Email = $('#Email').val();

    // Validaciones
    if (!Nombre || Nombre.trim() === '') {
        Swal.fire({
            icon: 'warning',
            title: 'Campo requerido',
            text: 'El nombre del proveedor es obligatorio'
        });
        $('#Nombre').trigger('focus');
        return;
    }

    if (!IdProveedor) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo identificar el proveedor a modificar'
        });
        return;
    }

    // Validar formato de email si se proporciona
    if (Email && Email.trim() !== '') {
        var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(Email.trim())) {
            Swal.fire({
                icon: 'warning',
                title: 'Formato inválido',
                text: 'El formato del email no es válido'
            });
            $('#Email').trigger('focus');
            return;
        }
    }

    var datosEnvio = {
        IdProveedores: IdProveedor,
        Nombre: Nombre.trim(),
        NombreContacto: NombreContacto ? NombreContacto.trim() : null,
        Telefono: Telefono ? Telefono.trim() : null,
        Direccion: Direccion ? Direccion.trim() : null,
        Email: Email ? Email.trim() : null
    };

    Swal.fire({
        title: '¿Confirmar modificación?',
        html: `Va a modificar los datos del proveedor:<br>
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

