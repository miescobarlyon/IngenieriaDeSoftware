using BE;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
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
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@id", obj.Id));
            int res = acceso.Escribir("EliminarUsuario", parametros);
            acceso.Cerrar();
            return res;
        }

        public override List<Usuario> Listar()
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<BE.Usuario> u = new List<Usuario>();
            acceso.Cerrar();
            return u;
        }


        //acá puse dos parametros cualquiera, porque es una funcion general, tipo no se ni quien va a modificar.
        public override int Modificar(Usuario obj)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@id", obj.Id));
            parametros.Add(acceso.CrearParameter("@nombre", obj.Nombre));
            parametros.Add(acceso.CrearParameter("@apellido", obj.Apellido)); 
            int res = acceso.Escribir("ModificarUsuario", parametros);
            acceso.Cerrar();
            return res;
        }

        public string TraerPass(string user)
        {
            acceso = new Acceso();
            try
            {
                acceso.Abrir();
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParameter("@user", user)
                };

                DataTable dt = acceso.Leer("TraerPass", parametros);
                

                DataRow dr = dt.Rows[0];

                string pass = dr.IsNull(0) ? null : dr[6].ToString();


              

                return pass;
            }
            finally
            {
                acceso.Cerrar();
            }
        }
    }
}
