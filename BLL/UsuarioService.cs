using DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class UsuarioService
    {
        BLL.SessionManager SessionManager = BLL.SessionManager.GetInstance();
        private readonly DAL.MP_Usuario mapper = new DAL.MP_Usuario();
        private readonly SecurityService hashService = new SecurityService();

        public bool Login(string user, string passwordPlano)
        {
            Tuple<string, string> credenciales = mapper.TraerPass(user);
            if (credenciales == null) return false;
            string hash = credenciales.Item1;
            string salt = credenciales.Item2;
            if (string.IsNullOrWhiteSpace(hash) || string.IsNullOrWhiteSpace(salt))
                return false;

            bool ok = hashService.Verify(passwordPlano, salt, hash);
            BE.Usuario usuario = mapper.TraerUsuario(user);
            if (usuario == null) return false;

            BE.Bitacora registro;

            if (ok)
            {
                registro = new BE.Bitacora
                {
                    Usuario = usuario,
                    Fecha = DateTime.Now,
                    Actividad = $"{usuario.Id} inició sesión.",
                    Criticidad = BE.EnumCriticidad.BAJA
                };
                mapper.ReestablecerIntentos(usuario);
                SessionManager.Login(usuario);
            }
            else
            {
                registro = new BE.Bitacora
                {
                    Usuario = usuario,
                    Fecha = DateTime.Now,
                    Actividad = $"{usuario.Id} intentó iniciar sesión con contraseña incorrecta.",
                    Criticidad = BE.EnumCriticidad.MEDIA
                };
                mapper.AgregarIntento(usuario);
            }

            BLL.BitacoraService.Guardar(registro);

            return ok;
        }

        public static List<BE.Usuario> Listar()
        {
            DAL.MP_Usuario mapper = new DAL.MP_Usuario();
            return mapper.Listar();
        }
    }
}
