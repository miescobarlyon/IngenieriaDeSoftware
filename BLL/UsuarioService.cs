using DAL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class UsuarioService
    {
        BLL.SessionManager SessionManager = BLL.SessionManager.GetInstance();
        BLL.ErrorManagerService ErrorManager = BLL.ErrorManagerService.GetInstance();
        private readonly DAL.MP_Usuario mapper = new DAL.MP_Usuario();
        private readonly SecurityService hashService = new SecurityService();
        private readonly DigitoVerificadorService dvService = new DigitoVerificadorService();
        public bool Login(string user, string passwordPlano)
        {
            bool resultado = false;
            BE.Bitacora registro = null;

            Tuple<string, string> credenciales = mapper.TraerPass(user);
            if (credenciales != null)
            {
                string hash = credenciales.Item1;
                string salt = credenciales.Item2;
                if (!string.IsNullOrWhiteSpace(hash) && !string.IsNullOrWhiteSpace(salt))
                {
                    bool ok = hashService.Verify(passwordPlano, salt, hash);
                    BE.Usuario usuario = mapper.TraerUsuario(user);
                    if (usuario != null)
                    {
                        if (usuario.Bloqueado == 1)
                        {
                            registro = new BE.Bitacora
                            {
                                Usuario = usuario,
                                Fecha = DateTime.Now,
                                Actividad = $"{usuario.Id} intentó iniciar sesión pero su usuario está bloqueado.",
                                Criticidad = BE.EnumCriticidad.MEDIA
                            };
                            ErrorManager.ManejarError("Tu usuario está bloqueado. Contacta con el administrador.", BE.EnumError.Advertencia);
                        }
                        else if (ok)
                        {
                            registro = new BE.Bitacora
                            {
                                Usuario = usuario,
                                Fecha = DateTime.Now,
                                Actividad = $"{usuario.Id} inició sesión.",
                                Criticidad = BE.EnumCriticidad.BAJA
                            };
                            mapper.ReestablecerIntentos(usuario);
                            dvService.Recalcular();
                            SessionManager.Login(usuario);
                            resultado = true;
                        }
                        else
                        {
                            int intentos = mapper.TraerIntentos(usuario);
                            int intentosRestantes = 3 - (intentos + 1);

                            registro = new BE.Bitacora
                            {
                                Usuario = usuario,
                                Fecha = DateTime.Now,
                                Actividad = $"{usuario.Id} intentó iniciar sesión con contraseña incorrecta.",
                                Criticidad = BE.EnumCriticidad.MEDIA
                            };
                            mapper.AgregarIntento(usuario);
                            dvService.Recalcular();
                            if (intentosRestantes > 0)
                            {
                                ErrorManager.ManejarError(
                                    $"Contraseña incorrecta. Te quedan {intentosRestantes} intentos.",
                                    BE.EnumError.Advertencia);
                            }

                            Bloquear(usuario);
                        }

                        if (registro != null)
                        {
                            BLL.BitacoraService.Guardar(registro);
                        }
                    }
                }
                else
                {
                    ErrorManager.ManejarError("Usuario no encontrado o datos incompletos.", BE.EnumError.Advertencia);
                }
            }
            else
            {
                ErrorManager.ManejarError("Usuario no encontrado.", BE.EnumError.Advertencia);
            }

            return resultado;
        }

        public static List<BE.Usuario> Listar()
        {
            DAL.MP_Usuario mapper = new DAL.MP_Usuario();
            return mapper.Listar();
        }

        public static void Bloquear(BE.Usuario usuario)
        {
            DAL.MP_Usuario mapper = new DAL.MP_Usuario();
            int intentos = mapper.TraerIntentos(usuario);

            if (intentos >= 3 && usuario.Bloqueado == 0)
            {
                BE.Bitacora registro = new BE.Bitacora
                {
                    Usuario = usuario,
                    Fecha = DateTime.Now,
                    Actividad = $"{usuario.Id} fue bloqueado.",
                    Criticidad = BE.EnumCriticidad.ALTA
                };
                BLL.BitacoraService.Guardar(registro);
                mapper.Bloquear(usuario);
                new DigitoVerificadorService().Recalcular();
                ErrorManagerService.GetInstance().ManejarError(
                    "Tu usuario ha sido bloqueado por exceso de intentos fallidos.",
                    BE.EnumError.Critico);
            }
        }

        public void Guardar(BE.Usuario usuario)
        {
            DAL.MP_Usuario mapper = new DAL.MP_Usuario();


            if (usuario.Id == 0)
            {
                mapper.Agregar(usuario, SessionManager.GetUsuario());
                BitacoraService.Guardar(new BE.Bitacora
                {
                    Usuario = usuario,
                    Fecha = DateTime.Now,
                    Actividad = $"{usuario.Id} fue creado.",
                    Criticidad = BE.EnumCriticidad.BAJA
                });
            }
            else
            {
                mapper.Modificar(usuario, SessionManager.GetUsuario());
                BitacoraService.Guardar(new BE.Bitacora
                {
                    Usuario = usuario,
                    Fecha = DateTime.Now,
                    Actividad = $"{usuario.Id} fue editado.",
                    Criticidad = BE.EnumCriticidad.MEDIA
                });
            }
        }

        public BE.Usuario HashearPassword(BE.Usuario usuario, string passwordPlano)
        {
            var sec = new SecurityService();
            usuario.Salt = sec.GenerarSalt();
            usuario.PasswordHash = sec.HashPassword(passwordPlano, usuario.Salt);
            return usuario;
        }

        public static void Desbloquear(BE.Usuario usuario)
        {
            new DAL.MP_Usuario().Desbloquear(usuario);
            BitacoraService.Guardar(new BE.Bitacora
            {
                Usuario = usuario,
                Fecha = DateTime.Now,
                Actividad = $"{usuario.Id} fue desbloqueado por el administrador.",
                Criticidad = BE.EnumCriticidad.MEDIA
            });
        }

        public static List<BE.Usuario> ListarActivos()
        {
            return Listar()
                .Where(u => u.Borrado == 0)
                .OrderBy(u => u.Apellido)
                .ThenBy(u => u.Nombre)
                .ToList();
        }
    }
}
