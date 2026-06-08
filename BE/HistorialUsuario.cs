using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HistorialUsuario
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string User { get; set; }
        public int Borrado { get; set; }
        public int? IdiomaId { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int ModificadorId { get; set; }
        public string Accion { get; set; }

        public string ModificadorNombre { get; set; }

    }
}