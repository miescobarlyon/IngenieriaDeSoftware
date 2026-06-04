using System;
using System.Windows.Forms;

namespace UI
{
    public partial class Inicio : TranslatableForm
    {
        private Main principal;

        public Inicio(Main main)
        {
            InitializeComponent();
            principal = main;
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            ActualizarSaludo();
        }

        public override void CambiarIdioma(BE.Idioma idioma)
        {
            base.CambiarIdioma(idioma);
            ActualizarSaludo();
        }

        private void ActualizarSaludo()
        {
            BE.Usuario usuario = BLL.SessionManager.GetInstance().GetUsuario();
            string base_text = BLL.IdiomaService.GetInstance().Traducir("lbl.bienvenido");

            labelSaludo.Text = usuario != null
                ? $"{base_text}, {usuario.Nombre}!"
                : base_text;
        }
    }
}
