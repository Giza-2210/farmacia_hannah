using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class Proveedores
{
    public Guid IdProveedores { get; set; }

    public string Nombre { get; set; } = null!;

    public string? NombreContacto { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? Email { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Compras> Compras { get; set; } = new List<Compras>();

    public virtual ICollection<Medicamentos> Medicamentos { get; set; } = new List<Medicamentos>();
}
