using BE;
using BLL;
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class HistorialUsuarioForm : TranslatableForm
    {
        private readonly Main principal;
        private readonly BE.Usuario usuario;
        private readonly ErrorManagerService errorManager =
            ErrorManagerService.GetInstance();

        public HistorialUsuarioForm(Main main, BE.Usuario usuarioConsultado)
        {
            InitializeComponent();
            principal = main;
            usuario = usuarioConsultado;
            errorManager.OnOcurrioError += ErrorManager_OnOcurrioError;
            ConfigurarGrilla();
        }

        // ── Load ──────────────────────────────────────────────────────────────

        private void HistorialUsuarioForm_Load(object sender, EventArgs e)
        {
            var svc = IdiomaService.GetInstance();
            labelEncabezado.Text =
                $"{svc.Traducir("lbl.historialDe")} {usuario.Nombre} {usuario.Apellido}" +
                $"  ({usuario.User})";

            CargarHistorial();
            ActualizarEncabezados();
        }

        // ── IIdiomaObserver override ──────────────────────────────────────────

        public override void CambiarIdioma(BE.Idioma idioma)
        {
            base.CambiarIdioma(idioma);
            ActualizarEncabezados();
            // Re-apply the header label (it contains translated prefix + user data).
            var svc = IdiomaService.GetInstance();
            labelEncabezado.Text =
                $"{svc.Traducir("lbl.historialDe")} {usuario.Nombre} {usuario.Apellido}" +
                $"  ({usuario.User})";
        }

        // ── Grid ──────────────────────────────────────────────────────────────

        private void ConfigurarGrilla()
        {
            dataGridViewHistorial.AutoGenerateColumns = false;
            dataGridViewHistorial.AllowUserToAddRows = false;
            dataGridViewHistorial.AllowUserToDeleteRows = false;
            dataGridViewHistorial.ReadOnly = true;
            dataGridViewHistorial.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridViewHistorial.MultiSelect = false;

            var svc = IdiomaService.GetInstance();

            var colId = new DataGridViewTextBoxColumn();
            colId.Name = "Id";
            colId.DataPropertyName = "Id";
            colId.Visible = false;

            var colFecha = new DataGridViewTextBoxColumn();
            colFecha.Name = "FechaModificacion";
            colFecha.DataPropertyName = "FechaModificacion";
            colFecha.HeaderText = svc.Traducir("lbl.fecha");
            colFecha.Width = 160;
            colFecha.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";

            var colMod = new DataGridViewTextBoxColumn();
            colMod.Name = "ModificadorNombre";
            colMod.DataPropertyName = "ModificadorNombre";
            colMod.HeaderText = svc.Traducir("lbl.modificadoPor");
            colMod.Width = 180;

            var colAccion = new DataGridViewTextBoxColumn();
            colAccion.Name = "Accion";
            colAccion.DataPropertyName = "Accion";
            colAccion.HeaderText = svc.Traducir("lbl.accion");
            colAccion.Width = 110;

            var colNombre = new DataGridViewTextBoxColumn();
            colNombre.Name = "Nombre";
            colNombre.DataPropertyName = "Nombre";
            colNombre.HeaderText = svc.Traducir("lbl.nombre");
            colNombre.Width = 130;

            var colApellido = new DataGridViewTextBoxColumn();
            colApellido.Name = "Apellido";
            colApellido.DataPropertyName = "Apellido";
            colApellido.HeaderText = svc.Traducir("lbl.apellido");
            colApellido.Width = 130;

            var colUser = new DataGridViewTextBoxColumn();
            colUser.Name = "User";
            colUser.DataPropertyName = "User";
            colUser.HeaderText = svc.Traducir("lbl.usuario");
            colUser.Width = 130;

            dataGridViewHistorial.Columns.Add(colId);
            dataGridViewHistorial.Columns.Add(colFecha);
            dataGridViewHistorial.Columns.Add(colMod);
            dataGridViewHistorial.Columns.Add(colAccion);
            dataGridViewHistorial.Columns.Add(colNombre);
            dataGridViewHistorial.Columns.Add(colApellido);
            dataGridViewHistorial.Columns.Add(colUser);
        }

        private void ActualizarEncabezados()
        {
            var svc = IdiomaService.GetInstance();
            var cols = dataGridViewHistorial.Columns;
            if (cols["FechaModificacion"] != null)
                cols["FechaModificacion"].HeaderText = svc.Traducir("lbl.fecha");
            if (cols["ModificadorNombre"] != null)
                cols["ModificadorNombre"].HeaderText = svc.Traducir("lbl.modificadoPor");
            if (cols["Accion"] != null) cols["Accion"].HeaderText = svc.Traducir("lbl.accion");
            if (cols["Nombre"] != null) cols["Nombre"].HeaderText = svc.Traducir("lbl.nombre");
            if (cols["Apellido"] != null) cols["Apellido"].HeaderText = svc.Traducir("lbl.apellido");
            if (cols["User"] != null) cols["User"].HeaderText = svc.Traducir("lbl.usuario");
        }

        private void CargarHistorial()
        {
            dataGridViewHistorial.DataSource = null;
            dataGridViewHistorial.DataSource =
                HistorialUsuarioService.Listar(usuario.Id);
        }

        // ── Button handlers ───────────────────────────────────────────────────

        private void buttonRevertir_Click(object sender, EventArgs e)
        {
            if (dataGridViewHistorial.CurrentRow == null) return;

            var snap = dataGridViewHistorial.CurrentRow.DataBoundItem
                       as BE.HistorialUsuario;
            if (snap == null) return;

            var confirmacion = MessageBox.Show(
                $"¿Revertir '{usuario.Nombre} {usuario.Apellido}' al estado del " +
                $"{snap.FechaModificacion:dd/MM/yyyy HH:mm}?\n\n" +
                "Esta acción actualizará nombre, apellido, usuario y contraseña.",
                "Confirmar reversión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion != DialogResult.Yes) return;

            try
            {
                HistorialUsuarioService.Revertir(snap.Id);
                CargarHistorial();   // refresh — new REVERSION row will appear at top
                MessageBox.Show("Reversión aplicada correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                errorManager.ManejarError(ex, BE.EnumError.Error);
            }
        }

        private void buttonVolver_Click(object sender, EventArgs e)
        {
            principal.LoadForm(new GestionUsuarios(principal));
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
