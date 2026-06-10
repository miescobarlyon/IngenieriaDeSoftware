using BLL;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class RecuperacionDB : TranslatableForm
    {
        private readonly Main principal;
        private readonly ErrorManagerService errorManager =
            ErrorManagerService.GetInstance();

        public RecuperacionDB(Main main)
        {
            InitializeComponent();
            principal = main;
            errorManager.OnOcurrioError += ErrorManager_OnOcurrioError;
        }

        private void RecuperacionDB_Load(object sender, EventArgs e)
        {
            MostrarEstado();
        }

        public override void CambiarIdioma(BE.Idioma idioma)
        {
            base.CambiarIdioma(idioma);
            MostrarEstado();
        }

        private void MostrarEstado()
        {
            var svc = IdiomaService.GetInstance();
            var resultado = SessionManager.GetInstance().GetVerificacionDV();

            if (resultado == null || resultado.EsValido)
            {
                labelEstado.Text = svc.Traducir("lbl.dbSana");
                textBoxDetalle.Text = "";
                buttonRestaurar.Enabled = false;
                return;
            }

            labelEstado.Text = svc.Traducir("lbl.dbCorrupta");
            buttonRestaurar.Enabled = true;

            var sb = new StringBuilder();
            if (resultado.IdsCorruptos.Count > 0)
            {
                sb.AppendLine(svc.Traducir("lbl.idsCorruptos") + ":");
                sb.AppendLine(string.Join(", ", resultado.IdsCorruptos));
                sb.AppendLine();
            }
            if (resultado.DvvCorrupto)
            {
                sb.AppendLine(svc.Traducir("lbl.dvvCorrupto"));
            }
            textBoxDetalle.Text = sb.ToString();
        }

        private void ErrorManager_OnOcurrioError(object sender, BE.Error e)
        {
            MessageBoxIcon icon;
            switch (e.Tipo)
            {
                case BE.EnumError.Info: icon = MessageBoxIcon.Information; break;
                case BE.EnumError.Advertencia: icon = MessageBoxIcon.Warning; break;
                case BE.EnumError.Error: icon = MessageBoxIcon.Error; break;
                case BE.EnumError.Critico: icon = MessageBoxIcon.Stop; break;
                default: icon = MessageBoxIcon.None; break;
            }
            MessageBox.Show(e.Mensaje, "Notificación", MessageBoxButtons.OK, icon);
        }

        private void buttonRestaurar_Click_1(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
        "Esto va a restaurar todos los usuarios al último backup. ¿Continuar?",
        "Notificación",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                var dv = new DigitoVerificadorService();
                dv.Restaurar();

                var nuevoResultado = dv.Verificar();
                SessionManager.GetInstance().SetVerificacionDV(nuevoResultado);

                BitacoraService.Guardar(new BE.Bitacora
                {
                    Usuario = SessionManager.GetInstance().GetUsuario(),
                    Fecha = DateTime.Now,
                    Actividad = "Restauración de la base de datos desde backup.",
                    Criticidad = BE.EnumCriticidad.ALTA
                });

                MessageBox.Show(
                    "Base de datos restaurada correctamente.",
                    "Notificación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                principal.LoadForm(new Inicio(principal));
            }
            catch (Exception ex)
            {
                errorManager.ManejarError(ex, BE.EnumError.Error);
            }
        }
    }
}