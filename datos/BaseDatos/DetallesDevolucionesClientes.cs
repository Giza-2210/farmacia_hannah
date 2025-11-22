using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class DetallesDevolucionesClientes
{
    public Guid IdDetDevClientes { get; set; }

    public Guid IdDevolucion { get; set; }

    public Guid IdMedicamento { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public bool AfectaStock { get; set; }

    public decimal? SubTotal { get; set; }

    public virtual DevolucionesClientes IdDevolucionNavigation { get; set; } = null!;

    public virtual Medicamentos IdMedicamentoNavigation { get; set; } = null!;
}
