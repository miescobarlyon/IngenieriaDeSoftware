using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UsuarioService
    {
        DAL.MP_Usuario mapper = new DAL.MP_Usuario();
        public string TraerPass(string user)
        {
            return mapper.TraerPass(user);
        }
    }
}
