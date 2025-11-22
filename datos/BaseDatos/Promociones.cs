using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class Promociones
{
    public Guid IdPromocion { get; set; }

    public Guid IdMedicamento { get; set; }

    public string? Descripcion { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public decimal Descuento { get; set; }

    public bool? Activo { get; set; }

    public virtual Medicamentos IdMedicamentoNavigation { get; set; } = null!;
}
