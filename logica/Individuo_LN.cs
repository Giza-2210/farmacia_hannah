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

        //public Individuo_VM? ConsultarIndividuo(int IdIndividuo, out string? errorMessage)
        //{
        //    try
        //    {
        //        // Validar que el ID no sea 0
        //        if (IdIndividuo == 0)
        //        {
        //            errorMessage = "El ID de el Individuo no puede ser 0.";
        //            return null;
        //        }

        //        // Convert int IdIndividuo to Guid for comparison
        //        Guid guidIdIndividuo = new Guid(IdIndividuo.ToString("D32"));

        //        var Departamento = bd.Individuos
        //            .FirstOrDefault(b => b.IdIndividuos == guidIdIndividuo);

        //        if (Departamento == null)
        //        {
        //            errorMessage = "El Departamento no existe.";
        //            return null;
        //        }

        //        var resultado = new Individuo_VM
        //        {
        //            IdDepartamento = Departamento.IdDepartamento,
        //            NombreDepartamento = Departamento.NombreDepartamento ?? string.Empty,
        //        };

        //        errorMessage = null;
        //        return resultado;
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage = ex.Message;
        //        return null;
        //    }
        //}

        //public bool ProporcionarListaMunicipioXDepartamento(int IdDepartamento, ref List<Municipios_VM> ListaMunicipios, out string? errorMessage)
        //{
        //    ListaMunicipios = (from M in bd.Municipios
        //                       join D in bd.Departamentos on M.IdDepartamento equals D.IdDepartamento
        //                       where M.EstadoMunicipio == true && M.IdDepartamento == IdDepartamento
        //                       select new Municipios_VM
        //                       {
        //                           IdMunicipio = M.IdMunicipio,
        //                           NombreMunicipio = M.NombreMunicipio ?? string.Empty
        //                       }).ToList();
        //    errorMessage = null;
        //    return true;
        //}

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

        #endregion
    }
}
