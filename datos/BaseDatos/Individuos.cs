using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class Individuos
{
    public Guid IdIndividuos { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? Email { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public bool? Activo { get; set; }

    public virtual Clientes? Clientes { get; set; }

    public virtual Empleados? Empleados { get; set; }

    public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
}
