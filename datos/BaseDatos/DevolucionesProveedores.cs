using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class DevolucionesProveedores
{
    public Guid IdDevolucionProv { get; set; }

    public Guid IdCompra { get; set; }

    public Guid IdEmpleado { get; set; }

    public string? MotivoGeneral { get; set; }

    public DateTime? FechaHora { get; set; }

    public decimal TotalDevolucion { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<DetallesDevolucionesProveedores> DetallesDevolucionesProveedores { get; set; } = new List<DetallesDevolucionesProveedores>();

    public virtual Compras IdCompraNavigation { get; set; } = null!;

    public virtual Empleados IdEmpleadoNavigation { get; set; } = null!;
}
