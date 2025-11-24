using datos.BaseDatos;
using modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace logica
{
    public  class Categoria_LN
    {

        private readonly Contexto bd;

        public Categoria_LN()
        {
            bd = new Contexto();
        }

        #region Consultas

        public Categoria_VM? ConsultarCategoria(Guid IdCategoria, out string? errorMessage)
        {
            try
            {
                // Validar que el ID no sea vacío
                if (IdCategoria == Guid.Empty)
                {
                    errorMessage = "El ID de la Categoría no puede estar vacío.";
                    return null;
                }

                var Categoria = bd.Categorias
                    .FirstOrDefault(b => b.IdCategorias == IdCategoria && b.Estado == true);

                if (Categoria == null)
                {
                    errorMessage = "La Categoría no existe o fue eliminada.";
                    return null;
                }

                var resultado = new Categoria_VM
                {
                    IdCategorias = Categoria.IdCategorias,
                    Nombre = Categoria.Nombre ?? string.Empty,
                    Descripcion = Categoria.Descripcion,
                    Estado = Categoria.Estado
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

        public bool ProporcionarListaCategorias(ref List<Categoria_VM> ListaCategorias, out string? errorMessage)
        {
            try
            {
                ListaCategorias = (from Cat in bd.Categorias
                                   where Cat.Estado == true
                                   select new Categoria_VM
                                   {
                                       IdCategorias = Cat.IdCategorias,
                                       Nombre = Cat.Nombre ?? string.Empty,
                                       Descripcion = Cat.Descripcion,
                                       Estado = Cat.Estado
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
        public bool AgregarCategoria(Categoria_VM Datos, out string? errorMessage)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    // Validar por nombre único
                    if (ExisteCategoriaConNombre(Datos.Nombre))
                    {
                        errorMessage = "Ya existe una categoría con el mismo nombre.";
                        return false;
                    }

                    var NuevaCategoria = new Categorias
                    {
                        IdCategorias = Guid.NewGuid(),
                        Nombre = Datos.Nombre.Trim(),
                        Descripcion = Datos.Descripcion?.Trim(),
                        Estado = true
                    };

                    bd.Categorias.Add(NuevaCategoria);
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

        public bool ModificarCategoria(Categoria_VM CategoriaMod, out string? MensajeError)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    // Validar nombre único
                    if (ExisteCategoriaConNombre(CategoriaMod.Nombre, CategoriaMod.IdCategorias))
                    {
                        MensajeError = "Ya existe una categoría con el mismo nombre.";
                        return false;
                    }

                    var Categoria = bd.Categorias.FirstOrDefault(b => b.IdCategorias == CategoriaMod.IdCategorias && b.Estado == true);
                    if (Categoria == null)
                    {
                        MensajeError = "La Categoría no existe o fue eliminada.";
                        return false;
                    }

                    // Actualizar datos
                    Categoria.Nombre = CategoriaMod.Nombre.Trim();
                    Categoria.Descripcion = CategoriaMod.Descripcion?.Trim();

                    bd.SaveChanges();
                    transaction.Commit();

                    MensajeError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MensajeError = "Error al modificar la Categoría: " + ex.Message;
                    return false;
                }
            }
        }

        public bool EliminarCategoria(string IdCategoria, out string? MensajeError)
        {
            using (var transaction = bd.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrEmpty(IdCategoria))
                    {
                        MensajeError = "El ID de la categoría no puede estar vacío.";
                        return false;
                    }

                    if (!Guid.TryParse(IdCategoria, out Guid guidIdCategoria))
                    {
                        MensajeError = "El formato del ID de la categoría no es válido.";
                        return false;
                    }

                    var Categoria = bd.Categorias.FirstOrDefault(b => b.IdCategorias == guidIdCategoria);
                    if (Categoria == null)
                    {
                        MensajeError = "La Categoría no existe o ya fue eliminada.";
                        return false;
                    }

                    if (Categoria.Estado == false)
                    {
                        MensajeError = "La Categoría ya está eliminada.";
                        return false;
                    }

                    // Eliminación lógica
                    Categoria.Estado = false;

                    bd.SaveChanges();
                    transaction.Commit();

                    MensajeError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MensajeError = "Error al eliminar la Categoría: " + ex.Message;
                    return false;
                }
            }
        }
        #endregion

        #region Validaciones
        private bool ExisteCategoriaConNombre(string nombre)
        {
            return bd.Categorias.Any(c =>
                c.Nombre.ToLower() == nombre.Trim().ToLower() &&
                c.Estado == true);
        }

        private bool ExisteCategoriaConNombre(string nombre, Guid idCategoriaActual)
        {
            return bd.Categorias.Any(c =>
                c.Nombre.ToLower() == nombre.Trim().ToLower() &&
                c.IdCategorias != idCategoriaActual &&
                c.Estado == true);
        }
        #endregion

    }
}
