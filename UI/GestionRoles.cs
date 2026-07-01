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

            cboUsuarios.SelectedIndexChanged += cboUsuarios_SelectedIndexChanged;
            CargarPermisosUsuario();
        }

        private void cboUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPermisosUsuario();
        }

        // Muestra en treeUsuario el árbol de permisos efectivo del usuario elegido.
        private void CargarPermisosUsuario()
        {
            treeUsuario.BeginUpdate();
            treeUsuario.Nodes.Clear();

            var usuario = cboUsuarios.SelectedItem as Usuario;
            if (usuario != null)
            {
                var arbol = gestor.ObtenerPermisosDeUsuario(usuario.Id);

                // ObtenerPermisosDeUsuario devuelve un nodo artificial "ROOT"/"EMPTY"
                // cuando el usuario tiene varios (o ningún) permiso directo:
                // en ese caso mostramos sus hijos como raíces, no el nodo artificial.
                if (arbol is GrupoPermiso && (arbol.Codigo == "ROOT" || arbol.Codigo == "EMPTY"))
                {
                    var hijos = ((GrupoPermiso)arbol).ObtenerHijos();
                    if (hijos.Count == 0)
                        treeUsuario.Nodes.Add(new TreeNode("(sin permisos asignados)"));
                    else
                        foreach (var h in hijos)
                            treeUsuario.Nodes.Add(ConstruirNodo(h));
                }
                else
                {
                    treeUsuario.Nodes.Add(ConstruirNodo(arbol));
                }
            }

            treeUsuario.ExpandAll();
            treeUsuario.EndUpdate();
        }

        private void CargarArbol()
        {
            treeRoles.BeginUpdate();
            treeRoles.Nodes.Clear();

            var grupos = gestor.ObtenerGrupos();

            // Todo lo que es hijo directo de ALGÚN grupo (para saber qué permisos simples
            // están sueltos y cuáles ya pertenecen a un grupo).
            var hijosDeAlguien = new HashSet<int>(
                grupos.SelectMany(g => gestor.ObtenerHijosDirectos(g.Codigo))
                      .Select(c => c.Id));

            // TODOS los grupos se muestran arriba, así siempre podés ver y gestionar
            // cualquier rol aunque esté anidado dentro de otro. Los subgrupos anidados
            // se muestran como referencia (ver ConstruirNodoResumen), no se re-expanden.
            foreach (var grupo in grupos)
            {
                var arbol = gestor.ObtenerArbolDe(grupo.Codigo);
                treeRoles.Nodes.Add(ConstruirNodoResumen(arbol, esRaiz: true));
            }

            // Permisos simples que no pertenecen a ningún grupo (sueltos).
            var simplesSueltos = gestor.ObtenerTodos()
                .OfType<PermisoSimple>()
                .Where(p => !hijosDeAlguien.Contains(p.Id));

            foreach (var p in simplesSueltos)
                treeRoles.Nodes.Add(ConstruirNodoResumen(p, esRaiz: true));

            treeRoles.ExpandAll();
            treeRoles.EndUpdate();
        }

        // Árbol de administración (treeRoles): expande el grupo raíz, pero si un grupo
        // aparece ANIDADO dentro de otro lo muestra como referencia, sin volver a
        // expandir su contenido. Evita duplicar subárboles enteros y, a la vez,
        // deja que todo grupo siga visible en su propio nodo del nivel superior.
        private TreeNode ConstruirNodoResumen(ComponentePermiso comp, bool esRaiz)
        {
            bool esGrupo = comp is GrupoPermiso;

            string etiqueta = esGrupo
                ? $"[Grupo] {comp.Codigo} - {comp.Nombre}"
                : $"{comp.Codigo} - {comp.Nombre}";

            ;

            var nodo = new TreeNode(etiqueta) { Tag = comp };

            if (esGrupo && esRaiz)
            {
                foreach (var hijo in ((GrupoPermiso)comp).ObtenerHijos())
                    nodo.Nodes.Add(ConstruirNodoResumen(hijo, esRaiz: false));
            }

            return nodo;
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
            var seleccionado = treeRoles.SelectedNode?.Tag as ComponentePermiso;
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
                CargarPermisosUsuario();
                MessageBox.Show($"Rol '{grupo.Codigo}' asignado a {usuario.User}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnQuitarRol_Click(object sender, EventArgs e)
        {
            var seleccionado = treeUsuario.SelectedNode?.Tag as ComponentePermiso;
            if (!(seleccionado is GrupoPermiso grupo) || cboUsuarios.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un rol (grupo) y un usuario.");
                return;
            }
            var usuario = (Usuario)cboUsuarios.SelectedItem;

            try
            {
                gestor.QuitarDeUsuario(usuario.Id, grupo.Codigo);
                CargarPermisosUsuario();
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