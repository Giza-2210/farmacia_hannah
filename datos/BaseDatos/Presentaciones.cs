using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class Presentaciones
{
    public Guid IdPresentaciones { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Medicamentos> Medicamentos { get; set; } = new List<Medicamentos>();
}
