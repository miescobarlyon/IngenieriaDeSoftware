using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class GestionRoles : TranslatableForm
    {
        private Main principal;
        private BLL.GestorPermisos gestor = new BLL.GestorPermisos();

        public GestionRoles(Main main)
        {
            InitializeComponent();
            principal = main;
            CargarRoles();
            CargarUsuarios();
        }

        // === Cargas iniciales ===

        private void CargarRoles()
        {
            listBoxRoles.DataSource = null;
            listBoxRoles.DataSource = gestor.ObtenerGrupos();
            listBoxRoles.DisplayMember = "Nombre";
        }

        private void CargarUsuarios()
        {
            cboUsuarios.DataSource = null;
            cboUsuarios.DataSource = BLL.UsuarioService.Listar();
            cboUsuarios.DisplayMember = "User";
        }

        private void CargarPermisosDelRol()
        {
            clbPermisos.Items.Clear();
            if (listBoxRoles.SelectedItem == null) return;

            var rolSeleccionado = (GrupoPermiso)listBoxRoles.SelectedItem;
            var hijosActuales = gestor.ObtenerHijosDirectos(rolSeleccionado.Codigo);
            var codigosActuales = new HashSet<string>(hijosActuales.Select(h => h.Codigo));

            // Todos los componentes menos el rol que estoy editando (evita auto-referencia)
            var todos = gestor.ObtenerTodos()
                .Where(c => c.Codigo != rolSeleccionado.Codigo);

            foreach (var comp in todos)
                clbPermisos.Items.Add(comp, codigosActuales.Contains(comp.Codigo));

            clbPermisos.DisplayMember = "Codigo";
        }

        // === Roles: crear / eliminar ===
        private void btnCrearRol_Click(object sender, EventArgs e)
        {
            try
            {
                string codigo = txtCodigoRol.Text.Trim();
                string nombre = txtNombreRol.Text.Trim();
                if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(nombre))
                {
                    MessageBox.Show("Completá código y nombre.");
                    return;
                }
                gestor.CrearGrupo(codigo, nombre);
                txtCodigoRol.Clear();
                txtNombreRol.Clear();
                CargarRoles();
                MessageBox.Show($"Rol '{codigo}' creado.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnEliminarRol_Click(object sender, EventArgs e)
        {
            if (listBoxRoles.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un rol.");
                return;
            }
            var rol = (GrupoPermiso)listBoxRoles.SelectedItem;
            if (MessageBox.Show($"¿Eliminar el rol '{rol.Codigo}'?", "Confirmar",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            try
            {
                gestor.Eliminar(rol.Codigo);
                CargarRoles();
                clbPermisos.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // === Permisos del rol: guardar cambios ===

        private void btnGuardarPermisos_Click(object sender, EventArgs e)
        {
            if (listBoxRoles.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un rol.");
                return;
            }
            var rol = (GrupoPermiso)listBoxRoles.SelectedItem;

            try
            {
                var actuales = new HashSet<string>(
                    gestor.ObtenerHijosDirectos(rol.Codigo).Select(h => h.Codigo));

                var deseados = new HashSet<string>(
                    clbPermisos.CheckedItems.Cast<ComponentePermiso>().Select(c => c.Codigo));

                foreach (var codigo in deseados.Except(actuales))
                    gestor.AgregarHijo(rol.Codigo, codigo);

                foreach (var codigo in actuales.Except(deseados))
                    gestor.QuitarHijo(rol.Codigo, codigo);

                MessageBox.Show("Permisos del rol actualizados.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // === Asignar / quitar rol a un usuario ===

        private void btnAsignarRol_Click(object sender, EventArgs e)
        {
            if (listBoxRoles.SelectedItem == null || cboUsuarios.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un rol y un usuario.");
                return;
            }
            var rol = (GrupoPermiso)listBoxRoles.SelectedItem;
            var usuario = (Usuario)cboUsuarios.SelectedItem;

            try
            {
                gestor.AsignarAUsuario(usuario.Id, rol.Codigo);
                MessageBox.Show($"Rol '{rol.Codigo}' asignado a {usuario.User}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnQuitarRol_Click(object sender, EventArgs e)
        {
            if (listBoxRoles.SelectedItem == null || cboUsuarios.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un rol y un usuario.");
                return;
            }
            var rol = (GrupoPermiso)listBoxRoles.SelectedItem;
            var usuario = (Usuario)cboUsuarios.SelectedItem;

            try
            {
                gestor.QuitarDeUsuario(usuario.Id, rol.Codigo);
                MessageBox.Show($"Rol '{rol.Codigo}' quitado a {usuario.User}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void listBoxRoles_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CargarPermisosDelRol();
        }
    }
}