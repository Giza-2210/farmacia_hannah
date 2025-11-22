using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class Empleados
{
    public Guid IdEmpleados { get; set; }

    public Guid IdIndividuo { get; set; }

    public Guid IdCargo { get; set; }

    public Guid? IdUsuario { get; set; }

    public Guid? IdRol { get; set; }

    public DateOnly? FechaContratacion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Compras> Compras { get; set; } = new List<Compras>();

    public virtual ICollection<DevolucionesClientes> DevolucionesClientes { get; set; } = new List<DevolucionesClientes>();

    public virtual ICollection<DevolucionesProveedores> DevolucionesProveedores { get; set; } = new List<DevolucionesProveedores>();

    public virtual CargosEmpleados IdCargoNavigation { get; set; } = null!;

    public virtual Individuos IdIndividuoNavigation { get; set; } = null!;

    public virtual Roles? IdRolNavigation { get; set; }

    public virtual Usuarios? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Ventas> Ventas { get; set; } = new List<Ventas>();
}
