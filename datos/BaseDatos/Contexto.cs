using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace datos.BaseDatos;

public partial class Contexto : DbContext
{
    public Contexto()
    {
    }

    public Contexto(DbContextOptions<Contexto> options)
        : base(options)
    {
    }

    public virtual DbSet<CargosEmpleados> CargosEmpleados { get; set; }

    public virtual DbSet<Categorias> Categorias { get; set; }

    public virtual DbSet<Clientes> Clientes { get; set; }

    public virtual DbSet<Compras> Compras { get; set; }

    public virtual DbSet<DetallesCompras> DetallesCompras { get; set; }

    public virtual DbSet<DetallesDevolucionesClientes> DetallesDevolucionesClientes { get; set; }

    public virtual DbSet<DetallesDevolucionesProveedores> DetallesDevolucionesProveedores { get; set; }

    public virtual DbSet<DetallesVentas> DetallesVentas { get; set; }

    public virtual DbSet<DevolucionesClientes> DevolucionesClientes { get; set; }

    public virtual DbSet<DevolucionesProveedores> DevolucionesProveedores { get; set; }

    public virtual DbSet<Empleados> Empleados { get; set; }

    public virtual DbSet<Individuos> Individuos { get; set; }

    public virtual DbSet<Laboratorios> Laboratorios { get; set; }

    public virtual DbSet<Medicamentos> Medicamentos { get; set; }

    public virtual DbSet<MovimientoStock> MovimientoStock { get; set; }

    public virtual DbSet<Presentaciones> Presentaciones { get; set; }

    public virtual DbSet<Promociones> Promociones { get; set; }

    public virtual DbSet<Proveedores> Proveedores { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    public virtual DbSet<Ventas> Ventas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=sql.bsite.net\\MSSQL2016; Database=quesito_farmacia; User ID=quesito_farmacia; Password='Tobby2025'; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CargosEmpleados>(entity =>
        {
            entity.HasKey(e => e.IdCargoEmpleados).HasName("PK__Cargos_E__F2DE6F0F6E1AB004");

            entity.ToTable("Cargos_Empleados");

            entity.HasIndex(e => e.NombreCargo, "UQ__Cargos_E__6DFBEFDCE130DD98").IsUnique();

            entity.Property(e => e.IdCargoEmpleados)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_CargoEmpleados");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NombreCargo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Nombre_Cargo");
            entity.Property(e => e.SalarioBase)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Salario_Base");
        });

        modelBuilder.Entity<Categorias>(entity =>
        {
            entity.HasKey(e => e.IdCategorias).HasName("PK__Categori__A48D955116AABB94");

            entity.Property(e => e.IdCategorias)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_Categorias");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Clientes>(entity =>
        {
            entity.HasKey(e => e.IdClientes).HasName("PK__Clientes__88A514CD6B4CBE0D");

            entity.HasIndex(e => e.IdIndividuo, "UQ__Clientes__2176DA34897199B8").IsUnique();

            entity.Property(e => e.IdClientes)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_Clientes");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("Fecha_Registro");
            entity.Property(e => e.IdIndividuo).HasColumnName("ID_Individuo");
            entity.Property(e => e.PuntosAcumulados)
                .HasDefaultValue(0)
                .HasColumnName("Puntos_Acumulados");

            entity.HasOne(d => d.IdIndividuoNavigation).WithOne(p => p.Clientes)
                .HasForeignKey<Clientes>(d => d.IdIndividuo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_Individuo");
        });

        modelBuilder.Entity<Compras>(entity =>
        {
            entity.HasKey(e => e.IdCompras).HasName("PK__Compras__B82691E8901D2FFD");

            entity.ToTable(tb => tb.HasTrigger("TR_Compras_ActualizarStock"));

            entity.Property(e => e.IdCompras)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_Compras");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Hora");
            entity.Property(e => e.IdEmpleado).HasColumnName("ID_Empleado");
            entity.Property(e => e.IdProveedor).HasColumnName("ID_Proveedor");
            entity.Property(e => e.Impuestos).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compra_Empleado");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compra_Proveedor");
        });

        modelBuilder.Entity<DetallesCompras>(entity =>
        {
            entity.HasKey(e => e.IdDetalleCompras).HasName("PK__Detalles__BB3AD922625BC883");

            entity.ToTable("Detalles_Compras");

            entity.Property(e => e.IdDetalleCompras)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_DetalleCompras");
            entity.Property(e => e.IdCompra).HasColumnName("ID_Compra");
            entity.Property(e => e.IdMedicamento).HasColumnName("ID_Medicamento");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Precio_Unitario");
            entity.Property(e => e.SubTotal)
                .HasComputedColumnSql("([Cantidad]*[Precio_Unitario])", true)
                .HasColumnType("decimal(21, 2)");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetallesCompras)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetCompra_Compra");

            entity.HasOne(d => d.IdMedicamentoNavigation).WithMany(p => p.DetallesCompras)
                .HasForeignKey(d => d.IdMedicamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetCompra_Medic");
        });

        modelBuilder.Entity<DetallesDevolucionesClientes>(entity =>
        {
            entity.HasKey(e => e.IdDetDevClientes).HasName("PK__Detalles__61003EADFB60EF9F");

            entity.ToTable("Detalles_Devoluciones_Clientes");

            entity.Property(e => e.IdDetDevClientes)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_DetDevClientes");
            entity.Property(e => e.AfectaStock)
                .HasDefaultValue(true)
                .HasColumnName("Afecta_Stock");
            entity.Property(e => e.IdDevolucion).HasColumnName("ID_Devolucion");
            entity.Property(e => e.IdMedicamento).HasColumnName("ID_Medicamento");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Precio_Unitario");
            entity.Property(e => e.SubTotal)
                .HasComputedColumnSql("([Cantidad]*[Precio_Unitario])", true)
                .HasColumnType("decimal(21, 2)");

            entity.HasOne(d => d.IdDevolucionNavigation).WithMany(p => p.DetallesDevolucionesClientes)
                .HasForeignKey(d => d.IdDevolucion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetDevCli_Dev");

            entity.HasOne(d => d.IdMedicamentoNavigation).WithMany(p => p.DetallesDevolucionesClientes)
                .HasForeignKey(d => d.IdMedicamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetDevCli_Medic");
        });

        modelBuilder.Entity<DetallesDevolucionesProveedores>(entity =>
        {
            entity.HasKey(e => e.IdDetDevProv).HasName("PK__Detalles__F8EB6220D6225AB9");

            entity.ToTable("Detalles_Devoluciones_Proveedores");

            entity.Property(e => e.IdDetDevProv)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_DetDevProv");
            entity.Property(e => e.AfectaStock)
                .HasDefaultValue(true)
                .HasColumnName("Afecta_Stock");
            entity.Property(e => e.IdDevolucion).HasColumnName("ID_Devolucion");
            entity.Property(e => e.IdMedicamento).HasColumnName("ID_Medicamento");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Precio_Unitario");
            entity.Property(e => e.SubTotal)
                .HasComputedColumnSql("([Cantidad]*[Precio_Unitario])", true)
                .HasColumnType("decimal(21, 2)");

            entity.HasOne(d => d.IdDevolucionNavigation).WithMany(p => p.DetallesDevolucionesProveedores)
                .HasForeignKey(d => d.IdDevolucion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetDevProv_Dev");

            entity.HasOne(d => d.IdMedicamentoNavigation).WithMany(p => p.DetallesDevolucionesProveedores)
                .HasForeignKey(d => d.IdMedicamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetDevProv_Medic");
        });

        modelBuilder.Entity<DetallesVentas>(entity =>
        {
            entity.HasKey(e => e.IdDetalleVentas).HasName("PK__Detalles__E2693B1F0BB129F5");

            entity.ToTable("Detalles_Ventas");

            entity.Property(e => e.IdDetalleVentas)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_DetalleVentas");
            entity.Property(e => e.IdMedicamento).HasColumnName("ID_Medicamento");
            entity.Property(e => e.IdVenta).HasColumnName("ID_Venta");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Precio_Unitario");
            entity.Property(e => e.SubTotal)
                .HasComputedColumnSql("([Cantidad]*[Precio_Unitario])", true)
                .HasColumnType("decimal(21, 2)");

            entity.HasOne(d => d.IdMedicamentoNavigation).WithMany(p => p.DetallesVentas)
                .HasForeignKey(d => d.IdMedicamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetVenta_Medic");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetallesVentas)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetVenta_Venta");
        });

        modelBuilder.Entity<DevolucionesClientes>(entity =>
        {
            entity.HasKey(e => e.IdDevolucionClientes).HasName("PK__Devoluci__FB2E12C9BB5A8F27");

            entity.ToTable("Devoluciones_Clientes", tb => tb.HasTrigger("TR_DevolucionesClientes_ActualizarStock"));

            entity.Property(e => e.IdDevolucionClientes)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_DevolucionClientes");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Hora");
            entity.Property(e => e.IdEmpleado).HasColumnName("ID_Empleado");
            entity.Property(e => e.IdVenta).HasColumnName("ID_Venta");
            entity.Property(e => e.MotivoGeneral)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("Motivo_General");
            entity.Property(e => e.TotalDevolucion)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Total_Devolucion");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.DevolucionesClientes)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DevCli_Empleado");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DevolucionesClientes)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DevCli_Venta");
        });

        modelBuilder.Entity<DevolucionesProveedores>(entity =>
        {
            entity.HasKey(e => e.IdDevolucionProv).HasName("PK__Devoluci__4742C7F74D096954");

            entity.ToTable("Devoluciones_Proveedores", tb => tb.HasTrigger("TR_DevolucionesProveedores_ActualizarStock"));

            entity.Property(e => e.IdDevolucionProv)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_DevolucionProv");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Hora");
            entity.Property(e => e.IdCompra).HasColumnName("ID_Compra");
            entity.Property(e => e.IdEmpleado).HasColumnName("ID_Empleado");
            entity.Property(e => e.MotivoGeneral)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("Motivo_General");
            entity.Property(e => e.TotalDevolucion)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Total_Devolucion");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DevolucionesProveedores)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DevProv_Compra");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.DevolucionesProveedores)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DevProv_Empleado");
        });

        modelBuilder.Entity<Empleados>(entity =>
        {
            entity.HasKey(e => e.IdEmpleados).HasName("PK__Empleado__EFACC92721C694A9");

            entity.HasIndex(e => e.IdIndividuo, "UQ__Empleado__2176DA34210E617B").IsUnique();

            entity.HasIndex(e => e.IdUsuario, "UQ__Empleado__DE4431C42FEC2532").IsUnique();

            entity.Property(e => e.IdEmpleados)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_Empleados");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaContratacion)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("Fecha_Contratacion");
            entity.Property(e => e.IdCargo).HasColumnName("ID_Cargo");
            entity.Property(e => e.IdIndividuo).HasColumnName("ID_Individuo");
            entity.Property(e => e.IdRol).HasColumnName("ID_Rol");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");

            entity.HasOne(d => d.IdCargoNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdCargo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Empleado_Cargo");

            entity.HasOne(d => d.IdIndividuoNavigation).WithOne(p => p.Empleados)
                .HasForeignKey<Empleados>(d => d.IdIndividuo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Empleado_Individuo");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK_Empleado_Rol");

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.Empleados)
                .HasForeignKey<Empleados>(d => d.IdUsuario)
                .HasConstraintName("FK_Empleado_Usuario");
        });

        modelBuilder.Entity<Individuos>(entity =>
        {
            entity.HasKey(e => e.IdIndividuos).HasName("PK__Individu__CF7C47B5D648DE54");

            entity.Property(e => e.IdIndividuos)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_Individuos");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("Fecha_Registro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Laboratorios>(entity =>
        {
            entity.HasKey(e => e.IdLaboratorios).HasName("PK__Laborato__7297A33A02B5831B");

            entity.Property(e => e.IdLaboratorios)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_Laboratorios");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Medicamentos>(entity =>
        {
            entity.HasKey(e => e.IdMedicamentos).HasName("PK__Medicame__57E228A1C24EC0E4");

            entity.HasIndex(e => e.IdCategoria, "IDX_Medicamentos_Categoria");

            entity.HasIndex(e => e.IdLaboratorio, "IDX_Medicamentos_Lab");

            entity.HasIndex(e => e.IdPresentacion, "IDX_Medicamentos_Presentacion");

            entity.HasIndex(e => e.IdProveedor, "IDX_Medicamentos_Proveedor");

            entity.Property(e => e.IdMedicamentos)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_Medicamentos");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FechaIngreso)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("Fecha_Ingreso");
            entity.Property(e => e.FechaVencimiento).HasColumnName("Fecha_Vencimiento");
            entity.Property(e => e.IdCategoria).HasColumnName("ID_Categoria");
            entity.Property(e => e.IdLaboratorio).HasColumnName("ID_Laboratorio");
            entity.Property(e => e.IdPresentacion).HasColumnName("ID_Presentacion");
            entity.Property(e => e.IdProveedor).HasColumnName("ID_Proveedor");
            entity.Property(e => e.Lote)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrecioCompra)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Precio_Compra");
            entity.Property(e => e.PrecioVenta)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Precio_Venta");
            entity.Property(e => e.StockMinimo)
                .HasDefaultValue(10)
                .HasColumnName("Stock_Minimo");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Medicamentos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicamento_Categoria");

            entity.HasOne(d => d.IdLaboratorioNavigation).WithMany(p => p.Medicamentos)
                .HasForeignKey(d => d.IdLaboratorio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicamento_Lab");

            entity.HasOne(d => d.IdPresentacionNavigation).WithMany(p => p.Medicamentos)
                .HasForeignKey(d => d.IdPresentacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicamento_Presentacion");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Medicamentos)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicamento_Proveedor");
        });

        modelBuilder.Entity<MovimientoStock>(entity =>
        {
            entity.HasKey(e => e.IdMovimientoStock).HasName("PK__Movimien__772BE75466D17E9E");

            entity.ToTable("Movimiento_Stock");

            entity.HasIndex(e => e.FechaMovimiento, "IDX_MovimientoStock_Fecha");

            entity.HasIndex(e => e.IdMedicamento, "IDX_MovimientoStock_Medicamento");

            entity.Property(e => e.IdMovimientoStock)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_MovimientoStock");
            entity.Property(e => e.Diferencia).HasComputedColumnSql("([CantidadNueva]-[CantidadAnterior])", false);
            entity.Property(e => e.FechaMovimiento)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdMedicamento).HasColumnName("ID_Medicamento");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Referencia)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdMedicamentoNavigation).WithMany(p => p.MovimientoStock)
                .HasForeignKey(d => d.IdMedicamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovimientoStock_Medicamento");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.MovimientoStock)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovimientoStock_Usuario");
        });

        modelBuilder.Entity<Presentaciones>(entity =>
        {
            entity.HasKey(e => e.IdPresentaciones).HasName("PK__Presenta__A4B1488E12EFAACE");

            entity.Property(e => e.IdPresentaciones)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_Presentaciones");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Promociones>(entity =>
        {
            entity.HasKey(e => e.IdPromocion).HasName("PK__Promocio__577F2CA6C4410121");

            entity.Property(e => e.IdPromocion)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_Promocion");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Descuento).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.FechaFin).HasColumnName("Fecha_Fin");
            entity.Property(e => e.FechaInicio).HasColumnName("Fecha_Inicio");
            entity.Property(e => e.IdMedicamento).HasColumnName("ID_Medicamento");

            entity.HasOne(d => d.IdMedicamentoNavigation).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.IdMedicamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Promo_Medicamento");
        });

        modelBuilder.Entity<Proveedores>(entity =>
        {
            entity.HasKey(e => e.IdProveedores).HasName("PK__Proveedo__4A6002B56900F7F7");

            entity.Property(e => e.IdProveedores)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_Proveedores");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreContacto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Nombre_Contacto");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__202AD2204A748274");

            entity.HasIndex(e => e.NombreRol, "UQ__Roles__320FDA7D5EF55E6F").IsUnique();

            entity.Property(e => e.IdRol)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_Rol");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Nombre_Rol");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.IdUsuarios).HasName("PK__Usuarios__FFE154A1BF5905B8");

            entity.HasIndex(e => e.Usuario, "UQ__Usuarios__E3237CF7926496CA").IsUnique();

            entity.Property(e => e.IdUsuarios)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_Usuarios");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ContraseñaHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Contraseña_Hash");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Creacion");
            entity.Property(e => e.IdIndividuo).HasColumnName("ID_Individuo");
            entity.Property(e => e.UltimoAcceso)
                .HasColumnType("datetime")
                .HasColumnName("Ultimo_Acceso");
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdIndividuoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdIndividuo)
                .HasConstraintName("FK_Usuario_Individuo");
        });

        modelBuilder.Entity<Ventas>(entity =>
        {
            entity.HasKey(e => e.IdVentas).HasName("PK__Ventas__508DCF49DFD9D075");

            entity.ToTable(tb => tb.HasTrigger("TR_Ventas_ActualizarStock"));

            entity.Property(e => e.IdVentas)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID_Ventas");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Hora");
            entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");
            entity.Property(e => e.IdEmpleado).HasColumnName("ID_Empleado");
            entity.Property(e => e.Impuestos).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Ventas)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Venta_Cliente");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Ventas)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Venta_Empleado");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
