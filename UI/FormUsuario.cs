using BE;
using BLL;
using System;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class FormUsuario : TranslatableForm
    {
        private readonly Main principal;
        private readonly UsuarioService usuarioService = new UsuarioService();
        private readonly ErrorManagerService errorManager =
            ErrorManagerService.GetInstance();

        // null → Add mode;  non-null → Edit mode
        private readonly BE.Usuario _usuarioOriginal;
        private bool IsEditMode => _usuarioOriginal != null;

        public FormUsuario(Main main, BE.Usuario usuarioAEditar)
        {
            InitializeComponent();
            principal = main;
            _usuarioOriginal = usuarioAEditar;
            errorManager.OnOcurrioError += ErrorManager_OnOcurrioError;
        }

        // ── Load ──────────────────────────────────────────────────────────────

        private void FormUsuario_Load(object sender, EventArgs e)
        {
            labelNotaContrasena.Visible = IsEditMode;

            if (IsEditMode)
            {
                this.Tag = "titulo.editarUsuario";
                this.Text = IdiomaService.GetInstance().Traducir("titulo.editarUsuario");
                textBoxNombre.Text = _usuarioOriginal.Nombre;
                textBoxApellido.Text = _usuarioOriginal.Apellido;
                textBoxUsuario.Text = _usuarioOriginal.User;
                // Password fields are left blank intentionally —
                // empty = keep existing credentials.
            }
            else
            {
                this.Tag = "titulo.nuevoUsuario";
                this.Text = IdiomaService.GetInstance().Traducir("titulo.nuevoUsuario");
            }
        }

        // ── Button handlers ───────────────────────────────────────────────────

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;

            try
            {
                BE.Usuario usuario = IsEditMode
                    ? CopiarCamposEditados(_usuarioOriginal)
                    : new BE.Usuario();

                usuario.Nombre = textBoxNombre.Text.Trim();
                usuario.Apellido = textBoxApellido.Text.Trim();
                usuario.User = textBoxUsuario.Text.Trim();

                bool hayNuevaContrasena = textBoxContrasena.Text.Length > 0;

                if (hayNuevaContrasena)
                {
                    // Hash and salt the new password inside BLL.
                    usuarioService.HashearPassword(usuario, textBoxContrasena.Text);
                }
                else if (IsEditMode)
                {
                    // No password change — preserve existing credentials.
                    usuario.PasswordHash = _usuarioOriginal.PasswordHash;
                    usuario.Salt = _usuarioOriginal.Salt;
                }

                usuarioService.Guardar(usuario);

                principal.LoadForm(new GestionUsuarios(principal));
            }
            catch (Exception ex)
            {
                errorManager.ManejarError(ex, BE.EnumError.Error);
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            principal.LoadForm(new GestionUsuarios(principal));
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>
        /// Creates a shallow copy of the original entity so that the edit
        /// carries the correct Id (and Borrado / Bloqueado state) into Guardar.
        /// </summary>
        private BE.Usuario CopiarCamposEditados(BE.Usuario original)
        {
            return new BE.Usuario
            {
                Id = original.Id,
                Borrado = original.Borrado,
                Bloqueado = original.Bloqueado
            };
        }

        // ── Validation ────────────────────────────────────────────────────────

        private bool Validar()
        {
            string nombre = textBoxNombre.Text.Trim();
            string apellido = textBoxApellido.Text.Trim();
            string usuario = textBoxUsuario.Text.Trim();
            string pass = textBoxContrasena.Text;
            string confirmar = textBoxConfirmar.Text;

            if (string.IsNullOrEmpty(nombre))
            {
                errorManager.ManejarError("El nombre no puede estar vacío.",
                    BE.EnumError.Advertencia);
                textBoxNombre.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(apellido))
            {
                errorManager.ManejarError("El apellido no puede estar vacío.",
                    BE.EnumError.Advertencia);
                textBoxApellido.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(usuario))
            {
                errorManager.ManejarError("El nombre de usuario no puede estar vacío.",
                    BE.EnumError.Advertencia);
                textBoxUsuario.Focus();
                return false;
            }

            // Username uniqueness — skip the current user's own record in edit mode.
            var todos = UsuarioService.Listar();
            bool existe = todos.Any(u =>
                u.User.Equals(usuario, StringComparison.OrdinalIgnoreCase)
                && u.Id != (_usuarioOriginal?.Id ?? 0));

            if (existe)
            {
                errorManager.ManejarError(
                    $"El nombre de usuario '{usuario}' ya está en uso.",
                    BE.EnumError.Advertencia);
                textBoxUsuario.Focus();
                return false;
            }

            // Password required for new users; optional for edits.
            if (!IsEditMode && string.IsNullOrEmpty(pass))
            {
                errorManager.ManejarError("La contraseña es obligatoria para un nuevo usuario.",
                    BE.EnumError.Advertencia);
                textBoxContrasena.Focus();
                return false;
            }

            if (pass.Length > 0 && pass != confirmar)
            {
                errorManager.ManejarError("Las contraseñas no coinciden.",
                    BE.EnumError.Advertencia);
                textBoxConfirmar.Focus();
                return false;
            }

            return true;
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
