using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MP_Idioma : MAPPER<Idioma>
    {
        public override int Agregar(Idioma obj)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@nombre", obj.Nombre));
            parametros.Add(acceso.CrearParameter("@codigo", obj.Codigo));
            int res = acceso.Escribir("InsertarIdioma", parametros);
            acceso.Cerrar();
            return res;
        }

        public override int Modificar(Idioma obj)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@idIdioma", obj.Id));
            parametros.Add(acceso.CrearParameter("@nombre", obj.Nombre));
            parametros.Add(acceso.CrearParameter("@codigo", obj.Codigo));
            int res = acceso.Escribir("EditarIdioma", parametros);
            acceso.Cerrar();
            return res;
        }

        public override int Eliminar(Idioma obj)
        {
            throw new NotImplementedException();
        }

        public override List<Idioma> Listar()
        {
            acceso = new Acceso();
            acceso.Abrir();
            DataTable dt = acceso.Leer("ListarIdiomas");
            acceso.Cerrar();

            List<Idioma> lista = new List<Idioma>();
            if (dt == null) return lista;

            foreach (DataRow row in dt.Rows)
            {
                Idioma i = new Idioma();
                i.Id     = Convert.ToInt32(row["IDIOMA_ID"]);
                i.Nombre = row["NOMBRE"].ToString();
                i.Codigo = row["CODIGO"].ToString();
                lista.Add(i);
            }
            return lista;
        }

        public Idioma TraerPorId(int id)
        {
            acceso = new Acceso();
            try
            {
                acceso.Abrir();
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acceso.CrearParameter("@id", id));
                DataTable dt = acceso.Leer("TraerIdiomaPorId", parametros);

                if (dt == null || dt.Rows.Count == 0) return null;

                DataRow row = dt.Rows[0];
                Idioma i = new Idioma();
                i.Id     = Convert.ToInt32(row["IDIOMA_ID"]);
                i.Nombre = row["NOMBRE"].ToString();
                i.Codigo = row["CODIGO"].ToString();
                return i;
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        public BE.Idioma AgregarYObtener(BE.Idioma obj)
        {
            acceso = new Acceso();
            try
            {
                acceso.Abrir();
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acceso.CrearParameter("@nombre", obj.Nombre));
                parametros.Add(acceso.CrearParameter("@codigo", obj.Codigo));

                DataTable dt = acceso.Leer("InsertarIdioma", parametros);

                if (dt != null && dt.Rows.Count > 0)
                    obj.Id = Convert.ToInt32(dt.Rows[0]["IDIOMA_ID"]);

                return obj;
            }
            finally
            {
                acceso.Cerrar();
            }
        }

    }
}
