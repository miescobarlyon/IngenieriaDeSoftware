using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MP_DigitoVerificador
    {
        internal Acceso acceso;

        public int ActualizarDVHUsuario(int id, string dvh)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@id", id));
            parametros.Add(acceso.CrearParameter("@dvh", dvh));
            int res = acceso.Escribir("ActualizarDVHUsuario", parametros);
            acceso.Cerrar();
            return res;
        }

        public string TraerDVV(string tabla)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@tabla", tabla));
            DataTable dt = acceso.Leer("TraerDVV", parametros);
            acceso.Cerrar();
            if (dt == null || dt.Rows.Count == 0) return "";
            return dt.Rows[0]["DVV"].ToString();
        }

        public int ActualizarDVV(string tabla, string dvv)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@tabla", tabla));
            parametros.Add(acceso.CrearParameter("@dvv", dvv));
            int res = acceso.Escribir("ActualizarDVV", parametros);
            acceso.Cerrar();
            return res;
        }
        public int ActualizarBackup()
        {
            acceso = new Acceso();
            acceso.Abrir();
            int res = acceso.Escribir("ActualizarBackupUsuario", new List<SqlParameter>());
            acceso.Cerrar();
            return res;
        }

        public int RestaurarDesdeBackup()
        {
            acceso = new Acceso();
            acceso.Abrir();
            int res = acceso.Escribir("RestaurarUsuarioDesdeBackup", new List<SqlParameter>());
            acceso.Cerrar();
            return res;
        }
    }
}   