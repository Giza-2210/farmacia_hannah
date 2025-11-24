using logica;
using Microsoft.AspNetCore.Mvc;
using modelo;

namespace Web.Controllers
{
    public class PresentacionesController : Controller
    {

        private readonly Presentaciones_LN ln;

        public PresentacionesController()
        {
            ln = new Presentaciones_LN();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EditarPresentaciones(string IdPresentacion)
        {
            string? errorMessage = null;

            if (!Guid.TryParse(IdPresentacion, out Guid guidPresentacion))
            {
                return Json(new { error = "Invalid laboratorio ID format." });
            }

            Presentacion_VM? presentacion = ln.ConsultarPresentacion(guidPresentacion, out errorMessage);

            if (presentacion == null)
                return Json(new { error = errorMessage });

            return PartialView(presentacion);
        }

        #region Consultas

        [HttpPost]
        public IActionResult ObtenerListaPresentaciones()
        {
            List<Presentacion_VM> Lista = new List<Presentacion_VM>();
            string? errorMessage = null;

            bool exito = ln.ProporcionarListaPresentaciones(ref Lista, out errorMessage);

            if (exito)
            {
                return Json(new { data = Lista });
            }
            else
            {
                return Json(new { error = errorMessage });
            }
            #endregion
        }
        #region CRUD
        [HttpPost]

        public IActionResult AgregarPresentacion([FromBody] Presentacion_VM Presentacion)
        {
            string? errorMessage = null;

            bool resultado = ln.AgregarPresentacion(Presentacion, out errorMessage);

            if (resultado)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = errorMessage });
            }
        }


        [HttpPost]
        public IActionResult EliminarPresentacion(string IdPresentacion)
        {

            string? errorMessage = null;

            bool resultado = ln.EliminarPresentacion(IdPresentacion, out errorMessage);
            if (resultado == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = errorMessage });
            }
        }

        [HttpPost]
        public IActionResult ModificarPresentacion([FromBody] Presentacion_VM presentacion)
        {
            string? errorMessage = null;

            bool resultado = ln.ModificarPresentacion(presentacion, out errorMessage);

            if (resultado)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = errorMessage });
            }
        }
        #endregion


    }
}
