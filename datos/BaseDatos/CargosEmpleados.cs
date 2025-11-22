using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class CargosEmpleados
{
    public Guid IdCargoEmpleados { get; set; }

    public string NombreCargo { get; set; } = null!;

    public decimal SalarioBase { get; set; }

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Empleados> Empleados { get; set; } = new List<Empleados>();
}
