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
        private BLL.SessionManager sm = SessionManager.GetInstance();

        // ← NEW: stored so we can unsubscribe cleanly in Dispose.
        private EventHandler<BE.Idioma> _onIdiomaAgregadoHandler;

        public Main()
        {
            InitializeComponent();

            // ← NEW: subscribe before building the menu so any language added
            //   between startup and menu build is not missed (edge case).
            _onIdiomaAgregadoHandler = (sender, idioma) =>
            {
                // Always marshal back to the UI thread.
                if (InvokeRequired) { Invoke(new Action(CargarMenuIdiomas)); return; }
                CargarMenuIdiomas();
            };
            BLL.IdiomaService.GetInstance().OnIdiomaAgregado += _onIdiomaAgregadoHandler;

            CargarMenuIdiomas();
            LoadForm(new Inicio(this));

            if (BLL.IdiomaService.GetInstance().IdiomaActual == null)
                BLL.IdiomaService.GetInstance().CambiarIdioma(1);
        }

        /// <summary>
        /// Embeds a child form inside panelContenido.
        /// Disposes the previous form first to release its observer subscription.
        /// </summary>
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
            var existing = menuStrip1.Items
                .OfType<ToolStripMenuItem>()
                .FirstOrDefault(i => (i.Tag as string) == "menu.idioma");
            if (existing != null)
                menuStrip1.Items.Remove(existing);

            var svc = BLL.IdiomaService.GetInstance();
            var idiomas = svc.ListarIdiomas();

            var idiomaMenu = new ToolStripMenuItem();
            idiomaMenu.Tag = "menu.idioma";
            idiomaMenu.Text = svc.Traducir("menu.idioma");

            foreach (var idioma in idiomas)
            {
                var capturedId = idioma.Id;
                var item = new ToolStripMenuItem(idioma.Nombre);
                item.Click += (s, e) => svc.CambiarIdioma(capturedId);
                idiomaMenu.DropDownItems.Add(item);
            }

            idiomaMenu.DropDownItems.Add(new ToolStripSeparator());
            var agregarItem = new ToolStripMenuItem();
            agregarItem.Tag = "menu.agregarIdioma";
            agregarItem.Text = svc.Traducir("menu.agregarIdioma");
            agregarItem.Click += (s, e) => LoadForm(new AgregarIdioma(this));
            idiomaMenu.DropDownItems.Add(agregarItem);

            menuStrip1.Items.Add(idiomaMenu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                BLL.IdiomaService.GetInstance().OnIdiomaAgregado -=
                    _onIdiomaAgregadoHandler;
            }
            base.Dispose(disposing); 
        }

        // ── Menu event handlers ────────────────────────────────────────────────

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sm.Logout();

            var log = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (log == null)
                log = new Form1();

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
