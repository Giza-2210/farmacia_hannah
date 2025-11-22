using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class DetallesCompras
{
    public Guid IdDetalleCompras { get; set; }

    public Guid IdCompra { get; set; }

    public Guid IdMedicamento { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal? SubTotal { get; set; }

    public virtual Compras IdCompraNavigation { get; set; } = null!;

    public virtual Medicamentos IdMedicamentoNavigation { get; set; } = null!;
}
