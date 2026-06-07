using BE;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class GestorPermisos
    {
        private readonly DAL.MP_ComponentePermiso mapper = new DAL.MP_ComponentePermiso();

        public void CrearGrupo(string codigo, string nombre)
        {
            if (mapper.TraerPorCodigo(codigo) != null)
                throw new InvalidOperationException($"Ya existe un componente con código '{codigo}'.");

            mapper.Agregar(new GrupoPermiso { Codigo = codigo, Nombre = nombre });
        }

        public void Eliminar(string codigo)
        {
            var componente = mapper.TraerPorCodigo(codigo)
                ?? throw new KeyNotFoundException($"No existe el componente '{codigo}'.");

            if (!(componente is GrupoPermiso))
                throw new InvalidOperationException(
                    "No se pueden eliminar permisos simples, solo grupos.");

            mapper.Eliminar(componente);
        }

        public void AgregarHijo(string codigoPadre, string codigoHijo)
        {
            var padre = mapper.TraerPorCodigo(codigoPadre)
                ?? throw new KeyNotFoundException($"No existe el padre '{codigoPadre}'.");

            if (!(padre is GrupoPermiso))
                throw new InvalidOperationException($"'{codigoPadre}' no es un grupo, no puede contener hijos.");

            var hijo = mapper.TraerPorCodigo(codigoHijo)
                ?? throw new KeyNotFoundException($"No existe el hijo '{codigoHijo}'.");

            if (padre.Id == hijo.Id)
                throw new InvalidOperationException("Un componente no puede contenerse a sí mismo.");

            mapper.AgregarHijo(padre.Id, hijo.Id);
        }

        public void QuitarHijo(string codigoPadre, string codigoHijo)
        {
            var padre = mapper.TraerPorCodigo(codigoPadre)
                ?? throw new KeyNotFoundException($"No existe el padre '{codigoPadre}'.");

            var hijo = mapper.TraerPorCodigo(codigoHijo)
                ?? throw new KeyNotFoundException($"No existe el hijo '{codigoHijo}'.");

            mapper.QuitarHijo(padre.Id, hijo.Id);
        }
        public List<ComponentePermiso> ObtenerHijosDirectos(string codigoGrupo)
        {
            var grupo = mapper.TraerPorCodigo(codigoGrupo)
                ?? throw new KeyNotFoundException($"No existe el componente '{codigoGrupo}'.");
            return mapper.TraerHijos(grupo.Id);
        }

        public void AsignarAUsuario(int idUsuario, string codigoComponente)
        {
            var componente = mapper.TraerPorCodigo(codigoComponente)
                ?? throw new KeyNotFoundException($"No existe el componente '{codigoComponente}'.");

            var actuales = mapper.TraerComponentesDeUsuario(idUsuario);
            if (actuales.Any(c => c.Codigo == codigoComponente))
                throw new InvalidOperationException($"El usuario ya tiene asignado '{codigoComponente}'.");

            mapper.AsignarAUsuario(idUsuario, componente.Id);
        }

        public void QuitarDeUsuario(int idUsuario, string codigoComponente)
        {
            var componente = mapper.TraerPorCodigo(codigoComponente)
                ?? throw new KeyNotFoundException($"No existe el componente '{codigoComponente}'.");

            mapper.QuitarDeUsuario(idUsuario, componente.Id);
        }

        public List<ComponentePermiso> ObtenerTodos()
        {
            return mapper.Listar();
        }

        public List<GrupoPermiso> ObtenerGrupos()
        {
            return mapper.Listar().OfType<GrupoPermiso>().ToList();
        }

        public List<PermisoSimple> ObtenerPermisosSimples()
        {
            return mapper.Listar().OfType<PermisoSimple>().ToList();
        }

        public ComponentePermiso ObtenerArbolDe(string codigoGrupo)
        {
            var componente = mapper.TraerPorCodigo(codigoGrupo)
                ?? throw new KeyNotFoundException($"No existe el componente '{codigoGrupo}'.");

            LlenarHijos(componente, new HashSet<int>());
            return componente;
        }

        public ComponentePermiso ObtenerPermisosDeUsuario(int idUsuario)
        {
            List<ComponentePermiso> directos = mapper.TraerComponentesDeUsuario(idUsuario);

            if (directos.Count == 0)
                return new GrupoPermiso { Codigo = "EMPTY", Nombre = "Sin permisos" };

            if (directos.Count == 1)
            {
                LlenarHijos(directos[0], new HashSet<int>());
                return directos[0];
            }

            var raiz = new GrupoPermiso { Codigo = "ROOT", Nombre = "Permisos del usuario" };
            foreach (var c in directos)
            {
                LlenarHijos(c, new HashSet<int>());
                raiz.Agregar(c);
            }
            return raiz;
        }

        public bool TienePermiso(int idUsuario, string codigoPermiso)
        {
            return ObtenerPermisosDeUsuario(idUsuario).Tiene(codigoPermiso);
        }

        private void LlenarHijos(ComponentePermiso componente, HashSet<int> visitados)
        {
            if (!(componente is GrupoPermiso)) return;
            if (!visitados.Add(componente.Id)) return;

            GrupoPermiso grupo = (GrupoPermiso)componente;
            foreach (var hijo in mapper.TraerHijos(componente.Id))
            {
                LlenarHijos(hijo, visitados);
                grupo.Agregar(hijo);
            }
        }
    }
}