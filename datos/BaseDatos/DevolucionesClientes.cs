using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class DevolucionesClientes
{
    public Guid IdDevolucionClientes { get; set; }

    public Guid IdVenta { get; set; }

    public Guid IdEmpleado { get; set; }

    public DateTime? FechaHora { get; set; }

    public string? MotivoGeneral { get; set; }

    public decimal TotalDevolucion { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<DetallesDevolucionesClientes> DetallesDevolucionesClientes { get; set; } = new List<DetallesDevolucionesClientes>();

    public virtual Empleados IdEmpleadoNavigation { get; set; } = null!;

    public virtual Ventas IdVentaNavigation { get; set; } = null!;
}
