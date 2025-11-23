using logica;
using Microsoft.AspNetCore.Mvc;
using modelo;
using System.Security.Claims;

namespace Web.Controllers
{
    public class IndividuoController : Controller
    {

        private readonly Individuo_LN ln;

        public IndividuoController()
        {
            ln = new Individuo_LN();
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Consultas
        [HttpPost]
        public IActionResult ObtenerListaIndividuos()
        {
            List<Individuo_VM> ListaIndividuo = new List<Individuo_VM>();
            string? errorMessage = null;


            bool exito = ln.ProporcionarListaIndividuos(ref ListaIndividuo, out errorMessage);

            if (exito)
            {

                return Json(new { data = ListaIndividuo });
            }
            else
            {
                return Json(new { error = errorMessage });
            }
            #endregion
        }

        #region CRUD
        [HttpPost]

        public IActionResult AgregarIndividuo([FromBody] Individuo_VM Individuo)
        {
            string? errorMessage = null;

            bool resultado = ln.AgregarIndividuo(Individuo, out errorMessage);

            if (resultado)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = errorMessage });
            }
        }


        //[HttpPost]
        //public IActionResult EliminarDepartamento(int IdDepartamento)
        //{
        //    string? errorMessage = null;
        //    var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(userIdStr))
        //    {
        //        return Json(new { success = false, error = "User identifier not found." });
        //    }

        //    bool resultado = ln.EliminarDepartamento(IdDepartamento, int.Parse(userIdStr), out errorMessage);

        //    if (resultado)
        //    {
        //        return Json(new { success = true });
        //    }
        //    else
        //    {
        //        return Json(new { success = false, error = errorMessage });
        //    }
        //}

        //[HttpPost]
        //public IActionResult EditarDepartamento([FromBody] Departamentos_VM Departamento)
        //{
        //    string? errorMessage = null;
        //    var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(userIdStr))
        //    {
        //        return Json(new { success = false, error = "User identifier not found." });
        //    }

        //    Departamento.ModificadoPor = int.Parse(userIdStr);
        //    bool resultado = ln.ModificarDepartamento(Departamento, out errorMessage);

        //    if (resultado)
        //    {
        //        return Json(new { success = true });
        //    }
        //    else
        //    {
        //        return Json(new { success = false, error = errorMessage });
        //    }
        //}
        #endregion


    }
}
