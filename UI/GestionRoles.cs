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
            CargarArbol();
            CargarUsuarios();
            CargarDisponibles();
        }

        private void CargarArbol()
        {
            treeRoles.BeginUpdate();
            treeRoles.Nodes.Clear();

            foreach (var grupo in gestor.ObtenerGrupos())
            {
                var arbol = gestor.ObtenerArbolDe(grupo.Codigo);
                treeRoles.Nodes.Add(ConstruirNodo(arbol));
            }

            var todos = gestor.ObtenerTodos();
            var simples = todos.OfType<PermisoSimple>();
            var hijosDeAlguien = new HashSet<int>(
                gestor.ObtenerGrupos()
                      .SelectMany(g => gestor.ObtenerHijosDirectos(g.Codigo))
                      .Select(c => c.Id));

            foreach (var p in simples.Where(p => !hijosDeAlguien.Contains(p.Id)))
                treeRoles.Nodes.Add(ConstruirNodo(p));

            treeRoles.ExpandAll();
            treeRoles.EndUpdate();
        }

        private TreeNode ConstruirNodo(ComponentePermiso comp)
        {
            string etiqueta = comp is GrupoPermiso
                ? $"[Grupo] {comp.Codigo} - {comp.Nombre}"
                : $"{comp.Codigo} - {comp.Nombre}";

            var nodo = new TreeNode(etiqueta) { Tag = comp };

            if (comp is GrupoPermiso grupo)
            {
                foreach (var hijo in grupo.ObtenerHijos())
                    nodo.Nodes.Add(ConstruirNodo(hijo));
            }
            return nodo;
        }

        private void CargarUsuarios()
        {
            cboUsuarios.DataSource = null;
            cboUsuarios.DataSource = BLL.UsuarioService.Listar();
            cboUsuarios.DisplayMember = "User";
        }

        private void CargarDisponibles()
        {
            var seleccionado = NodoSeleccionado();
            var todos = gestor.ObtenerTodos();

            if (seleccionado is GrupoPermiso grupo)
            {
                var hijosDirectos = new HashSet<string>(
                    gestor.ObtenerHijosDirectos(grupo.Codigo).Select(h => h.Codigo));
                todos = todos
                    .Where(c => c.Codigo != grupo.Codigo && !hijosDirectos.Contains(c.Codigo))
                    .ToList();
            }

            lstDisponibles.DataSource = null;
            lstDisponibles.DataSource = todos;
            lstDisponibles.DisplayMember = "Codigo";
        }

        private ComponentePermiso NodoSeleccionado()
        {
            return treeRoles.SelectedNode?.Tag as ComponentePermiso;
        }

        private ComponentePermiso PadreSeleccionado()
        {
            var nodo = treeRoles.SelectedNode;
            return nodo?.Parent?.Tag as ComponentePermiso;
        }

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
                CargarArbol();
                CargarDisponibles();
                MessageBox.Show($"Rol '{codigo}' creado.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnEliminarRol_Click(object sender, EventArgs e)
        {
            var seleccionado = NodoSeleccionado();
            if (!(seleccionado is GrupoPermiso grupo))
            {
                MessageBox.Show("Seleccioná un rol (grupo) para eliminar.");
                return;
            }

            if (MessageBox.Show($"¿Eliminar el rol '{grupo.Codigo}'?", "Confirmar",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            try
            {
                gestor.Eliminar(grupo.Codigo);
                CargarArbol();
                CargarDisponibles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnAgregarHijo_Click(object sender, EventArgs e)
        {
            var seleccionado = NodoSeleccionado();
            if (!(seleccionado is GrupoPermiso grupo))
            {
                MessageBox.Show("Seleccioná un grupo en el árbol al que agregarle un hijo.");
                return;
            }
            var hijo = lstDisponibles.SelectedItem as ComponentePermiso;
            if (hijo == null)
            {
                MessageBox.Show("Seleccioná un componente de la lista para agregar.");
                return;
            }

            try
            {
                gestor.AgregarHijo(grupo.Codigo, hijo.Codigo);
                CargarArbol();
                CargarDisponibles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnQuitarHijo_Click(object sender, EventArgs e)
        {
            var hijo = NodoSeleccionado();
            var padre = PadreSeleccionado();
            if (hijo == null || !(padre is GrupoPermiso grupoPadre))
            {
                MessageBox.Show("Seleccioná un nodo hijo (uno que cuelgue de un grupo) para quitarlo.");
                return;
            }

            try
            {
                gestor.QuitarHijo(grupoPadre.Codigo, hijo.Codigo);
                CargarArbol();
                CargarDisponibles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnAsignarRol_Click(object sender, EventArgs e)
        {
            var seleccionado = NodoSeleccionado();
            if (!(seleccionado is GrupoPermiso grupo) || cboUsuarios.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un rol (grupo) y un usuario.");
                return;
            }
            var usuario = (Usuario)cboUsuarios.SelectedItem;

            try
            {
                gestor.AsignarAUsuario(usuario.Id, grupo.Codigo);
                MessageBox.Show($"Rol '{grupo.Codigo}' asignado a {usuario.User}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnQuitarRol_Click(object sender, EventArgs e)
        {
            var seleccionado = NodoSeleccionado();
            if (!(seleccionado is GrupoPermiso grupo) || cboUsuarios.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un rol (grupo) y un usuario.");
                return;
            }
            var usuario = (Usuario)cboUsuarios.SelectedItem;

            try
            {
                gestor.QuitarDeUsuario(usuario.Id, grupo.Codigo);
                MessageBox.Show($"Rol '{grupo.Codigo}' quitado a {usuario.User}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void treeRoles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CargarDisponibles();
        }
    }
}
