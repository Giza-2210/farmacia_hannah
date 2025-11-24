using datos.BaseDatos;
using modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace logica
{
    public  class Presentaciones_LN
    {

        private readonly Contexto bd;

        public Presentaciones_LN()
        {
            bd = new Contexto();
        }

        #region Consultas

        public Presentacion_VM? ConsultarPresentacion(Guid IdPresentacion, out string? errorMessage)
        {
            try
            {
                // Validar que el ID no sea vacío
                if (IdPresentacion == Guid.Empty)
                {
                    errorMessage = "El ID de la Presentación no puede estar vacío.";
                    return null;
                }

                var Presentacion = bd.Presentaciones
                    .FirstOrDefault(b => b.IdPresentaciones == IdPresentacion && b.Estado == true);

                if (Presentacion == null)
                {
                    errorMessage = "La Presentación no existe o fue eliminada.";
                    return null;
                }

                var resultado = new Presentacion_VM
                {
                    IdPresentaciones = Presentacion.IdPresentaciones,
                    Nombre = Presentacion.Nombre ?? string.Empty,
                    Descripcion = Presentacion.Descripcion,
                    Estado = Presentacion.Estado
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

        public bool ProporcionarListaPresentaciones(ref List<Presentacion_VM> ListaPresentaciones, out string? errorMessage)
        {
            try
            {
                ListaPresentaciones = (from Pre in bd.Presentaciones
                                       where Pre.Estado == true
                                       select new Presentacion_VM
                                       {
                                           IdPresentaciones = Pre.IdPresentaciones,
                                           Nombre = Pre.Nombre ?? string.Empty,
                                           Descripcion = Pre.Descripcion,
                                           Estado = Pre.Estado
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
        public bool AgregarPresentacion(Presentacion_VM Datos, out string? errorMessage)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    // Validar por nombre único
                    if (ExistePresentacionConNombre(Datos.Nombre))
                    {
                        errorMessage = "Ya existe una presentación con el mismo nombre.";
                        return false;
                    }

                    var NuevaPresentacion = new Presentaciones
                    {
                        IdPresentaciones = Guid.NewGuid(),
                        Nombre = Datos.Nombre.Trim(),
                        Descripcion = Datos.Descripcion?.Trim(),
                        Estado = true
                    };

                    bd.Presentaciones.Add(NuevaPresentacion);
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

        public bool ModificarPresentacion(Presentacion_VM PresentacionMod, out string? MensajeError)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    // Validar nombre único
                    if (ExistePresentacionConNombre(PresentacionMod.Nombre, PresentacionMod.IdPresentaciones))
                    {
                        MensajeError = "Ya existe una presentación con el mismo nombre.";
                        return false;
                    }

                    var Presentacion = bd.Presentaciones.FirstOrDefault(b => b.IdPresentaciones == PresentacionMod.IdPresentaciones && b.Estado == true);
                    if (Presentacion == null)
                    {
                        MensajeError = "La Presentación no existe o fue eliminada.";
                        return false;
                    }

                    // Actualizar datos
                    Presentacion.Nombre = PresentacionMod.Nombre.Trim();
                    Presentacion.Descripcion = PresentacionMod.Descripcion?.Trim();

                    bd.SaveChanges();
                    transaction.Commit();

                    MensajeError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MensajeError = "Error al modificar la Presentación: " + ex.Message;
                    return false;
                }
            }
        }

        public bool EliminarPresentacion(string IdPresentacion, out string? MensajeError)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrEmpty(IdPresentacion))
                    {
                        MensajeError = "El ID de la presentación no puede estar vacío.";
                        return false;
                    }

                    if (!Guid.TryParse(IdPresentacion, out Guid guidIdPresentacion))
                    {
                        MensajeError = "El formato del ID de la presentación no es válido.";
                        return false;
                    }

                    var Presentacion = bd.Presentaciones.FirstOrDefault(b => b.IdPresentaciones == guidIdPresentacion);
                    if (Presentacion == null)
                    {
                        MensajeError = "La Presentación no existe o ya fue eliminada.";
                        return false;
                    }

                    if (Presentacion.Estado == false)
                    {
                        MensajeError = "La Presentación ya está eliminada.";
                        return false;
                    }

                    // Eliminación lógica
                    Presentacion.Estado = false;

                    bd.SaveChanges();
                    transaction.Commit();

                    MensajeError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MensajeError = "Error al eliminar la Presentación: " + ex.Message;
                    return false;
                }
            }
        }
        #endregion

        #region Validaciones
        private bool ExistePresentacionConNombre(string nombre)
        {
            return bd.Presentaciones.Any(p =>
                p.Nombre.ToLower() == nombre.Trim().ToLower() &&
                p.Estado == true);
        }

        private bool ExistePresentacionConNombre(string nombre, Guid idPresentacionActual)
        {
            return bd.Presentaciones.Any(p =>
                p.Nombre.ToLower() == nombre.Trim().ToLower() &&
                p.IdPresentaciones != idPresentacionActual &&
                p.Estado == true);
        }
        #endregion
    }
}
