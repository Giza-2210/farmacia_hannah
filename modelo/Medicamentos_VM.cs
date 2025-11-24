using System;
using System.Collections.Generic;
using System.Text;

namespace modelo
{
    public  class Medicamentos_VM
    {
        public Guid IdMedicamentos { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public decimal PrecioCompra { get; set; }

        public decimal PrecioVenta { get; set; }

        public int Stock { get; set; }

        public int StockMinimo { get; set; }

        public DateOnly FechaIngreso { get; set; }

        public DateOnly FechaVencimiento { get; set; }

        public string Lote { get; set; } = null!;

        public Guid IdProveedor { get; set; }

        public Guid IdCategoria { get; set; }

        public Guid IdLaboratorio { get; set; }

        public Guid IdPresentacion { get; set; }

        public bool? Activo { get; set; }
    }
}
