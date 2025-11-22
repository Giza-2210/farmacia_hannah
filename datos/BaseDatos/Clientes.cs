using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class Clientes
{
    public Guid IdClientes { get; set; }

    public Guid IdIndividuo { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public int? PuntosAcumulados { get; set; }

    public bool? Activo { get; set; }

    public virtual Individuos IdIndividuoNavigation { get; set; } = null!;

    public virtual ICollection<Ventas> Ventas { get; set; } = new List<Ventas>();
}
