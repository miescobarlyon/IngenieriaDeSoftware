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
        private EventHandler<BE.Idioma> _onIdiomaAgregadoHandler;

        public Main()
        {
            InitializeComponent();
            _onIdiomaAgregadoHandler = (sender, idioma) =>
            {
                if (InvokeRequired) { Invoke(new Action(CargarMenuIdiomas)); return; }
                CargarMenuIdiomas();
            };
            BLL.IdiomaService.GetInstance().OnIdiomaAgregado += _onIdiomaAgregadoHandler;

            CargarMenuIdiomas();
            var sm = SessionManager.GetInstance();
            if (sm.DbCorrupta && sm.TienePermiso("BD.RESTAURAR"))
                LoadForm(new RecuperacionDB(this));
            else
                LoadForm(new Inicio(this));

            if (BLL.IdiomaService.GetInstance().IdiomaActual == null)
                BLL.IdiomaService.GetInstance().CambiarIdioma(1);
            AplicarPermisos();
        }

        private void AplicarPermisos()
        {
            var sm = BLL.SessionManager.GetInstance();
            bitácoraToolStripMenuItem.Visible = sm.TienePermiso("BITACORA.VER");
            gestionRolesToolStripMenuItem.Visible    = sm.TienePermiso("PERMISOS.GESTIONAR");
            recuperacionDBToolStripMenuItem.Visible = sm.TienePermiso("BD.RESTAURAR");
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

            if (BLL.SessionManager.GetInstance().TienePermiso("IDIOMAS.GESTIONAR"))
            {
                idiomaMenu.DropDownItems.Add(new ToolStripSeparator());
                var agregarItem = new ToolStripMenuItem();
                agregarItem.Tag = "menu.agregarIdioma";
                agregarItem.Text = svc.Traducir("menu.agregarIdioma");
                agregarItem.Click += (s, e) => LoadForm(new AgregarIdioma(this));
                idiomaMenu.DropDownItems.Add(agregarItem);
            }

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

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(new GestionUsuarios(this));
        }

        private void gestionRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(new GestionRoles(this));
        }

        private void recuperacionDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(new RecuperacionDB(this));
        }
    }
}
