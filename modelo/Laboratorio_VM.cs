using System;
using System.Collections.Generic;
using System.Text;

namespace modelo
{
    public  class Laboratorio_VM
    {
        public Guid IdLaboratorios { get; set; }

        public string Nombre { get; set; } = null!;

        public bool? Estado { get; set; }
    }
}
