using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MP_Bitacora : MAPPER<BE.Bitacora>
    {
        public override int Agregar(Bitacora obj)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@usuario", obj.Usuario.Id));
            parametros.Add(acceso.CrearParameter("@fecha", obj.Fecha));
            parametros.Add(acceso.CrearParameter("@actividad", obj.Actividad));
            parametros.Add(acceso.CrearParameter("@criticidad", (int)obj.Criticidad));
            int res = acceso.Escribir("InsertarBitacora", parametros);
            acceso.Cerrar();
            return res;
        }

        public override int Eliminar(Bitacora obj)
        {
            throw new NotImplementedException();
        }

        public override List<Bitacora> Listar()
        {
            return Listar(null, DateTime.MinValue, DateTime.MaxValue, null, null);
        }

        public List<Bitacora> Listar(List<int> usuarioIds, DateTime fechaInicio, DateTime fechaFin, List<string> actividades, List<EnumCriticidad> criticidades)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();

            if (usuarioIds != null && usuarioIds.Count > 0)
                parametros.Add(acceso.CrearParameter("@usuarioIds", string.Join(",", usuarioIds)));

            if (fechaInicio != DateTime.MinValue)
                parametros.Add(acceso.CrearParameter("@fechaInicio", fechaInicio));

            if (fechaFin != DateTime.MaxValue)
                parametros.Add(acceso.CrearParameter("@fechaFin", fechaFin));

            if (actividades != null && actividades.Count > 0)
                parametros.Add(acceso.CrearParameter("@actividades", string.Join(",", actividades)));

            if (criticidades != null && criticidades.Count > 0)
                parametros.Add(acceso.CrearParameter("@criticidades", string.Join(",", criticidades.Select(c => (int)c))));

            DataTable dt = acceso.Leer("ListarBitacoraFiltrado", parametros);
            List<Bitacora> registros = new List<Bitacora>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Bitacora b = new Bitacora();
                    b.Id = Convert.ToInt32(dr["Id"]);
                    b.Usuario = new Usuario { Id = Convert.ToInt32(dr["UsuarioId"]) };
                    b.Fecha = Convert.ToDateTime(dr["Fecha"]);
                    b.Actividad = Convert.ToString(dr["Actividad"]);
                    b.Criticidad = (EnumCriticidad)Convert.ToInt32(dr["Criticidad"]);

                    registros.Add(b);
                }
            }

            acceso.Cerrar();
            return registros;
        }

        public override int Modificar(Bitacora obj)
        {
            throw new NotImplementedException();
        }
    }
}
