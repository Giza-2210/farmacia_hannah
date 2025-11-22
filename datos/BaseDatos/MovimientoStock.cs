using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class MovimientoStock
{
    public Guid IdMovimientoStock { get; set; }

    public Guid IdMedicamento { get; set; }

    public DateTime? FechaMovimiento { get; set; }

    public int TipoMovimiento { get; set; }

    public int CantidadAnterior { get; set; }

    public int CantidadNueva { get; set; }

    public int? Diferencia { get; set; }

    public Guid IdUsuario { get; set; }

    public string? Referencia { get; set; }

    public string? Observaciones { get; set; }

    public virtual Medicamentos IdMedicamentoNavigation { get; set; } = null!;

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
}
