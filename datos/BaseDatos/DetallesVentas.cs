using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class DetallesVentas
{
    public Guid IdDetalleVentas { get; set; }

    public Guid IdVenta { get; set; }

    public Guid IdMedicamento { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal? SubTotal { get; set; }

    public virtual Medicamentos IdMedicamentoNavigation { get; set; } = null!;

    public virtual Ventas IdVentaNavigation { get; set; } = null!;
}
