using logica;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly Home_LN _homeLN;

        public HomeController()
        {
            _homeLN = new Home_LN();
        }


        public IActionResult Index()
        {
            // Ejemplos: reemplaza con datos reales desde tu base de datos
            //ViewBag.VentasHoy = 25;
            ViewBag.MedicamentosProximos = _homeLN.ObtenerMedicamentosProximosVencer(); ;
            ViewBag.Clientes = _homeLN.ObtenerTotalClientes();
            ViewBag.Proveedores = _homeLN.ObtenerTotalProveedores();

            // Datos para la gráfica
            ViewBag.LabelsDias = new[] { "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb", "Dom" };
            ViewBag.VentasPorDia = new[] { 10, 14, 8, 20, 25, 18, 12 };

            return View();
        }


        public IActionResult Login()
        {


            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
