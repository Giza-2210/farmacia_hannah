using datos.BaseDatos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace logica
{
    public  class Home_LN
    {
        private readonly Contexto _bd;

        public Home_LN()
        {
            _bd = new Contexto();
        }

        //public int ObtenerVentasHoy()
        //{
        //    var hoy = DateTime.Today;
        //    return _bd.Ventas
        //        .Where(v => v.FechaVenta.Date == hoy && v.Estado == true)
        //        .Count();
        //}


        //public int ObtenerNumeroClientesRegistrados()
        //{
        //    return _bd.Individuos
        //        .Where(i =>  i.Activo == true)
        //        .Count();
        //}

        public int ObtenerMedicamentosProximosVencer()
        {
            var fechaLimite = DateOnly.FromDateTime(DateTime.Today.AddDays(30)); // Próximos 30 días
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            return _bd.Medicamentos
                .Where(m => m.FechaVencimiento <= fechaLimite &&
                           m.FechaVencimiento > hoy &&
                           m.Activo == true)
                .Count();
        }

        public int ObtenerTotalClientes()
        {
            return _bd.Individuos
                .Where(i => i.Activo == true)
                .Count();
        }

        public int ObtenerTotalProveedores()
        {
            return _bd.Proveedores
                .Where(p => p.Estado == true)
                .Count();
        }

        //public (string[] labels, int[] datos) ObtenerVentasUltimaSemana()
        //{
        //    var fechaInicio = DateTime.Today.AddDays(-6); // Últimos 7 días
        //    var fechaFin = DateTime.Today;

        //    var ventasPorDia = _bd.Ventas
        //        .Where(v => v.FechaVenta.Date >= fechaInicio &&
        //                   v.FechaVenta.Date <= fechaFin &&
        //                   v.Estado == true)
        //        .GroupBy(v => v.FechaVenta.Date)
        //        .Select(g => new { Fecha = g.Key, Total = g.Count() })
        //        .ToList();

        //    // Generar datos para todos los días de la semana
        //    var labels = new List<string>();
        //    var datos = new List<int>();

        //    for (int i = 0; i < 7; i++)
        //    {
        //        var fecha = fechaInicio.AddDays(i);
        //        var label = fecha.ToString("ddd", new System.Globalization.CultureInfo("es-ES"));
        //        labels.Add(char.ToUpper(label[0]) + label.Substring(1)); // Capitalizar

        //        var ventaDelDia = ventasPorDia.FirstOrDefault(v => v.Fecha == fecha.Date);
        //        datos.Add(ventaDelDia?.Total ?? 0);
        //    }

        //    return (labels.ToArray(), datos.ToArray());
        //}
    }
}
