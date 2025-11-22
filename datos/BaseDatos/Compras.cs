using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class Compras
{
    public Guid IdCompras { get; set; }

    public Guid IdProveedor { get; set; }

    public Guid IdEmpleado { get; set; }

    public DateTime? FechaHora { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuestos { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<DetallesCompras> DetallesCompras { get; set; } = new List<DetallesCompras>();

    public virtual ICollection<DevolucionesProveedores> DevolucionesProveedores { get; set; } = new List<DevolucionesProveedores>();

    public virtual Empleados IdEmpleadoNavigation { get; set; } = null!;

    public virtual Proveedores IdProveedorNavigation { get; set; } = null!;
}
