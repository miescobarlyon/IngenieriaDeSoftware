using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class Inicio : Form
    {
        private Main principal;

        public Inicio(Main main)
        {
            InitializeComponent();
            principal = main;
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            string saludo = $"Buenas, {BLL.SessionManager.GetInstance().GetUsuario().Nombre}";
            if (0 <= DateTime.Now.Hour && DateTime.Now.Hour <= 4)
            {
                saludo = $"Andá a dormir, {BLL.SessionManager.GetInstance().GetUsuario().Nombre}";
            }
            else if (5 <= DateTime.Now.Hour && DateTime.Now.Hour <= 11)
            {
                saludo = $"Buen día, {BLL.SessionManager.GetInstance().GetUsuario().Nombre}";
            }
            else if (12 <= DateTime.Now.Hour && DateTime.Now.Hour <= 19)
            {
                saludo = $"Buenas tardes, {BLL.SessionManager.GetInstance().GetUsuario().Nombre}";
            }
            else if (20 <= DateTime.Now.Hour && DateTime.Now.Hour <= 23)
            {
                saludo = $"Buenas noches, {BLL.SessionManager.GetInstance().GetUsuario().Nombre}";
            }
            
            labelSaludo.Text = saludo;
        }
    }
}
