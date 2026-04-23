using System;

namespace BLL
{
    public sealed class SessionManager
    {
        private static SessionManager _instance;
        private static readonly object _padlock = new object();

        private BE.Usuario _usuario;

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

        public BE.Usuario GetUsuario()
        {
            return _usuario;
        }

        public void Login(BE.Usuario u)
        {
            if (u == null) throw new ArgumentNullException(nameof(u));
            _usuario = u;
        }

        public void Logout()
        {
            _usuario = null;
        }

      
    }
}
