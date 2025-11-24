using datos.BaseDatos;
using modelo;

namespace logica
{
    public class Individuo_LN
    {
        private readonly Contexto bd;

        public Individuo_LN()
        {
            bd = new Contexto();
        }

        #region Consultas

        public Individuo_VM? ConsultarIndividuo(Guid IdIndividuo, out string? errorMessage)
        {
            try
            {
                // Validar que el ID no sea vacío
                if (IdIndividuo == Guid.Empty)
                {
                    errorMessage = "El ID del Individuo no puede estar vacío.";
                    return null;
                }

                var Individuo = bd.Individuos
                    .FirstOrDefault(b => b.IdIndividuos == IdIndividuo);

                if (Individuo == null)
                {
                    errorMessage = "El Individuo no existe.";
                    return null;
                }

                var resultado = new Individuo_VM
                {
                    IdIndividuos = Individuo.IdIndividuos,
                    Nombre = Individuo.Nombre ?? string.Empty,
                    Apellido = Individuo.Apellido ?? string.Empty,
                    Telefono = Individuo.Telefono,
                    Direccion = Individuo.Direccion,
                    Email = Individuo.Email,
                    FechaRegistro = Individuo.FechaRegistro,
                    Activo = Individuo.Activo
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

        public bool ProporcionarListaIndividuos(ref List<Individuo_VM> ListaIndividuo, out string? errorMessage)
        {
            try
            {
                ListaIndividuo = (from Indi in bd.Individuos
                                  where Indi.Activo == true
                                  select new Individuo_VM
                                  {
                                      IdIndividuos = Indi.IdIndividuos,
                                      Nombre = Indi.Nombre ?? string.Empty,
                                      Apellido = Indi.Apellido ?? string.Empty,
                                      Telefono = Indi.Telefono,
                                      Direccion = Indi.Direccion,
                                      Email = Indi.Email,
                                      FechaRegistro = Indi.FechaRegistro,
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
        public bool AgregarIndividuo(Individuo_VM Datos, out string? errorMessage)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    // Validar por nombre y apellido
                    if (ExisteIndividuoConNombreApellido(Datos.Nombre, Datos.Apellido))
                    {
                        errorMessage = "Ya existe un individuo con el mismo nombre y apellido.";
                        return false;
                    }

                    var NuevoIndividuo = new Individuos
                    {
                        IdIndividuos = Guid.NewGuid(),
                        Nombre = Datos.Nombre.Trim(),
                        Apellido = Datos.Apellido.Trim(),
                        Telefono = Datos.Telefono?.Trim(),
                        Direccion = Datos.Direccion?.Trim(),
                        Email = Datos.Email?.Trim(),
                        FechaRegistro = Datos.FechaRegistro ?? DateOnly.FromDateTime(DateTime.Now),
                        Activo = Datos.Activo ?? true
                    };

                    bd.Individuos.Add(NuevoIndividuo);
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

        public bool ModificarIndividuo(Individuo_VM IndividuoMod, out string? MensajeError)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    // Validar nombre y apellido único
                    if (ExisteIndividuoConNombreApellido(IndividuoMod.Nombre, IndividuoMod.Apellido, IndividuoMod.IdIndividuos))
                    {
                        MensajeError = "Ya existe un individuo con el mismo nombre y apellido.";
                        return false;
                    }

                    // Validar email único (si el email no está vacío)
                    if (!string.IsNullOrEmpty(IndividuoMod.Email) &&
                        ExisteIndividuoConEmail(IndividuoMod.Email, IndividuoMod.IdIndividuos))
                    {
                        MensajeError = "Ya existe un individuo con el mismo email.";
                        return false;
                    }

                    var Individuo = bd.Individuos.FirstOrDefault(b => b.IdIndividuos == IndividuoMod.IdIndividuos && b.Activo == true);
                    if (Individuo == null)
                    {
                        MensajeError = "El Individuo no existe o fue eliminado.";
                        return false;
                    }

                    // Actualizar datos
                    Individuo.Nombre = IndividuoMod.Nombre.Trim();
                    Individuo.Apellido = IndividuoMod.Apellido.Trim();
                    Individuo.Telefono = IndividuoMod.Telefono?.Trim();
                    Individuo.Direccion = IndividuoMod.Direccion?.Trim();
                    Individuo.Email = IndividuoMod.Email?.Trim();

                    bd.SaveChanges();
                    transaction.Commit();

                    MensajeError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MensajeError = "Error al modificar el Individuo: " + ex.Message;
                    return false;
                }
            }
        }

        public bool EliminarIndividuo(string IdIndividuo, out string? MensajeError)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrEmpty(IdIndividuo))
                    {
                        MensajeError = "El ID del individuo no puede estar vacío.";
                        return false;
                    }


                    if (!Guid.TryParse(IdIndividuo, out Guid guidIdIndividuo))
                    {
                        MensajeError = "El formato del ID del individuo no es válido.";
                        return false;
                    }

                    var Individuo = bd.Individuos.FirstOrDefault(b => b.IdIndividuos == guidIdIndividuo);
                    if (Individuo == null)
                    {
                        MensajeError = "El Individuo no existe o ya fue eliminado.";
                        return false;
                    }


                    if (Individuo.Activo == false)
                    {
                        MensajeError = "El Individuo ya está eliminado.";
                        return false;
                    }

                    Individuo.Activo = false;

                    bd.SaveChanges();
                    transaction.Commit();

                    MensajeError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MensajeError = "Error al eliminar el Individuo: " + ex.Message;
                    return false;
                }
            }
        }



        #endregion

        #region Validaciones
        private bool ExisteIndividuoConNombreApellido(string nombre, string apellido)
        {
            return bd.Individuos.Any(i =>
                i.Nombre.ToLower() == nombre.ToLower() &&
                i.Apellido.ToLower() == apellido.ToLower());
        }

        private bool ExisteIndividuoConNombreApellido(string nombre, string apellido, Guid idIndividuoActual)
        {
            return bd.Individuos.Any(i =>
                i.Nombre.ToLower() == nombre.Trim().ToLower() &&
                i.Apellido.ToLower() == apellido.Trim().ToLower() &&
                i.IdIndividuos != idIndividuoActual && // Excluir el individuo actual
                i.Activo == true); // Solo considerar individuos activos
        }

        private bool ExisteIndividuoConEmail(string email, Guid idIndividuoActual)
        {
            return bd.Individuos.Any(i =>
                i.Email != null &&
                i.Email.ToLower() == email.Trim().ToLower() &&
                i.IdIndividuos != idIndividuoActual &&
                i.Activo == true);
        }
        #endregion
    }
}
