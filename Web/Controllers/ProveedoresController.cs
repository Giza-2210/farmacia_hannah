using logica;
using Microsoft.AspNetCore.Mvc;
using modelo;

namespace Web.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly Proveedores_LN ln;

        public ProveedoresController()
        {
            ln = new Proveedores_LN();
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult EditarProveedor(string idproveedor)
        {
            string? errorMessage = null;

            if (!Guid.TryParse(idproveedor, out Guid guidIndividuo))
            {
                return Json(new { error = "Invalid Individuo ID format." });
            }

            Proveedores_VM? proveedor = ln.ConsultarProveedor(guidIndividuo, out errorMessage);

            if (proveedor == null)
                return Json(new { error = errorMessage });

            return PartialView(proveedor);
        }

        #region Consultas

        [HttpPost]
        public IActionResult ObtenerListaProveedores()
        {
            List<Proveedores_VM> ListaProveedores = new List<Proveedores_VM>();
            string? errorMessage = null;

            bool exito = ln.ProporcionarListaProveedores(ref ListaProveedores, out errorMessage);

            if (exito)
            {
                return Json(new { data = ListaProveedores });
            }
            else
            {
                return Json(new { error = errorMessage });
            }
            #endregion
        }
        #region CRUD
        [HttpPost]

        public IActionResult AgregarProveedor([FromBody] Proveedores_VM Proveedor)
        {
            string? errorMessage = null;

            bool resultado = ln.AgregarProveedor(Proveedor, out errorMessage);

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
        public IActionResult EliminarProveedor(string IdProveedor)
        {

            string? errorMessage = null;

            bool resultado = ln.EliminarProveedor(IdProveedor, out errorMessage);
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
        public IActionResult EditarProveedor([FromBody] Proveedores_VM Proveedor)
        {
            string? errorMessage = null;

            bool resultado = ln.ModificarProveedor(Proveedor, out errorMessage);

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
