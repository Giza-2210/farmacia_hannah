using datos.BaseDatos;
using modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace logica
{
    public  class Medicamentos_LN
    {
        private readonly Contexto bd;
        public Medicamentos_LN()
        {
            bd = new Contexto();
        }

        #region Consultas

        public Medicamentos_VM? ConsultarMedicamento(Guid IdMedicamento, out string? errorMessage)
        {
            try
            {
                // Validar que el ID no sea vacío
                if (IdMedicamento == Guid.Empty)
                {
                    errorMessage = "El ID del Medicamento no puede estar vacío.";
                    return null;
                }

                var Medicamento = bd.Medicamentos
                    .FirstOrDefault(b => b.IdMedicamentos == IdMedicamento && b.Activo == true);

                if (Medicamento == null)
                {
                    errorMessage = "El Medicamento no existe o fue eliminado.";
                    return null;
                }

                var resultado = new Medicamentos_VM
                {
                    IdMedicamentos = Medicamento.IdMedicamentos,
                    Nombre = Medicamento.Nombre ?? string.Empty,
                    Descripcion = Medicamento.Descripcion,
                    PrecioCompra = Medicamento.PrecioCompra,
                    PrecioVenta = Medicamento.PrecioVenta,
                    Stock = Medicamento.Stock,
                    StockMinimo = Medicamento.StockMinimo,
                    FechaIngreso = Medicamento.FechaIngreso,
                    FechaVencimiento = Medicamento.FechaVencimiento,
                    Lote = Medicamento.Lote ?? string.Empty,
                    IdProveedor = Medicamento.IdProveedor,
                    IdCategoria = Medicamento.IdCategoria,
                    IdLaboratorio = Medicamento.IdLaboratorio,
                    IdPresentacion = Medicamento.IdPresentacion,
                    Activo = Medicamento.Activo
                };

                errorMessage = null;
                return resultado;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
        }

        public bool ProporcionarListaMedicamentos(ref List<Medicamentos_VM> ListaMedicamentos, out string? errorMessage)
        {
            try
            {
                ListaMedicamentos = (from Med in bd.Medicamentos
                                     where Med.Activo == true
                                     select new Medicamentos_VM
                                     {
                                         IdMedicamentos = Med.IdMedicamentos,
                                         Nombre = Med.Nombre ?? string.Empty,
                                         Descripcion = Med.Descripcion,
                                         PrecioCompra = Med.PrecioCompra,
                                         PrecioVenta = Med.PrecioVenta,
                                         Stock = Med.Stock,
                                         StockMinimo = Med.StockMinimo,
                                         FechaIngreso = Med.FechaIngreso,
                                         FechaVencimiento = Med.FechaVencimiento,
                                         Lote = Med.Lote ?? string.Empty,
                                         IdProveedor = Med.IdProveedor,
                                         IdCategoria = Med.IdCategoria,
                                         IdLaboratorio = Med.IdLaboratorio,
                                         IdPresentacion = Med.IdPresentacion,
                                         Activo = Med.Activo
                                     }).ToList();
                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
        #endregion

        #region CRUD
        public bool AgregarMedicamento(Medicamentos_VM Datos, out string? errorMessage)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    // Validar por nombre único
                    if (ExisteMedicamentoConNombre(Datos.Nombre))
                    {
                        errorMessage = "Ya existe un medicamento con el mismo nombre.";
                        return false;
                    }

                    // Validar lote único
                    if (ExisteMedicamentoConLote(Datos.Lote))
                    {
                        errorMessage = "Ya existe un medicamento con el mismo lote.";
                        return false;
                    }

                    var NuevoMedicamento = new Medicamentos
                    {
                        IdMedicamentos = Guid.NewGuid(),
                        Nombre = Datos.Nombre.Trim(),
                        Descripcion = Datos.Descripcion?.Trim(),
                        PrecioCompra = Datos.PrecioCompra,
                        PrecioVenta = Datos.PrecioVenta,
                        Stock = Datos.Stock,
                        StockMinimo = Datos.StockMinimo,
                        FechaIngreso = Datos.FechaIngreso,
                        FechaVencimiento = Datos.FechaVencimiento,
                        Lote = Datos.Lote.Trim(),
                        IdProveedor = Datos.IdProveedor,
                        IdCategoria = Datos.IdCategoria,
                        IdLaboratorio = Datos.IdLaboratorio,
                        IdPresentacion = Datos.IdPresentacion,
                        Activo = true
                    };

                    bd.Medicamentos.Add(NuevoMedicamento);
                    bd.SaveChanges();

                    transaction.Commit();

                    errorMessage = null;
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    errorMessage = ex.Message;
                    return false;
                }
            }
        }

        public bool ModificarMedicamento(Medicamentos_VM MedicamentoMod, out string? MensajeError)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    // Validar nombre único
                    if (ExisteMedicamentoConNombre(MedicamentoMod.Nombre, MedicamentoMod.IdMedicamentos))
                    {
                        MensajeError = "Ya existe un medicamento con el mismo nombre.";
                        return false;
                    }

                    // Validar lote único
                    if (ExisteMedicamentoConLote(MedicamentoMod.Lote, MedicamentoMod.IdMedicamentos))
                    {
                        MensajeError = "Ya existe un medicamento con el mismo lote.";
                        return false;
                    }

                    var Medicamento = bd.Medicamentos.FirstOrDefault(b => b.IdMedicamentos == MedicamentoMod.IdMedicamentos && b.Activo == true);
                    if (Medicamento == null)
                    {
                        MensajeError = "El Medicamento no existe o fue eliminado.";
                        return false;
                    }

                    // Actualizar datos
                    Medicamento.Nombre = MedicamentoMod.Nombre.Trim();
                    Medicamento.Descripcion = MedicamentoMod.Descripcion?.Trim();
                    Medicamento.PrecioCompra = MedicamentoMod.PrecioCompra;
                    Medicamento.PrecioVenta = MedicamentoMod.PrecioVenta;
                    Medicamento.Stock = MedicamentoMod.Stock;
                    Medicamento.StockMinimo = MedicamentoMod.StockMinimo;
                    Medicamento.FechaIngreso = MedicamentoMod.FechaIngreso;
                    Medicamento.FechaVencimiento = MedicamentoMod.FechaVencimiento;
                    Medicamento.Lote = MedicamentoMod.Lote.Trim();
                    Medicamento.IdProveedor = MedicamentoMod.IdProveedor;
                    Medicamento.IdCategoria = MedicamentoMod.IdCategoria;
                    Medicamento.IdLaboratorio = MedicamentoMod.IdLaboratorio;
                    Medicamento.IdPresentacion = MedicamentoMod.IdPresentacion;

                    bd.SaveChanges();
                    transaction.Commit();

                    MensajeError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MensajeError = "Error al modificar el Medicamento: " + ex.Message;
                    return false;
                }
            }
        }

        public bool EliminarMedicamento(string IdMedicamento, out string? MensajeError)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrEmpty(IdMedicamento))
                    {
                        MensajeError = "El ID del medicamento no puede estar vacío.";
                        return false;
                    }

                    if (!Guid.TryParse(IdMedicamento, out Guid guidIdMedicamento))
                    {
                        MensajeError = "El formato del ID del medicamento no es válido.";
                        return false;
                    }

                    var Medicamento = bd.Medicamentos.FirstOrDefault(b => b.IdMedicamentos == guidIdMedicamento);
                    if (Medicamento == null)
                    {
                        MensajeError = "El Medicamento no existe o ya fue eliminado.";
                        return false;
                    }

                    if (Medicamento.Activo == false)
                    {
                        MensajeError = "El Medicamento ya está eliminado.";
                        return false;
                    }

                    // Eliminación lógica
                    Medicamento.Activo = false;

                    bd.SaveChanges();
                    transaction.Commit();

                    MensajeError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MensajeError = "Error al eliminar el Medicamento: " + ex.Message;
                    return false;
                }
            }
        }
        #endregion

        #region Validaciones
        private bool ExisteMedicamentoConNombre(string nombre)
        {
            return bd.Medicamentos.Any(m =>
                m.Nombre.ToLower() == nombre.Trim().ToLower() &&
                m.Activo == true);
        }

        private bool ExisteMedicamentoConNombre(string nombre, Guid idMedicamentoActual)
        {
            return bd.Medicamentos.Any(m =>
                m.Nombre.ToLower() == nombre.Trim().ToLower() &&
                m.IdMedicamentos != idMedicamentoActual &&
                m.Activo == true);
        }

        private bool ExisteMedicamentoConLote(string lote)
        {
            return bd.Medicamentos.Any(m =>
                m.Lote.ToLower() == lote.Trim().ToLower() &&
                m.Activo == true);
        }

        private bool ExisteMedicamentoConLote(string lote, Guid idMedicamentoActual)
        {
            return bd.Medicamentos.Any(m =>
                m.Lote.ToLower() == lote.Trim().ToLower() &&
                m.IdMedicamentos != idMedicamentoActual &&
                m.Activo == true);
        }
        #endregion
    }
}
