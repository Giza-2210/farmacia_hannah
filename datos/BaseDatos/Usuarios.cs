using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class Usuarios
{
    public Guid IdUsuarios { get; set; }

    public string Usuario { get; set; } = null!;

    public string ContraseñaHash { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public DateTime? UltimoAcceso { get; set; }

    public Guid? IdIndividuo { get; set; }

    public bool? Activo { get; set; }

    public virtual Empleados? Empleados { get; set; }

    public virtual Individuos? IdIndividuoNavigation { get; set; }

    public virtual ICollection<MovimientoStock> MovimientoStock { get; set; } = new List<MovimientoStock>();
}
