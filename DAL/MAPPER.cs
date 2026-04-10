using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public abstract class MAPPER<T>
    {
        internal Acceso acceso;
        public abstract int Agregar(T obj);
        public abstract int Modificar(T obj);
        public abstract int Eliminar(T obj);
        public abstract List<T> Listar();
    }
}
