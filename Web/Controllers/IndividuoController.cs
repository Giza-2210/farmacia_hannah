using logica;
using Microsoft.AspNetCore.Mvc;
using modelo;

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


    }
}
