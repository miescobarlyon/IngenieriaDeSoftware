using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class HistorialUsuarioService
    {
        public static List<BE.HistorialUsuario> Listar(int usuarioId)
        {
            return new DAL.MP_HistorialUsuario().Listar(usuarioId);
        }

        public static void Revertir(int historialId)
        {
            BE.Usuario modificador = SessionManager.GetInstance().GetUsuario();
            if (modificador == null)
                throw new InvalidOperationException(
                    "No hay sesión activa. No se puede registrar el modificador.");

            new DAL.MP_HistorialUsuario().Revertir(historialId, modificador.Id);

            BitacoraService.Guardar(new BE.Bitacora
            {
                Usuario = modificador,
                Fecha = DateTime.Now,
                Actividad = $"{modificador.Id} revirtió historial #{historialId}.",
                Criticidad = BE.EnumCriticidad.ALTA
            });
        }
    }
}
