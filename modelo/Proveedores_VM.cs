using System;
using System.Collections.Generic;
using System.Text;

namespace modelo
{
    public  class Proveedores_VM
    {
        public Guid IdProveedores { get; set; }

        public string Nombre { get; set; } = null!;

        public string? NombreContacto { get; set; }

        public string? Telefono { get; set; }

        public string? Direccion { get; set; }

        public string? Email { get; set; }
    }
}
