using System;
using System.Collections.Generic;

namespace datos.BaseDatos;

public partial class Medicamentos
{
    public Guid IdMedicamentos { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal PrecioCompra { get; set; }

    public decimal PrecioVenta { get; set; }

    public int Stock { get; set; }

    public int StockMinimo { get; set; }

    public DateOnly FechaIngreso { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public string Lote { get; set; } = null!;

    public Guid IdProveedor { get; set; }

    public Guid IdCategoria { get; set; }

    public Guid IdLaboratorio { get; set; }

    public Guid IdPresentacion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<DetallesCompras> DetallesCompras { get; set; } = new List<DetallesCompras>();

    public virtual ICollection<DetallesDevolucionesClientes> DetallesDevolucionesClientes { get; set; } = new List<DetallesDevolucionesClientes>();

    public virtual ICollection<DetallesDevolucionesProveedores> DetallesDevolucionesProveedores { get; set; } = new List<DetallesDevolucionesProveedores>();

    public virtual ICollection<DetallesVentas> DetallesVentas { get; set; } = new List<DetallesVentas>();

    public virtual Categorias IdCategoriaNavigation { get; set; } = null!;

    public virtual Laboratorios IdLaboratorioNavigation { get; set; } = null!;

    public virtual Presentaciones IdPresentacionNavigation { get; set; } = null!;

    public virtual Proveedores IdProveedorNavigation { get; set; } = null!;

    public virtual ICollection<MovimientoStock> MovimientoStock { get; set; } = new List<MovimientoStock>();

    public virtual ICollection<Promociones> Promociones { get; set; } = new List<Promociones>();
}
