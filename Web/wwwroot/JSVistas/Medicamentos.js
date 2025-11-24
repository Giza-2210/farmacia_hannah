window.Componente = {
    UrlControlador: "/Medicamentos/"
};

//Variables Globales
var table;


PoblarTablaMedicamento();

function LimpiarFormulario() {
    $('#DivAgregarMedicamento').hide();
    $('#DivTablaMedicamento').show();
    $('#FormAgregarMedicamento')[0].reset();

    // Resetear modo edición
    $('#FormAgregarMedicamento').removeData('modo-edicion');
    $('#FormAgregarMedicamento').removeData('id-medicamento');
    $('#GuardarDatosMedicamento').html('<i class="fa-solid fa-floppy-disk"></i> Agregar');

    $('.invalid-feedback').text('');
    $('.form-control').removeClass('is-invalid');
    $('.text-danger').text('');
}

function PoblarTablaMedicamento() {
    if ($.fn.DataTable.isDataTable('#TablaMedicamento')) {
        $('#TablaMedicamento').DataTable().destroy();
    }

    $.ajax({
        url: Componente.UrlControlador + "ObtenerListaMedicamentos",
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

            table = $('#TablaMedicamento').DataTable({
                data: response.data,
                columns: [
                    { data: 'nombre' },
                    { data: 'descripcion' },
                    {
                        data: 'precioCompra',
                        render: function (data) {
                            return 'C$ ' + parseFloat(data).toFixed(2);
                        }
                    },
                    {
                        data: 'precioVenta',
                        render: function (data) {
                            return 'C$ ' + parseFloat(data).toFixed(2);
                        }
                    },
                    { data: 'stock' },
                    { data: 'lote' },
                    {
                        data: 'fechaVencimiento',
                        render: function (data) {
                            return data ? new Date(data).toLocaleDateString() : '';
                        }
                    },
                    {
                        data: 'idMedicamentos',
                        render: function (data, type, row) {
                            return `
                                <div class="text-center">
                                    <div class="btn-group" role="group">
                                        <button class="btn btn-primary editar-btn" data-idmedicamentos="${row.idMedicamentos}" title="Editar">
                                            <i class="fa-solid fa-pen-to-square"></i>
                                        </button>
                                        <button class="btn btn-danger eliminar-btn" data-idmedicamentos="${row.idMedicamentos}" title="Eliminar">
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
                            // Asegurarse de que esté en modo agregar
                            $('#FormAgregarMedicamento').removeData('modo-edicion');
                            $('#FormAgregarMedicamento').removeData('id-medicamento');
                            $('#GuardarDatosMedicamento').html('<i class="fa-solid fa-floppy-disk"></i> Agregar');

                            $('#DivAgregarMedicamento').show();
                            $('#DivTablaMedicamento').hide();
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

$('#cancelButtonMedicamento').on('click', function () {
    LimpiarFormulario();
});

// Función para llenar el formulario con los datos del medicamento
function LlenarFormularioEdicion(medicamento) {
    $('#Nombre').val(medicamento.nombre);
    $('#Descripcion').val(medicamento.descripcion);
    $('#PrecioCompra').val(medicamento.precioCompra);
    $('#PrecioVenta').val(medicamento.precioVenta);
    $('#Stock').val(medicamento.stock);
    $('#StockMinimo').val(medicamento.stockMinimo);
    $('#FechaIngreso').val(formatearFechaParaInput(medicamento.fechaIngreso));
    $('#FechaVencimiento').val(formatearFechaParaInput(medicamento.fechaVencimiento));
    $('#Lote').val(medicamento.lote);
    $('#IdProveedor').val(medicamento.idProveedor);
    $('#IdCategoria').val(medicamento.idCategoria);
    $('#IdLaboratorio').val(medicamento.idLaboratorio);
    $('#IdPresentacion').val(medicamento.idPresentacion);
}

// Función para formatear fechas al formato YYYY-MM-DD para inputs type="date"
function formatearFechaParaInput(fecha) {
    if (!fecha) return '';
    const date = new Date(fecha);
    return date.toISOString().split('T')[0];
}

// Función para validar el formulario (compartida entre agregar y modificar)
function ValidarFormulario() {
    var nombre = $('#Nombre').val();
    var precioCompra = $('#PrecioCompra').val();
    var precioVenta = $('#PrecioVenta').val();
    var stock = $('#Stock').val();
    var stockMinimo = $('#StockMinimo').val();
    var fechaIngreso = $('#FechaIngreso').val();
    var fechaVencimiento = $('#FechaVencimiento').val();
    var lote = $('#Lote').val();
    var idProveedor = $('#IdProveedor').val();
    var idCategoria = $('#IdCategoria').val();
    var idLaboratorio = $('#IdLaboratorio').val();
    var idPresentacion = $('#IdPresentacion').val();

    // Limpiar errores previos
    $('.text-danger').text('');
    $('.form-control').removeClass('is-invalid');

    let valido = true;

    if (!nombre || nombre.trim() === '') {
        mostrarError('Nombre', 'El nombre del medicamento es obligatorio');
        valido = false;
    }

    if (!precioCompra || parseFloat(precioCompra) <= 0) {
        mostrarError('PrecioCompra', 'El precio de compra debe ser mayor a 0');
        valido = false;
    }

    if (!precioVenta || parseFloat(precioVenta) <= 0) {
        mostrarError('PrecioVenta', 'El precio de venta debe ser mayor a 0');
        valido = false;
    }

    if (!stock || parseInt(stock) < 0) {
        mostrarError('Stock', 'El stock no puede ser negativo');
        valido = false;
    }

    if (!stockMinimo || parseInt(stockMinimo) < 0) {
        mostrarError('StockMinimo', 'El stock mínimo no puede ser negativo');
        valido = false;
    }

    if (!fechaIngreso) {
        mostrarError('FechaIngreso', 'La fecha de ingreso es obligatoria');
        valido = false;
    }

    if (!fechaVencimiento) {
        mostrarError('FechaVencimiento', 'La fecha de vencimiento es obligatoria');
        valido = false;
    }

    if (!lote || lote.trim() === '') {
        mostrarError('Lote', 'El número de lote es obligatorio');
        valido = false;
    }

    if (!idProveedor) {
        mostrarError('IdProveedor', 'Debe seleccionar un proveedor');
        valido = false;
    }

    if (!idCategoria) {
        mostrarError('IdCategoria', 'Debe seleccionar una categoría');
        valido = false;
    }

    if (!idLaboratorio) {
        mostrarError('IdLaboratorio', 'Debe seleccionar un laboratorio');
        valido = false;
    }

    if (!idPresentacion) {
        mostrarError('IdPresentacion', 'Debe seleccionar una presentación');
        valido = false;
    }

    // Validar que la fecha de vencimiento sea mayor a la fecha de ingreso
    if (fechaIngreso && fechaVencimiento && new Date(fechaVencimiento) <= new Date(fechaIngreso)) {
        mostrarError('FechaVencimiento', 'La fecha de vencimiento debe ser posterior a la fecha de ingreso');
        valido = false;
    }

    return valido;
}

function mostrarError(campoId, mensaje) {
    $('#' + campoId).addClass('is-invalid');
    $('#' + campoId).siblings('.text-danger').text(mensaje);
}

// Función principal de guardar
$('#GuardarDatosMedicamento').on('click', function () {
    const esEdicion = $('#FormAgregarMedicamento').data('modo-edicion');

    if (!ValidarFormulario()) {
        return;
    }

    // Obtener valores de los campos
    var nombre = $('#Nombre').val();
    var descripcion = $('#Descripcion').val();
    var precioCompra = $('#PrecioCompra').val();
    var precioVenta = $('#PrecioVenta').val();
    var stock = $('#Stock').val();
    var stockMinimo = $('#StockMinimo').val();
    var fechaIngreso = $('#FechaIngreso').val();
    var fechaVencimiento = $('#FechaVencimiento').val();
    var lote = $('#Lote').val();
    var idProveedor = $('#IdProveedor').val();
    var idCategoria = $('#IdCategoria').val();
    var idLaboratorio = $('#IdLaboratorio').val();
    var idPresentacion = $('#IdPresentacion').val();

    var datosEnvio = {
        Nombre: nombre.trim(),
        Descripcion: descripcion ? descripcion.trim() : null,
        PrecioCompra: parseFloat(precioCompra),
        PrecioVenta: parseFloat(precioVenta),
        Stock: parseInt(stock),
        StockMinimo: parseInt(stockMinimo),
        FechaIngreso: fechaIngreso,
        FechaVencimiento: fechaVencimiento,
        Lote: lote.trim(),
        IdProveedor: idProveedor,
        IdCategoria: idCategoria,
        IdLaboratorio: idLaboratorio,
        IdPresentacion: idPresentacion
    };

    // Si es edición, agregar el ID
    if (esEdicion) {
        datosEnvio.IdMedicamentos = $('#FormAgregarMedicamento').data('id-medicamento');
    }

    const accion = esEdicion ? 'ModificarMedicamento' : 'AgregarMedicamento';
    const mensajeConfirmacion = esEdicion
        ? `Va a modificar el medicamento: <strong>${nombre.trim()}</strong><br>¿Desea continuar?`
        : `Va a crear el medicamento: <strong>${nombre.trim()}</strong><br>¿Desea continuar?`;
    const textoBoton = esEdicion ? 'Sí, modificar' : 'Sí, guardar';

    Swal.fire({
        title: '¿Confirmar guardado?',
        html: mensajeConfirmacion,
        icon: 'question',
        showCancelButton: true,
        cancelButtonText: 'Cancelar',
        confirmButtonText: textoBoton,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            enviarDatosAlServidor(datosEnvio, accion);
        }
    });
});

function enviarDatosAlServidor(datos, accion) {
    $.ajax({
        url: Componente.UrlControlador + accion,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(datos),
        success: function (response) {
            if (response.success) {
                const mensaje = accion === 'ModificarMedicamento'
                    ? 'El Medicamento se modificó exitosamente'
                    : 'El Medicamento se creó exitosamente';

                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: mensaje,
                    showConfirmButton: true,
                }).then(function () {
                    LimpiarFormulario();
                    PoblarTablaMedicamento();
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

$('#TablaMedicamento').on('click', '.eliminar-btn', function () {
    const id = $(this).data('idmedicamentos');

    if (!id || id === '00000000-0000-0000-0000-000000000000') {
        Swal.fire('Error', 'ID de Medicamento no válido', 'error');
        return;
    }

    Swal.fire({
        title: '¿Eliminar Medicamento?',
        text: "Esta acción eliminará el medicamento.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar',
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: Componente.UrlControlador + 'EliminarMedicamento',
                type: 'POST',
                data: { IdMedicamento: id },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Eliminado',
                            text: 'El medicamento fue eliminado correctamente.'
                        });
                        PoblarTablaMedicamento();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.error || 'No se pudo eliminar el medicamento.'
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

$('#TablaMedicamento').on('click', '.editar-btn', function () {
    const id = $(this).data('idmedicamentos');

    $.ajax({
        url: Componente.UrlControlador + "ConsultarMedicamento",
        type: 'POST',
        data: { IdMedicamento: id },
        success: function (response) {
            if (response.success) {
                LlenarFormularioEdicion(response.data);

                $('#DivAgregarMedicamento').show();
                $('#DivTablaMedicamento').hide();

                $('#GuardarDatosMedicamento').html('<i class="fa-solid fa-floppy-disk"></i> Modificar');

                $('#FormAgregarMedicamento').data('modo-edicion', true);
                $('#FormAgregarMedicamento').data('id-medicamento', id);

            } else {
                Swal.fire("Error", response.error || "No se pudo cargar los datos del medicamento.", "error");
            }
        },
        error: function (xhr, status, error) {
            Swal.fire("Error", "No se pudo cargar los datos del medicamento.", "error");
        }
    });
});