using System;
using System.Collections.Generic;
using System.Text;

namespace modelo
{
    public  class Categoria_VM
    {
        public Guid IdCategorias { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public bool? Estado { get; set; }

    }
}
