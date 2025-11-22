using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class Categorias
{
    public Guid IdCategorias { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Medicamentos> Medicamentos { get; set; } = new List<Medicamentos>();
}
