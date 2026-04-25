using System;

namespace BLL
{
    public class ErrorManagerService
    {
        private static ErrorManagerService _instance;
        private static readonly object _padlock = new object();

        public event EventHandler<BE.Error> OnOcurrioError;

        private ErrorManagerService() { }

        public static ErrorManagerService GetInstance()
        {
            lock (_padlock)
            {
                if (_instance == null)
                    _instance = new ErrorManagerService();

                return _instance;
            }
        }

        public void ManejarError(string msj, BE.EnumError tipo = BE.EnumError.Advertencia)
        {
            if (string.IsNullOrWhiteSpace(msj))
                throw new ArgumentNullException(nameof(msj));

            OnOcurrioError?.Invoke(this, new BE.Error { Mensaje = msj, Tipo = tipo });
        }

        public void ManejarError(Exception ex, BE.EnumError tipo = BE.EnumError.Error)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));

            OnOcurrioError?.Invoke(this, new BE.Error
            { 
                Mensaje = ex.Message, 
                Tipo = tipo,
                Exepcion = ex 
            });
        }
    }

}