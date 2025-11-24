using logica;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using modelo;

namespace Web.Controllers
{
    public class MedicamentosController : Controller
    {

        private readonly Medicamentos_LN ln;

        public MedicamentosController()
        {
            ln = new Medicamentos_LN();
        }

        public IActionResult Index()
        {

            string? MsgError;

            List<Proveedores_VM> Proveedores = new();
            List<Laboratorio_VM> Laboratorios = new();
            List<Presentacion_VM> Presentaciones = new();
            List<Categoria_VM> Categorias = new();

            // Cargar lista de Proveedores
            if (new Proveedores_LN().ProporcionarListaProveedores(ref Proveedores, out MsgError))
            {
                ViewBag.ListProveedores = new SelectList(
                    Proveedores,
                    "IdProveedores",
                    "Nombre"
                );
            }
            else
            {
                // Opcional: Manejar el error o dejar lista vacía
                ViewBag.ListProveedores = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            // Cargar lista de Laboratorios
            if (new Laboratorio_LN().ProporcionarListaLaboratorios(ref Laboratorios, out MsgError))
            {
                ViewBag.ListLaboratorios = new SelectList(
                    Laboratorios,
                    "IdLaboratorios",
                    "Nombre"
                );
            }
            else
            {
                ViewBag.ListLaboratorios = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            // Cargar lista de Presentaciones
            if (new Presentaciones_LN().ProporcionarListaPresentaciones(ref Presentaciones, out MsgError))
            {
                ViewBag.ListPresentaciones = new SelectList(
                    Presentaciones,
                    "IdPresentaciones",
                    "Nombre"
                );
            }
            else
            {
                ViewBag.ListPresentaciones = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            // Cargar lista de Categorías
            if (new Categoria_LN().ProporcionarListaCategorias(ref Categorias, out MsgError))
            {
                ViewBag.ListCategorias = new SelectList(
                    Categorias,
                    "IdCategorias",
                    "Nombre"
                );
            }
            else
            {
                ViewBag.ListCategorias = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            return View();
        }

        #region Consultas

        [HttpPost]
        public IActionResult ObtenerListaMedicamentos()
        {
            List<Medicamentos_VM> Lista = new List<Medicamentos_VM>();
            string? errorMessage = null;

            bool exito = ln.ProporcionarListaMedicamentos(ref Lista, out errorMessage);

            if (exito)
            {
                return Json(new { data = Lista });
            }
            else
            {
                return Json(new { error = errorMessage });
            }
        }

        [HttpPost]
        public IActionResult ConsultarMedicamento(Guid IdMedicamento)
        {
            try
            {
                var resultado = ln.ConsultarMedicamento(IdMedicamento, out string? errorMessage);

                if (resultado != null)
                {
                    return Json(new { success = true, data = resultado });
                }
                else
                {
                    return Json(new { success = false, error = errorMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
#endregion

        #region CRUD
        [HttpPost]

        public IActionResult AgregarMedicamento([FromBody] Medicamentos_VM Medicamento)
        {
            string? errorMessage = null;

            bool resultado = ln.AgregarMedicamento(Medicamento, out errorMessage);

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
        public IActionResult EliminarMedicamento(string IdMedicamento)
        {

            string? errorMessage = null;

            bool resultado = ln.EliminarMedicamento(IdMedicamento, out errorMessage);
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
        public IActionResult ModificarMedicamento([FromBody] Medicamentos_VM Medicamento)
        {
            string? errorMessage = null;

            bool resultado = ln.ModificarMedicamento(Medicamento, out errorMessage);

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
