using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class Roles
{
    public Guid IdRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Empleados> Empleados { get; set; } = new List<Empleados>();
}
