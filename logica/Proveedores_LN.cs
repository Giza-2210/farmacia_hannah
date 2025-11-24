using datos.BaseDatos;
using modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace logica
{
    public  class Proveedores_LN
    {
        private readonly Contexto bd;

        public Proveedores_LN()
        {
            bd = new Contexto();
        }

        #region Consultas

        public Proveedores_VM? ConsultarProveedor(Guid IdProveedor, out string? errorMessage)
        {
            try
            {
                // Validar que el ID no sea vacío
                if (IdProveedor == Guid.Empty)
                {
                    errorMessage = "El ID del Proveedor no puede estar vacío.";
                    return null;
                }

                var Proveedor = bd.Proveedores
                    .FirstOrDefault(b => b.IdProveedores == IdProveedor && b.Estado == true);

                if (Proveedor == null)
                {
                    errorMessage = "El Proveedor no existe o fue eliminado.";
                    return null;
                }

                var resultado = new Proveedores_VM
                {
                    IdProveedores = Proveedor.IdProveedores,
                    Nombre = Proveedor.Nombre ?? string.Empty,
                    NombreContacto = Proveedor.NombreContacto ?? string.Empty,
                    Telefono = Proveedor.Telefono,
                    Direccion = Proveedor.Direccion,
                    Email = Proveedor.Email
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

        public bool ProporcionarListaProveedores(ref List<Proveedores_VM> ListaProveedores, out string? errorMessage)
        {
            try
            {
                ListaProveedores = (from Prov in bd.Proveedores
                                    where Prov.Estado == true
                                    select new Proveedores_VM
                                    {
                                        IdProveedores = Prov.IdProveedores,
                                        Nombre = Prov.Nombre ?? string.Empty,
                                        NombreContacto = Prov.NombreContacto ?? string.Empty,
                                        Telefono = Prov.Telefono,
                                        Direccion = Prov.Direccion,
                                        Email = Prov.Email
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
        public bool AgregarProveedor(Proveedores_VM Datos, out string? errorMessage)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    // Validar por nombre único
                    if (ExisteProveedorConNombre(Datos.Nombre))
                    {
                        errorMessage = "Ya existe un proveedor con el mismo nombre.";
                        return false;
                    }

                    var NuevoProveedor = new Proveedores
                    {
                        IdProveedores = Guid.NewGuid(),
                        Nombre = Datos.Nombre.Trim(),
                        NombreContacto = Datos.NombreContacto?.Trim(),
                        Telefono = Datos.Telefono?.Trim(),
                        Direccion = Datos.Direccion?.Trim(),
                        Email = Datos.Email?.Trim(),
                        Estado = true // Asumiendo que tienes campo Estado para eliminación lógica
                    };

                    bd.Proveedores.Add(NuevoProveedor);
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

        public bool ModificarProveedor(Proveedores_VM ProveedorMod, out string? MensajeError)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    // Validar nombre único
                    if (ExisteProveedorConNombre(ProveedorMod.Nombre, ProveedorMod.IdProveedores))
                    {
                        MensajeError = "Ya existe un proveedor con el mismo nombre.";
                        return false;
                    }

                    // Validar email único (si el email no está vacío)
                    if (!string.IsNullOrEmpty(ProveedorMod.Email) &&
                        ExisteProveedorConEmail(ProveedorMod.Email, ProveedorMod.IdProveedores))
                    {
                        MensajeError = "Ya existe un proveedor con el mismo email.";
                        return false;
                    }

                    var Proveedor = bd.Proveedores.FirstOrDefault(b => b.IdProveedores == ProveedorMod.IdProveedores && b.Estado == true);
                    if (Proveedor == null)
                    {
                        MensajeError = "El Proveedor no existe o fue eliminado.";
                        return false;
                    }

                    // Actualizar datos
                    Proveedor.Nombre = ProveedorMod.Nombre.Trim();
                    Proveedor.NombreContacto = ProveedorMod.NombreContacto?.Trim();
                    Proveedor.Telefono = ProveedorMod.Telefono?.Trim();
                    Proveedor.Direccion = ProveedorMod.Direccion?.Trim();
                    Proveedor.Email = ProveedorMod.Email?.Trim();

                    bd.SaveChanges();
                    transaction.Commit();

                    MensajeError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MensajeError = "Error al modificar el Proveedor: " + ex.Message;
                    return false;
                }
            }
        }

        public bool EliminarProveedor(string IdProveedor, out string? MensajeError)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrEmpty(IdProveedor))
                    {
                        MensajeError = "El ID del proveedor no puede estar vacío.";
                        return false;
                    }

                    if (!Guid.TryParse(IdProveedor, out Guid guidIdProveedor))
                    {
                        MensajeError = "El formato del ID del proveedor no es válido.";
                        return false;
                    }

                    var Proveedor = bd.Proveedores.FirstOrDefault(b => b.IdProveedores == guidIdProveedor);
                    if (Proveedor == null)
                    {
                        MensajeError = "El Proveedor no existe o ya fue eliminado.";
                        return false;
                    }

                    if (Proveedor.Estado == false)
                    {
                        MensajeError = "El Proveedor ya está eliminado.";
                        return false;
                    }

                    // Eliminación lógica
                    Proveedor.Estado = false;

                    bd.SaveChanges();
                    transaction.Commit();

                    MensajeError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MensajeError = "Error al eliminar el Proveedor: " + ex.Message;
                    return false;
                }
            }
        }
        #endregion

        #region Validaciones
        private bool ExisteProveedorConNombre(string nombre)
        {
            return bd.Proveedores.Any(p =>
                p.Nombre.ToLower() == nombre.Trim().ToLower() &&
                p.Estado == true);
        }

        private bool ExisteProveedorConNombre(string nombre, Guid idProveedorActual)
        {
            return bd.Proveedores.Any(p =>
                p.Nombre.ToLower() == nombre.Trim().ToLower() &&
                p.IdProveedores != idProveedorActual &&
                p.Estado == true);
        }

        private bool ExisteProveedorConEmail(string email, Guid idProveedorActual)
        {
            return bd.Proveedores.Any(p =>
                p.Email != null &&
                p.Email.ToLower() == email.Trim().ToLower() &&
                p.IdProveedores != idProveedorActual &&
                p.Estado == true);
        }
        #endregion
    }
}
