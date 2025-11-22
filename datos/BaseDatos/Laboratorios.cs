using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class Laboratorios
{
    public Guid IdLaboratorios { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Medicamentos> Medicamentos { get; set; } = new List<Medicamentos>();
}
