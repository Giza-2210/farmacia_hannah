using System;
using System.Collections.Generic;
using System.Text;

namespace modelo
{
    public  class Individuo_VM
    {
        public Guid IdIndividuos { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string? Telefono { get; set; }

        public string? Direccion { get; set; }

        public string? Email { get; set; }

        public DateOnly? FechaRegistro { get; set; }

        public bool? Activo { get; set; }
    }
}
