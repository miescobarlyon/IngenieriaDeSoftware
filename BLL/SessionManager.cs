using BE;
using System;

namespace BLL
{
    public sealed class SessionManager
    {
        private static SessionManager _instance;
        private static readonly object _padlock = new object();

        private BE.Usuario _usuario;
        private ComponentePermiso _permisos;

        private SessionManager() { }

        public static SessionManager GetInstance()
        {
            lock (_padlock)
            {
                if (_instance == null)
                    _instance = new SessionManager();
                return _instance;
            }
        }

        public BE.Usuario GetUsuario() => _usuario;

        public void Login(BE.Usuario u)
        {
            if (u == null) throw new ArgumentNullException(nameof(u));
            _usuario = u;
            _permisos = new GestorPermisos().ObtenerPermisosDeUsuario(u.Id);
        }

        public void Logout()
        {
            _usuario = null;
            _permisos = null;
        }

        public bool TienePermiso(string codigo)
        {
            return _permisos != null && _permisos.Tiene(codigo);
        }
        private BE.ResultadoVerificacion _verificacionDV;

        public void SetVerificacionDV(BE.ResultadoVerificacion r)
        {
            _verificacionDV = r;
        }

        public BE.ResultadoVerificacion GetVerificacionDV()
        {
            return _verificacionDV;
        }

        public bool DbCorrupta => _verificacionDV != null && !_verificacionDV.EsValido;
    }
}