using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var dv = new DigitoVerificadorService();
            var resultado = dv.Verificar();
            BLL.SessionManager.GetInstance().SetVerificacionDV(resultado);

            if (!resultado.EsValido)
            {
                string detalle = "";
                if (resultado.IdsCorruptos.Count > 0)
                    detalle += $"Filas con datos modificados (IDs): {string.Join(", ", resultado.IdsCorruptos)}\n";
                if (resultado.DvvCorrupto)
                    detalle += "Posible inserción o eliminación de filas no autorizadas.\n";

                MessageBox.Show(
                    "Se detectó una alteración en la base de datos:\n\n" + detalle +
                    "\nSolo administradores podrán iniciar sesión para recuperarla.",
                    "Atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            Application.Run(new Form1());
            
        }
    }
}
