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
    public class MP_HistorialUsuario : MAPPER<HistorialUsuario>
    {
        public override int Agregar(HistorialUsuario obj)
            => throw new NotImplementedException(
                "El historial es gestionado automáticamente por los SPs de USUARIO.");

        public override int Modificar(HistorialUsuario obj)
            => throw new NotImplementedException("El historial es inmutable.");

        public override int Eliminar(HistorialUsuario obj)
            => throw new NotImplementedException(
                "Los registros de auditoría no pueden eliminarse.");

        public override List<HistorialUsuario> Listar()
            => throw new InvalidOperationException(
                "Use Listar(int usuarioId) para obtener el historial de un usuario específico.");

        public List<HistorialUsuario> Listar(int usuarioId)
        {
            acceso = new Acceso();
            try
            {
                acceso.Abrir();
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acceso.CrearParameter("@usuario_id", usuarioId));
                DataTable dt = acceso.Leer("ListarHistorialUsuario", parametros);

                List<HistorialUsuario> lista = new List<HistorialUsuario>();
                if (dt == null) return lista;

                foreach (DataRow row in dt.Rows)
                {
                    HistorialUsuario h = new HistorialUsuario();
                    h.Id = Convert.ToInt32(row["HISTORIAL_USUARIO_ID"]);
                    h.UsuarioId = Convert.ToInt32(row["USUARIO_ID"]);
                    h.Nombre = row["NOMBRE"].ToString();
                    h.Apellido = row["APELLIDO"].ToString();
                    h.User = row["USUARIO"].ToString();
                    h.Borrado = Convert.ToInt32(row["BORRADO"]);
                    h.IdiomaId = row.IsNull("IDIOMA_ID")
                                            ? (int?)null
                                            : Convert.ToInt32(row["IDIOMA_ID"]);
                    h.FechaModificacion = Convert.ToDateTime(row["FECHA_MODIFICACION"]);
                    h.ModificadorId = Convert.ToInt32(row["MODIFICADOR_ID"]);
                    h.Accion = row["ACCION"].ToString();
                    h.ModificadorNombre = row["MODIFICADOR_NOMBRE"].ToString();
                    lista.Add(h);
                }
                return lista;
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        public int Revertir(int historialId, int modificadorId)
        {
            acceso = new Acceso();
            try
            {
                acceso.Abrir();
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acceso.CrearParameter("@historial_id", historialId));
                parametros.Add(acceso.CrearParameter("@modificador_id", modificadorId));
                return acceso.Escribir("RevertirUsuario", parametros);
            }
            finally
            {
                acceso.Cerrar();
            }
        }
    }
}
