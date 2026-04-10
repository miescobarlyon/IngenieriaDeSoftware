using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Acceso
    {
        private SqlConnection conexion;
        public void Abrir()
        {
            conexion = new SqlConnection("Initial Catalog=CAMPO; Data source=.;Integrated Security = SSPI");
            conexion.Open();
        }
        public void Cerrar()
        {
            conexion.Close();
            conexion = null;
            GC.Collect();
        }

        private SqlCommand CrearCommand(string sql, List<SqlParameter> parameters = null)
        {
            SqlCommand cmd = new SqlCommand(sql, conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }
            return cmd;
        }

        public DataTable Leer(string sql, List<SqlParameter> parameters = null)
        {

            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = CrearCommand(sql, parameters);
            adapter.Fill(dt);
            adapter.Dispose();
            adapter = null;
            return dt;
        }

        public int Escribir(string sql, List<SqlParameter> parameters = null)
        {
            using (SqlCommand cmd = CrearCommand(sql, parameters))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public SqlParameter CrearParameter(string nombre, string valor)
        {
            SqlParameter p = new SqlParameter(nombre, valor);
            p.DbType = DbType.String;
            return p;
        }
        public SqlParameter CrearParameter(string nombre, int valor)
        {
            SqlParameter p = new SqlParameter(nombre, valor);
            p.DbType = DbType.Int32;
            return p;
        }
        public SqlParameter CrearParameter(string nombre, float valor)
        {
            SqlParameter p = new SqlParameter(nombre, valor);
            p.DbType = DbType.Single;
            return p;
        }
    }
}
