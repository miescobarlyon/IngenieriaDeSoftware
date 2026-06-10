using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class PermisoSimple : ComponentePermiso
    {
        public override bool Tiene(string codigo)
        {
            return this.Codigo == codigo;
        }
    }
}
