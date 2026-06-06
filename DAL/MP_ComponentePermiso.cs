using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MP_ComponentePermiso : MAPPER<ComponentePermiso>
    {

        public override int Agregar(ComponentePermiso obj)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@codigo", obj.Codigo));
            parametros.Add(acceso.CrearParameter("@nombre", obj.Nombre));
            parametros.Add(acceso.CrearParameter("@tipo", obj is GrupoPermiso ? "g" : "p"));
            int res = acceso.Escribir("InsertarComponentePermiso", parametros);
            acceso.Cerrar();
            return res;
        }

        public override int Modificar(ComponentePermiso obj)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@id", obj.Id));
            parametros.Add(acceso.CrearParameter("@codigo", obj.Codigo));
            parametros.Add(acceso.CrearParameter("@nombre", obj.Nombre));
            int res = acceso.Escribir("ModificarComponentePermiso", parametros);
            acceso.Cerrar();
            return res;
        }

        public override int Eliminar(ComponentePermiso obj)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@id", obj.Id));
            int res = acceso.Escribir("EliminarComponentePermiso", parametros);
            acceso.Cerrar();
            return res;
        }

        public override List<ComponentePermiso> Listar()
        {
            List<ComponentePermiso> componentes = new List<ComponentePermiso>();
            acceso = new Acceso();
            acceso.Abrir();
            DataTable tabla = acceso.Leer("ListarComponentePermiso");
            acceso.Cerrar();
            foreach (DataRow row in tabla.Rows)
            {
                componentes.Add(MapearFila(row));
            }
            return componentes;
        }

        public ComponentePermiso TraerPorCodigo(string codigo)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@codigo", codigo));
            DataTable tabla = acceso.Leer("TraerComponentePorCodigo", parametros);
            acceso.Cerrar();
            if (tabla == null || tabla.Rows.Count == 0) return null;
            return MapearFila(tabla.Rows[0]);
        }

        public int AgregarHijo(int idPadre, int idHijo)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@padre", idPadre));
            parametros.Add(acceso.CrearParameter("@hijo", idHijo));
            int res = acceso.Escribir("AgregarHijoAComponente", parametros);
            acceso.Cerrar();
            return res;
        }

        public int QuitarHijo(int idPadre, int idHijo)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@padre", idPadre));
            parametros.Add(acceso.CrearParameter("@hijo", idHijo));
            int res = acceso.Escribir("QuitarHijoDeComponente", parametros);
            acceso.Cerrar();
            return res;
        }

        public List<ComponentePermiso> TraerHijos(int idPadre)
        {
            List<ComponentePermiso> hijos = new List<ComponentePermiso>();
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@padre", idPadre));
            DataTable tabla = acceso.Leer("TraerHijosDeComponente", parametros);
            acceso.Cerrar();
            foreach (DataRow row in tabla.Rows)
            {
                hijos.Add(MapearFila(row));
            }
            return hijos;
        }


        public int AsignarAUsuario(int idUsuario, int idComponente)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@usuario", idUsuario));
            parametros.Add(acceso.CrearParameter("@componente", idComponente));
            int res = acceso.Escribir("AsignarComponenteAUsuario", parametros);
            acceso.Cerrar();
            return res;
        }

        public int QuitarDeUsuario(int idUsuario, int idComponente)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@usuario", idUsuario));
            parametros.Add(acceso.CrearParameter("@componente", idComponente));
            int res = acceso.Escribir("QuitarComponenteDeUsuario", parametros);
            acceso.Cerrar();
            return res;
        }

        public List<ComponentePermiso> TraerComponentesDeUsuario(int idUsuario)
        {
            List<ComponentePermiso> componentes = new List<ComponentePermiso>();
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@usuario", idUsuario));
            DataTable tabla = acceso.Leer("TraerComponentesDeUsuario", parametros);
            acceso.Cerrar();
            foreach (DataRow row in tabla.Rows)
            {
                componentes.Add(MapearFila(row));
            }
            return componentes;
        }

        private ComponentePermiso MapearFila(DataRow row)
        {
            string tipo = row["tipo"].ToString();
            ComponentePermiso comp;
            if (tipo == "g")
                comp = new GrupoPermiso();
            else
                comp = new PermisoSimple();

            comp.Id = Convert.ToInt32(row["componente_permiso_id"]);
            comp.Codigo = row["codigo"].ToString();
            comp.Nombre = row["nombre"].ToString();
            return comp;
        }
    }
}