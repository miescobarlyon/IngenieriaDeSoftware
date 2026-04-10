using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MP_Usuario : MAPPER<Usuario>
    {
        public override int Agregar(Usuario obj)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@nombre", obj.Nombre));
            parametros.Add(acceso.CrearParameter("@apellido",obj.Apellido));
            parametros.Add(acceso.CrearParameter("@user",obj.User));
            parametros.Add(acceso.CrearParameter("@pass",obj.PasswordHash));
            int res = acceso.Escribir("InsertarUsuario", parametros);
            acceso.Cerrar();
            return res;
        }

        public override int Eliminar(Usuario obj)
        {
            throw new NotImplementedException();
        }

        public override List<Usuario> Listar()
        {
            throw new NotImplementedException();
        }

        public override int Modificar(Usuario obj)
        {
            throw new NotImplementedException();
        }
    }
}
