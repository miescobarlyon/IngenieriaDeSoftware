using System;

namespace BLL
{
    public class UsuarioService
    {
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

            return hashService.Verify(passwordPlano, salt, hash);
        }
    }
}
