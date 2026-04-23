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
            List<BE.Usuario> usuarios = new List<BE.Usuario>();

            acceso = new Acceso();

            acceso.Abrir();
            DataTable tabla = acceso.Leer("ListarUsuario");
            acceso.Cerrar();

            foreach (DataRow row in tabla.Rows)
            {
                BE.Usuario usuario = new BE.Usuario();

                usuario.Id = int.Parse(row["usuario_id"].ToString());
                usuario.Nombre = row["nombre"].ToString();
                usuario.Apellido = row["apellido"].ToString();
                usuario.User = row["usuario"].ToString();
                usuario.PasswordHash = row["pass_hash"].ToString();
                usuario.Salt = row["salt"].ToString();
                usuario.Borrado = int.Parse(row["borrado"].ToString());

                usuarios.Add(usuario);
            }

            return usuarios;
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

        public int AgregarIntento(Usuario obj)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@id", obj.Id));
            int res = acceso.Escribir("AgregarIntento", parametros);
            acceso.Cerrar();
            return res;
        }

        public int ReestablecerIntentos(Usuario obj)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@id", obj.Id));
            int res = acceso.Escribir("ReestablecerIntentos", parametros);
            acceso.Cerrar();
            return res;
        }

        public Tuple<string, string> TraerPass(string user)
        {
            acceso = new Acceso();
            try
            {
                acceso.Abrir();

                var parametros = new List<SqlParameter>
                {
                    acceso.CrearParameter("@user", user)
                };

                DataTable dt = acceso.Leer("TraerPass", parametros);
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                DataRow dr = dt.Rows[0];

                string hash = dr.IsNull(0) ? null : Convert.ToString(dr[0]);
                string salt = dr.IsNull(1) ? null : Convert.ToString(dr[1]);

                return Tuple.Create(hash, salt);
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        public BE.Usuario TraerUsuario(string user)
        {
            acceso = new Acceso();
            try
            {
                acceso.Abrir();
                var parametros = new List<SqlParameter>
                {
                    acceso.CrearParameter("@user", user)
                };
                DataTable dt = acceso.Leer("TraerUsuario", parametros);
                if (dt == null || dt.Rows.Count == 0)
                    return null;
                var result = dt.Rows[0];
                Usuario u = new Usuario();
                u.User = user;
                u.Nombre = result["Nombre"].ToString();
                u.Id = Convert.ToInt32(result["Usuario_Id"]);
                u.Apellido = result["Apellido"].ToString();
                return u;
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        
    }
}
