using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MP_Traduccion : MAPPER<Traduccion>
    {
        public override int Agregar(Traduccion obj)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@clave",    obj.Clave));
            parametros.Add(acceso.CrearParameter("@idiomaId", obj.IdiomaId));
            parametros.Add(acceso.CrearParameter("@texto",    obj.Texto));
            int res = acceso.Escribir("InsertarTraduccion", parametros);
            acceso.Cerrar();
            return res;
        }

        public override int Modificar(Traduccion obj)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@idTraduccion", obj.Id));
            parametros.Add(acceso.CrearParameter("@texto", obj.Texto));
            int res = acceso.Escribir("EditarTraduccion", parametros);
            acceso.Cerrar();
            return res;
        }

        public override int Eliminar(Traduccion obj)
        {
            throw new NotImplementedException();
        }

        public override List<Traduccion> Listar()
        {
            throw new InvalidOperationException(
                "Usar Listar(int idiomaId) para listar las traducciones de un idioma en especifico.");
        }

        public List<Traduccion> Listar(int idiomaId)
        {
            acceso = new Acceso();
            try
            {
                acceso.Abrir();
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acceso.CrearParameter("@idiomaId", idiomaId));
                DataTable dt = acceso.Leer("ListarTraduccionesPorIdioma", parametros);

                List<Traduccion> lista = new List<Traduccion>();
                if (dt == null) return lista;

                foreach (DataRow row in dt.Rows)
                {
                    Traduccion t = new Traduccion();
                    t.Id       = Convert.ToInt32(row["TRADUCCION_ID"]);
                    t.Clave    = row["CLAVE"].ToString();
                    t.IdiomaId = Convert.ToInt32(row["IDIOMA_ID"]);
                    t.Texto    = row["TEXTO"].ToString();
                    lista.Add(t);
                }
                return lista;
            }
            finally
            {
                acceso.Cerrar();
            }
        }
    }
}
