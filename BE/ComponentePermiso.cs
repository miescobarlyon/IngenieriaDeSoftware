using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public abstract class ComponentePermiso
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }

        public abstract bool Tiene(string codigo);

        public virtual void Agregar(ComponentePermiso componente)
        {
            throw new InvalidOperationException(
                "No se pueden agregar permisos a una patente.");
        }

        public virtual void Quitar(ComponentePermiso componente)
        {
            throw new InvalidOperationException(
                "No se pueden quitar permisos de una patente.");
        }

        public virtual IReadOnlyList<ComponentePermiso> ObtenerHijos()
        {
            throw new InvalidOperationException(
                "Una patente no contiene hijos.");
        }
    }
}
