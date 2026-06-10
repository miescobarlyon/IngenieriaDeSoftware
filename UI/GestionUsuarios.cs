using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UI
{
    public partial class GestionUsuarios : TranslatableForm
    {
        private readonly Main principal;
        private readonly UsuarioService usuarioService = new UsuarioService();
        private readonly ErrorManagerService errorManager =
            ErrorManagerService.GetInstance();

        public GestionUsuarios(Main main)
        {
            InitializeComponent();
            principal = main;
            errorManager.OnOcurrioError += ErrorManager_OnOcurrioError;
            ConfigurarGrilla();
        }

        // ── Load ──────────────────────────────────────────────────────────────

        private void GestionUsuarios_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
            ActualizarEncabezados();
            ActualizarBotones();
        }

        // ── IIdiomaObserver override ──────────────────────────────────────────

        public override void CambiarIdioma(BE.Idioma idioma)
        {
            base.CambiarIdioma(idioma);
            ActualizarEncabezados();
        }

        // ── Grid setup ────────────────────────────────────────────────────────

        private void ConfigurarGrilla()
        {
            dataGridViewUsuarios.AutoGenerateColumns = false;
            dataGridViewUsuarios.AllowUserToAddRows = false;
            dataGridViewUsuarios.AllowUserToDeleteRows = false;
            dataGridViewUsuarios.ReadOnly = true;
            dataGridViewUsuarios.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsuarios.MultiSelect = false;

            var svc = IdiomaService.GetInstance();

            var colId = new DataGridViewTextBoxColumn();
            colId.Name = "Id";
            colId.DataPropertyName = "Id";
            colId.Visible = false;

            var colNombre = new DataGridViewTextBoxColumn();
            colNombre.Name = "Nombre";
            colNombre.DataPropertyName = "Nombre";
            colNombre.HeaderText = svc.Traducir("lbl.nombre");
            colNombre.Width = 160;

            var colApellido = new DataGridViewTextBoxColumn();
            colApellido.Name = "Apellido";
            colApellido.DataPropertyName = "Apellido";
            colApellido.HeaderText = svc.Traducir("lbl.apellido");
            colApellido.Width = 160;

            var colUser = new DataGridViewTextBoxColumn();
            colUser.Name = "User";
            colUser.DataPropertyName = "User";
            colUser.HeaderText = svc.Traducir("lbl.usuario");
            colUser.Width = 160;

            var colEstado = new DataGridViewTextBoxColumn();
            colEstado.Name = "Estado";
            colEstado.DataPropertyName = "Estado";   // computed property on BE.Usuario
            colEstado.HeaderText = svc.Traducir("lbl.estado");
            colEstado.Width = 100;

            dataGridViewUsuarios.Columns.Add(colId);
            dataGridViewUsuarios.Columns.Add(colNombre);
            dataGridViewUsuarios.Columns.Add(colApellido);
            dataGridViewUsuarios.Columns.Add(colUser);
            dataGridViewUsuarios.Columns.Add(colEstado);
        }

        private void ActualizarEncabezados()
        {
            var svc = IdiomaService.GetInstance();
            var cols = dataGridViewUsuarios.Columns;
            if (cols["Nombre"] != null) cols["Nombre"].HeaderText = svc.Traducir("lbl.nombre");
            if (cols["Apellido"] != null) cols["Apellido"].HeaderText = svc.Traducir("lbl.apellido");
            if (cols["User"] != null) cols["User"].HeaderText = svc.Traducir("lbl.usuario");
            if (cols["Estado"] != null) cols["Estado"].HeaderText = svc.Traducir("lbl.estado");
        }

        // ── Data ──────────────────────────────────────────────────────────────

        private void CargarUsuarios()
        {
            dataGridViewUsuarios.DataSource = null;
            dataGridViewUsuarios.DataSource = UsuarioService.ListarActivos();
        }

        // ── Button state ──────────────────────────────────────────────────────

        /// <summary>
        /// Enables/disables action buttons based on whether a row is selected.
        /// Also updates the block/unblock button text to reflect the selected
        /// user's current state.
        /// </summary>
        private void ActualizarBotones()
        {
            bool haySeleccion = ObtenerUsuarioSeleccionado() != null;
            buttonEditar.Enabled = haySeleccion;
            buttonBloquear.Enabled = haySeleccion;
            buttonVerHistorial.Enabled = haySeleccion;

            var svc = IdiomaService.GetInstance();
            if (haySeleccion)
            {
                var u = ObtenerUsuarioSeleccionado();
                buttonBloquear.Text = u.Bloqueado == 1
                    ? svc.Traducir("btn.desbloquear")
                    : svc.Traducir("btn.bloquear");
                buttonBloquear.Tag = u.Bloqueado == 1
                    ? "btn.desbloquear"
                    : "btn.bloquear";
            }
            else
            {
                buttonBloquear.Text = svc.Traducir("btn.bloquear");
                buttonBloquear.Tag = "btn.bloquear";
            }
        }

        private BE.Usuario ObtenerUsuarioSeleccionado()
        {
            if (dataGridViewUsuarios.CurrentRow == null) return null;
            return dataGridViewUsuarios.CurrentRow.DataBoundItem as BE.Usuario;
        }

        // ── Button handlers ───────────────────────────────────────────────────

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            principal.LoadForm(new FormUsuario(principal, null));
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            var usuario = ObtenerUsuarioSeleccionado();
            if (usuario == null) return;
            principal.LoadForm(new FormUsuario(principal, usuario));
        }

        private void buttonBloquear_Click(object sender, EventArgs e)
        {
            var usuario = ObtenerUsuarioSeleccionado();
            if (usuario == null) return;

            try
            {
                if (usuario.Bloqueado == 1)
                    UsuarioService.Desbloquear(usuario);
                else
                    UsuarioService.BloquearAdHoc(usuario);

                CargarUsuarios();
                ActualizarBotones();
            }
            catch (Exception ex)
            {
                errorManager.ManejarError(ex, BE.EnumError.Error);
            }
        }

        private void buttonVerHistorial_Click(object sender, EventArgs e)
        {
            var usuario = ObtenerUsuarioSeleccionado();
            if (usuario == null) return;
            principal.LoadForm(new HistorialUsuarioForm(principal, usuario));
        }

        private void dataGridViewUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotones();
        }

        // ── Error display ─────────────────────────────────────────────────────

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

        // ── Cleanup ───────────────────────────────────────────────────────────

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                errorManager.OnOcurrioError -= ErrorManager_OnOcurrioError;
            }
            base.Dispose(disposing);
        }
    }
}
