using System;
using System.Collections.Generic;
using System.Text;

namespace modelo
{
    public  class Presentacion_VM
    {
        public Guid IdPresentaciones { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public bool? Estado { get; set; }
    }
}
