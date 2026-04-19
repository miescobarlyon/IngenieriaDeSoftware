using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BitacoraService
    {
        public static void Guardar(BE.Bitacora bitacora) 
        {
            DAL.MP_Bitacora mapper = new DAL.MP_Bitacora();
            mapper.Agregar(bitacora);

        }

        public static List<BE.Bitacora> Listar(List<BE.Usuario> usuarios, DateTime inicio, DateTime fin, List<string> actividad, List<BE.EnumCriticidad> criticidad)
        {
            DAL.MP_Bitacora mapper = new DAL.MP_Bitacora();

            List<int> usuarioIds = usuarios?.Select(u => u.Id).ToList();

            return mapper.Listar(usuarioIds, inicio, fin, actividad, criticidad);
        }

    }
}
