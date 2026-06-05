using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class GrupoPermiso : ComponentePermiso
    {
        private readonly List<ComponentePermiso> _hijos = new List<ComponentePermiso>();

        public override bool Tiene(string codigo)
        {
            if (this.Codigo == codigo) return true;
            foreach (var hijo in _hijos)
            {
                if (hijo.Tiene(codigo)) return true;
            }
            return false;
        }

        public override void Agregar(ComponentePermiso componente)
        {
            if (componente == null)
                throw new System.ArgumentNullException(nameof(componente));
            if (componente == this)
                throw new System.InvalidOperationException(
                    "Una familia no puede contenerse a sí misma.");
            _hijos.Add(componente);
        }

        public override void Quitar(ComponentePermiso componente)
        {
            if (!_hijos.Remove(componente))
                throw new System.InvalidOperationException(
                    "El componente no pertenece a esta familia.");
        }

        public override IReadOnlyList<ComponentePermiso> ObtenerHijos()
        {
            return _hijos.AsReadOnly();
        }
    }
}
