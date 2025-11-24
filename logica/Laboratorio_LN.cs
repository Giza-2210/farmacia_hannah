using datos.BaseDatos;
using modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace logica
{
    public  class Laboratorio_LN
    {
        private readonly Contexto bd;

        public Laboratorio_LN()
        {
            bd = new Contexto();
        }

        #region Consultas

        public Laboratorio_VM? ConsultarLaboratorio(Guid IdLaboratorio, out string? errorMessage)
        {
            try
            {
                // Validar que el ID no sea vacío
                if (IdLaboratorio == Guid.Empty)
                {
                    errorMessage = "El ID del Laboratorio no puede estar vacío.";
                    return null;
                }

                var Laboratorio = bd.Laboratorios
                    .FirstOrDefault(b => b.IdLaboratorios == IdLaboratorio && b.Estado == true);

                if (Laboratorio == null)
                {
                    errorMessage = "El Laboratorio no existe o fue eliminado.";
                    return null;
                }

                var resultado = new Laboratorio_VM
                {
                    IdLaboratorios = Laboratorio.IdLaboratorios,
                    Nombre = Laboratorio.Nombre ?? string.Empty,
                    Estado = Laboratorio.Estado
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

        public bool ProporcionarListaLaboratorios(ref List<Laboratorio_VM> ListaLaboratorios, out string? errorMessage)
        {
            try
            {
                ListaLaboratorios = (from Lab in bd.Laboratorios
                                     where Lab.Estado == true
                                     select new Laboratorio_VM
                                     {
                                         IdLaboratorios = Lab.IdLaboratorios,
                                         Nombre = Lab.Nombre ?? string.Empty,
                                         Estado = Lab.Estado
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
        public bool AgregarLaboratorio(Laboratorio_VM Datos, out string? errorMessage)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    // Validar por nombre único
                    if (ExisteLaboratorioConNombre(Datos.Nombre))
                    {
                        errorMessage = "Ya existe un laboratorio con el mismo nombre.";
                        return false;
                    }

                    var NuevoLaboratorio = new Laboratorios
                    {
                        IdLaboratorios = Guid.NewGuid(),
                        Nombre = Datos.Nombre.Trim(),
                        Estado = true
                    };

                    bd.Laboratorios.Add(NuevoLaboratorio);
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

        public bool ModificarLaboratorio(Laboratorio_VM LaboratorioMod, out string? MensajeError)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    // Validar nombre único
                    if (ExisteLaboratorioConNombre(LaboratorioMod.Nombre, LaboratorioMod.IdLaboratorios))
                    {
                        MensajeError = "Ya existe un laboratorio con el mismo nombre.";
                        return false;
                    }

                    var Laboratorio = bd.Laboratorios.FirstOrDefault(b => b.IdLaboratorios == LaboratorioMod.IdLaboratorios && b.Estado == true);
                    if (Laboratorio == null)
                    {
                        MensajeError = "El Laboratorio no existe o fue eliminado.";
                        return false;
                    }

                    // Actualizar datos
                    Laboratorio.Nombre = LaboratorioMod.Nombre.Trim();

                    bd.SaveChanges();
                    transaction.Commit();

                    MensajeError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MensajeError = "Error al modificar el Laboratorio: " + ex.Message;
                    return false;
                }
            }
        }

        public bool EliminarLaboratorio(string IdLaboratorio, out string? MensajeError)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrEmpty(IdLaboratorio))
                    {
                        MensajeError = "El ID del laboratorio no puede estar vacío.";
                        return false;
                    }

                    if (!Guid.TryParse(IdLaboratorio, out Guid guidIdLaboratorio))
                    {
                        MensajeError = "El formato del ID del laboratorio no es válido.";
                        return false;
                    }

                    var Laboratorio = bd.Laboratorios.FirstOrDefault(b => b.IdLaboratorios == guidIdLaboratorio);
                    if (Laboratorio == null)
                    {
                        MensajeError = "El Laboratorio no existe o ya fue eliminado.";
                        return false;
                    }

                    if (Laboratorio.Estado == false)
                    {
                        MensajeError = "El Laboratorio ya está eliminado.";
                        return false;
                    }

                    // Eliminación lógica
                    Laboratorio.Estado = false;

                    bd.SaveChanges();
                    transaction.Commit();

                    MensajeError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MensajeError = "Error al eliminar el Laboratorio: " + ex.Message;
                    return false;
                }
            }
        }
        #endregion

        #region Validaciones
        private bool ExisteLaboratorioConNombre(string nombre)
        {
            return bd.Laboratorios.Any(l =>
                l.Nombre.ToLower() == nombre.Trim().ToLower() &&
                l.Estado == true);
        }

        private bool ExisteLaboratorioConNombre(string nombre, Guid idLaboratorioActual)
        {
            return bd.Laboratorios.Any(l =>
                l.Nombre.ToLower() == nombre.Trim().ToLower() &&
                l.IdLaboratorios != idLaboratorioActual &&
                l.Estado == true);
        }
        #endregion

    }
}
