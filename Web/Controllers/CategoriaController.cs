using logica;
using Microsoft.AspNetCore.Mvc;
using modelo;

namespace Web.Controllers
{
    public class CategoriaController : Controller
    {

        private readonly Categoria_LN ln;

        public CategoriaController()
        {
            ln = new Categoria_LN();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EditarCategoria(string IdCategoria)
        {
            string? errorMessage = null;

            if (!Guid.TryParse(IdCategoria, out Guid guidCategoria))
            {
                return Json(new { error = "Invalid laboratorio ID format." });
            }

            Categoria_VM? categoria = ln.ConsultarCategoria(guidCategoria, out errorMessage);

            if (categoria == null)
                return Json(new { error = errorMessage });

            return PartialView(categoria);
        }

        #region Consultas

        [HttpPost]
        public IActionResult ObtenerListaCategorias()
        {
            List<Categoria_VM> Lista = new List<Categoria_VM>();
            string? errorMessage = null;

            bool exito = ln.ProporcionarListaCategorias(ref Lista, out errorMessage);

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

        public IActionResult AgregarCategoria([FromBody] Categoria_VM Cateogria)
        {
            string? errorMessage = null;

            bool resultado = ln.AgregarCategoria(Cateogria, out errorMessage);

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
        public IActionResult EliminarCategoria(string IdCategoria)
        {

            string? errorMessage = null;

            bool resultado = ln.EliminarCategoria(IdCategoria, out errorMessage);
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
        public IActionResult EditarCategoria([FromBody] Categoria_VM Categoria)
        {
            string? errorMessage = null;

            bool resultado = ln.ModificarCategoria(Categoria, out errorMessage);

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
