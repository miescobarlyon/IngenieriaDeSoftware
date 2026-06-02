using BLL;
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
    public partial class Main : TranslatableForm
    {
        BLL.SessionManager sm = SessionManager.GetInstance();

        public Main()
        {
            InitializeComponent();
            CargarMenuIdiomas();
            LoadForm(new Inicio(this));

            if (BLL.IdiomaService.GetInstance().IdiomaActual == null)
                BLL.IdiomaService.GetInstance().CambiarIdioma(1);
        }

        public void LoadForm(Form form)
        {
            foreach (Control c in panelContenido.Controls)
                c.Dispose();
            panelContenido.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(form);
            form.Show();
        }

        private void CargarMenuIdiomas()
        {
            List<BE.Idioma> idiomas = BLL.IdiomaService.GetInstance().ListarIdiomas();
            if (idiomas == null || idiomas.Count == 0) return;

            ToolStripMenuItem idiomaMenu = new ToolStripMenuItem();
            idiomaMenu.Tag = "menu.idioma";

            foreach (BE.Idioma idioma in idiomas)
            {
                int capturedId = idioma.Id;
                ToolStripMenuItem item = new ToolStripMenuItem(idioma.Nombre);
                item.Click += (s, e) =>
                    BLL.IdiomaService.GetInstance().CambiarIdioma(capturedId);
                idiomaMenu.DropDownItems.Add(item);
            }

            menuStrip1.Items.Add(idiomaMenu);
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sm.Logout();

            Form1 log = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (log == null)
            {
                log = new Form1();
            }

            log.Show();
            log.BringToFront();
            this.Close();
        }

        private void bitácoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(new Bitacora(this));
        }

        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(new Inicio(this));
        }
    }
}
