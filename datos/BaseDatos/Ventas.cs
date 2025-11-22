using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class Ventas
{
    public Guid IdVentas { get; set; }

    public Guid IdEmpleado { get; set; }

    public Guid IdCliente { get; set; }

    public DateTime? FechaHora { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuestos { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<DetallesVentas> DetallesVentas { get; set; } = new List<DetallesVentas>();

    public virtual ICollection<DevolucionesClientes> DevolucionesClientes { get; set; } = new List<DevolucionesClientes>();

    public virtual Clientes IdClienteNavigation { get; set; } = null!;

    public virtual Empleados IdEmpleadoNavigation { get; set; } = null!;
}
