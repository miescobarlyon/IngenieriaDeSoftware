# Agregar Idioma Form — Implementation Guide

## Overview

This guide adds a runtime **Agregar Idioma** screen to the multi-language system implemented in `MULTILANG_IMPLEMENTATION.md`. The form lets a user type a new language name and code, fill in translations for every existing key against a Spanish reference column, and persist everything to the database. As soon as the user saves, the new language appears in the `Main` menu without restarting the application.

### How it fits into the existing architecture

The existing `IdiomaService` already owns language state and notifies observers. This guide extends it with two new responsibilities:

- **`AgregarIdioma(nombre, codigo)`** — inserts the new language, fires a new `OnIdiomaAgregado` event.
- **`AgregarTraducciones(traducciones)`** — bulk-inserts translation rows and invalidates the cache.

`Main.cs` subscribes to `OnIdiomaAgregado` and rebuilds its language menu in-place. The form itself inherits `TranslatableForm`, so it translates itself the moment it opens and again whenever the user switches language while filling it in.

### Files to create (3 new files)

| File | Purpose |
|---|---|
| `UI/AgregarIdioma.cs` | Form logic |
| `UI/AgregarIdioma.Designer.cs` | Control layout |
| `UI/AgregarIdioma.resx` | Required resource stub |

### Files to modify (5 files)

| File | Change |
|---|---|
| `DB/MigracionAgregarIdiomaForm.sql` | New migration — alter SP + seed keys |
| `DAL/MP_Idioma.cs` | Add `AgregarYObtener` method |
| `BLL/IdiomaService.cs` | Add event + 3 new methods |
| `UI/Main.cs` | Subscribe to event, refresh menu, add navigation item |

---

## Phase 1 — Handled by instructor

## Phase 2 — DAL

### 2.1 — Add `AgregarYObtener` to `DAL/MP_Idioma.cs`

This method calls `Acceso.Leer` (not `Escribir`) so it can capture the `IDIOMA_ID` row returned by the altered SP. The existing `Agregar` method is left untouched — it still works for cases where the ID is not needed.

Add the following method to the existing `MP_Idioma` class, after the `Listar()` method:

```csharp
/// <summary>
/// Inserts a new language and returns the same entity with its
/// database-assigned Id populated.
/// Uses Acceso.Leer (not Escribir) to capture the SELECT @id returned
/// by the altered InsertarIdioma stored procedure.
/// </summary>
public BE.Idioma AgregarYObtener(BE.Idioma obj)
{
    acceso = new Acceso();
    try
    {
        acceso.Abrir();
        List<SqlParameter> parametros = new List<SqlParameter>();
        parametros.Add(acceso.CrearParameter("@nombre", obj.Nombre));
        parametros.Add(acceso.CrearParameter("@codigo", obj.Codigo));

        DataTable dt = acceso.Leer("InsertarIdioma", parametros);

        if (dt != null && dt.Rows.Count > 0)
            obj.Id = Convert.ToInt32(dt.Rows[0]["IDIOMA_ID"]);

        return obj;
    }
    finally
    {
        acceso.Cerrar();
    }
}
```

No `.csproj` changes needed — the method is added to an already-registered file.

### Phase 2 verification

Build the `DAL` project alone. It must compile with zero errors.

---

## Phase 3 — BLL

### 3.1 — Update `BLL/IdiomaService.cs`

Three additions are needed inside the existing `IdiomaService` class:

1. A public `OnIdiomaAgregado` event so `Main` can refresh its menu.
2. `AgregarIdioma` — inserts the language, fires the event, returns the populated entity.
3. `AgregarTraducciones` — bulk-inserts translation rows and invalidates the cache.
4. `ListarTraducciones` — exposes reference translations to the UI without it touching DAL directly.

**Add the event declaration** immediately after the `_idiomaActual` field (keep the field grouping intact):

```csharp
// ?? Events ????????????????????????????????????????????????????????????????
/// <summary>
/// Fired after a new language is successfully persisted.
/// Main subscribes to this to rebuild its language switcher menu.
/// </summary>
public event EventHandler<BE.Idioma> OnIdiomaAgregado;
```

**Add the three new methods** after the existing `Traducir` and `ListarIdiomas` methods, still inside the class:

```csharp
// ?? CRUD helpers used by AgregarIdioma form ????????????????????????????????

/// <summary>
/// Inserts a new language into the database and fires OnIdiomaAgregado
/// so all subscribers (e.g. Main menu) can refresh immediately.
/// Returns the entity with its database-assigned Id.
/// </summary>
public BE.Idioma AgregarIdioma(string nombre, string codigo)
{
    var idioma = new BE.Idioma { Nombre = nombre, Codigo = codigo };
    idioma = new DAL.MP_Idioma().AgregarYObtener(idioma);
    OnIdiomaAgregado?.Invoke(this, idioma);
    return idioma;
}

/// <summary>
/// Bulk-inserts a list of translations and invalidates the in-memory cache
/// so the next CambiarIdioma call re-reads from the database.
/// Empty or null Texto values are silently skipped.
/// </summary>
public void AgregarTraducciones(List<BE.Traduccion> traducciones)
{
    if (traducciones == null || traducciones.Count == 0) return;
    var mapper = new DAL.MP_Traduccion();
    foreach (var t in traducciones)
    {
        if (!string.IsNullOrWhiteSpace(t.Texto))
            mapper.Agregar(t);
    }
    _tradService.InvalidarCache();
}

/// <summary>
/// Returns all translations for a given language.
/// Used by AgregarIdioma to populate the reference column of the grid.
/// </summary>
public List<BE.Traduccion> ListarTraducciones(int idiomaId)
{
    return new DAL.MP_Traduccion().Listar(idiomaId);
}
```

No `.csproj` changes needed — the methods are added to an already-registered file.

### Phase 3 verification

Build the `BLL` project alone. It must compile with zero errors.

---

## Phase 4 — New Form

### 4.1 — Create `UI/AgregarIdioma.cs`

```csharp
using BE;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UI
{
    public partial class AgregarIdioma : TranslatableForm
    {
        // ?? Constants ??????????????????????????????????????????????????????????
        /// <summary>
        /// IDIOMA_ID of the reference language used to populate the grid's
        /// second column. Spanish (1) is the authoritative source for all keys.
        /// </summary>
        private const int IDIOMA_REFERENCIA_ID = 1;

        // ?? Fields ?????????????????????????????????????????????????????????????
        private readonly Main principal;
        private readonly BLL.ErrorManagerService errorManager =
            BLL.ErrorManagerService.GetInstance();

        // ?? Constructor ????????????????????????????????????????????????????????

        public AgregarIdioma(Main main)
        {
            InitializeComponent();
            principal = main;
            errorManager.OnOcurrioError += ErrorManager_OnOcurrioError;
            ConfigurarGrilla();
        }

        // ?? Load ???????????????????????????????????????????????????????????????

        private void AgregarIdioma_Load(object sender, EventArgs e)
        {
            CargarIdiomasExistentes();
            CargarTraduccionesReferencia();
            // TranslatableForm.OnLoad already fired Suscribir ? OnIdiomaCambiado,
            // which translated Tags. Call ActualizarEncabezados here so that the
            // column headers (set in code, not via Tag) are also up to date.
            ActualizarEncabezados();
        }

        // ?? IIdiomaObserver override ???????????????????????????????????????????

        /// <summary>
        /// Called by IdiomaService when the active language changes.
        /// base.OnIdiomaCambiado translates all Tag-based controls;
        /// this override additionally refreshes the DataGridView column headers
        /// because they are set programmatically, not via the Tag property.
        /// </summary>
        public override void OnIdiomaCambiado(BE.Idioma idioma)
        {
            base.OnIdiomaCambiado(idioma);
            ActualizarEncabezados();
        }

        // ?? Grid setup ?????????????????????????????????????????????????????????

        /// <summary>
        /// Adds three columns to the DataGridView:
        ///   - Clave       : translation key (read-only)
        ///   - Referencia  : Spanish text (read-only, for context)
        ///   - Traduccion  : user input for the new language
        /// Column headers use the translation service; the fallback (key name)
        /// is visible for a brief moment before OnIdiomaCambiado fires on load.
        /// </summary>
        private void ConfigurarGrilla()
        {
            dataGridViewTraducciones.Columns.Clear();
            dataGridViewTraducciones.AllowUserToAddRows    = false;
            dataGridViewTraducciones.AllowUserToDeleteRows = false;
            dataGridViewTraducciones.RowHeadersVisible     = false;
            dataGridViewTraducciones.SelectionMode         =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTraducciones.AutoSizeColumnsMode   =
                DataGridViewAutoSizeColumnsMode.None;

            var svc = BLL.IdiomaService.GetInstance();

            var colClave = new DataGridViewTextBoxColumn();
            colClave.Name       = "Clave";
            colClave.HeaderText = svc.Traducir("lbl.clave");
            colClave.ReadOnly   = true;
            colClave.Width      = 180;
            colClave.SortMode   = DataGridViewColumnSortMode.NotSortable;

            var colRef = new DataGridViewTextBoxColumn();
            colRef.Name       = "Referencia";
            colRef.HeaderText = svc.Traducir("lbl.referencia");
            colRef.ReadOnly   = true;
            colRef.Width      = 290;
            colRef.SortMode   = DataGridViewColumnSortMode.NotSortable;

            var colTrad = new DataGridViewTextBoxColumn();
            colTrad.Name       = "Traduccion";
            colTrad.HeaderText = svc.Traducir("lbl.traduccion");
            colTrad.ReadOnly   = false;
            colTrad.Width      = 290;
            colTrad.SortMode   = DataGridViewColumnSortMode.NotSortable;

            dataGridViewTraducciones.Columns.Add(colClave);
            dataGridViewTraducciones.Columns.Add(colRef);
            dataGridViewTraducciones.Columns.Add(colTrad);
        }

        private void ActualizarEncabezados()
        {
            var svc = BLL.IdiomaService.GetInstance();
            var cols = dataGridViewTraducciones.Columns;

            if (cols["Clave"]      != null) cols["Clave"].HeaderText      = svc.Traducir("lbl.clave");
            if (cols["Referencia"] != null) cols["Referencia"].HeaderText = svc.Traducir("lbl.referencia");
            if (cols["Traduccion"] != null) cols["Traduccion"].HeaderText = svc.Traducir("lbl.traduccion");
        }

        // ?? Data loading ???????????????????????????????????????????????????????

        private void CargarIdiomasExistentes()
        {
            listBoxIdiomas.Items.Clear();
            var idiomas = BLL.IdiomaService.GetInstance().ListarIdiomas();
            foreach (var idioma in idiomas)
                listBoxIdiomas.Items.Add($"{idioma.Nombre}  ({idioma.Codigo})");
        }

        private void CargarTraduccionesReferencia()
        {
            dataGridViewTraducciones.Rows.Clear();
            var referencia = BLL.IdiomaService.GetInstance()
                                .ListarTraducciones(IDIOMA_REFERENCIA_ID);
            foreach (var t in referencia)
                dataGridViewTraducciones.Rows.Add(t.Clave, t.Texto, string.Empty);
        }

        // ?? Button handlers ????????????????????????????????????????????????????

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;

            try
            {
                // 1. Insert the new language; IdiomaService fires OnIdiomaAgregado
                //    which Main catches to rebuild the language menu.
                var nuevoIdioma = BLL.IdiomaService.GetInstance().AgregarIdioma(
                    textBoxNombre.Text.Trim(),
                    textBoxCodigo.Text.Trim()
                );

                // 2. Collect non-empty translation rows.
                var traducciones = new List<Traduccion>();
                foreach (DataGridViewRow row in dataGridViewTraducciones.Rows)
                {
                    if (row.IsNewRow) continue;
                    string texto = row.Cells["Traduccion"].Value?.ToString()?.Trim();
                    if (string.IsNullOrEmpty(texto)) continue;

                    traducciones.Add(new Traduccion
                    {
                        Clave    = row.Cells["Clave"].Value.ToString(),
                        IdiomaId = nuevoIdioma.Id,
                        Texto    = texto
                    });
                }

                // 3. Persist translations (empty rows are silently skipped inside
                //    AgregarTraducciones; the key fallback in Traducir handles them).
                BLL.IdiomaService.GetInstance().AgregarTraducciones(traducciones);

                MessageBox.Show(
                    $"Idioma '{nuevoIdioma.Nombre}' agregado con " +
                    $"{traducciones.Count} traducciones.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                errorManager.ManejarError(ex, EnumError.Error);
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            principal.LoadForm(new Inicio(principal));
        }

        // ?? Validation ?????????????????????????????????????????????????????????

        /// <summary>
        /// Returns true only if all required fields are valid.
        /// Fires the ErrorManagerService event (shows a MessageBox) on failure
        /// so the form's own subscription handles the display — consistent with
        /// how Form1 (Log_In) handles validation errors.
        /// </summary>
        private bool Validar()
        {
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text))
            {
                errorManager.ManejarError(
                    "El nombre del idioma no puede estar vacío.",
                    EnumError.Advertencia);
                textBoxNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBoxCodigo.Text))
            {
                errorManager.ManejarError(
                    "El código del idioma no puede estar vacío.",
                    EnumError.Advertencia);
                textBoxCodigo.Focus();
                return false;
            }

            // Uniqueness check — client-side against already-loaded list.
            var idiomas = BLL.IdiomaService.GetInstance().ListarIdiomas();
            bool codigoExiste = idiomas.Exists(i =>
                i.Codigo.Equals(textBoxCodigo.Text.Trim(),
                                StringComparison.OrdinalIgnoreCase));

            if (codigoExiste)
            {
                errorManager.ManejarError(
                    $"Ya existe un idioma con el código '{textBoxCodigo.Text.Trim()}'.",
                    EnumError.Advertencia);
                textBoxCodigo.Focus();
                return false;
            }

            return true;
        }

        // ?? Helpers ????????????????????????????????????????????????????????????

        private void LimpiarFormulario()
        {
            textBoxNombre.Clear();
            textBoxCodigo.Clear();

            foreach (DataGridViewRow row in dataGridViewTraducciones.Rows)
                if (!row.IsNewRow)
                    row.Cells["Traduccion"].Value = string.Empty;

            // Refresh the existing-languages list to show the newly added one.
            CargarIdiomasExistentes();
            textBoxNombre.Focus();
        }

        // ?? Error display ??????????????????????????????????????????????????????

        private void ErrorManager_OnOcurrioError(object sender, BE.Error e)
        {
            MessageBoxIcon icon;
            switch (e.Tipo)
            {
                case EnumError.Info:      icon = MessageBoxIcon.Information; break;
                case EnumError.Advertencia: icon = MessageBoxIcon.Warning;  break;
                case EnumError.Error:     icon = MessageBoxIcon.Error;      break;
                case EnumError.Critico:   icon = MessageBoxIcon.Stop;       break;
                default:                  icon = MessageBoxIcon.None;       break;
            }
            MessageBox.Show(e.Mensaje, "Notificación", MessageBoxButtons.OK, icon);
        }

        // ?? Cleanup ????????????????????????????????????????????????????????????

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                errorManager.OnOcurrioError -= ErrorManager_OnOcurrioError;
            base.Dispose(disposing); // TranslatableForm.Dispose unsubscribes from IdiomaService
        }
    }
}
```

### 4.2 — Create `UI/AgregarIdioma.Designer.cs`

This file defines the static control layout. Column configuration and data loading are done in code (see `ConfigurarGrilla` and `CargarTraduccionesReferencia` above). Tags are set here so `TranslatableForm.AplicarTraducciones` picks them up automatically.

```csharp
namespace UI
{
    partial class AgregarIdioma
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.labelNombre               = new System.Windows.Forms.Label();
            this.textBoxNombre             = new System.Windows.Forms.TextBox();
            this.labelCodigo               = new System.Windows.Forms.Label();
            this.textBoxCodigo             = new System.Windows.Forms.TextBox();
            this.labelIdiomasExistentes    = new System.Windows.Forms.Label();
            this.listBoxIdiomas            = new System.Windows.Forms.ListBox();
            this.labelTraducciones         = new System.Windows.Forms.Label();
            this.dataGridViewTraducciones  = new System.Windows.Forms.DataGridView();
            this.buttonGuardar             = new System.Windows.Forms.Button();
            this.buttonCancelar            = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTraducciones)).BeginInit();
            this.SuspendLayout();

            // ?? labelNombre ??????????????????????????????????????????????????
            this.labelNombre.AutoSize = true;
            this.labelNombre.Location = new System.Drawing.Point(12, 22);
            this.labelNombre.Name     = "labelNombre";
            this.labelNombre.Size     = new System.Drawing.Size(46, 13);
            this.labelNombre.TabIndex = 0;
            this.labelNombre.Text     = "Nombre";
            this.labelNombre.Tag      = "lbl.nombreIdioma";

            // ?? textBoxNombre ????????????????????????????????????????????????
            this.textBoxNombre.Location = new System.Drawing.Point(66, 18);
            this.textBoxNombre.Name     = "textBoxNombre";
            this.textBoxNombre.Size     = new System.Drawing.Size(200, 22);
            this.textBoxNombre.TabIndex = 1;

            // ?? labelCodigo ??????????????????????????????????????????????????
            this.labelCodigo.AutoSize = true;
            this.labelCodigo.Location = new System.Drawing.Point(282, 22);
            this.labelCodigo.Name     = "labelCodigo";
            this.labelCodigo.Size     = new System.Drawing.Size(44, 13);
            this.labelCodigo.TabIndex = 2;
            this.labelCodigo.Text     = "Código";
            this.labelCodigo.Tag      = "lbl.codigoIdioma";

            // ?? textBoxCodigo ????????????????????????????????????????????????
            // Intentionally narrow — codes like "es-AR", "en", "pt-BR" are short.
            this.textBoxCodigo.Location = new System.Drawing.Point(334, 18);
            this.textBoxCodigo.Name     = "textBoxCodigo";
            this.textBoxCodigo.Size     = new System.Drawing.Size(100, 22);
            this.textBoxCodigo.TabIndex = 3;

            // ?? labelIdiomasExistentes ???????????????????????????????????????
            this.labelIdiomasExistentes.AutoSize = true;
            this.labelIdiomasExistentes.Location = new System.Drawing.Point(470, 10);
            this.labelIdiomasExistentes.Name     = "labelIdiomasExistentes";
            this.labelIdiomasExistentes.Size     = new System.Drawing.Size(110, 13);
            this.labelIdiomasExistentes.TabIndex = 4;
            this.labelIdiomasExistentes.Text     = "Idiomas existentes";
            this.labelIdiomasExistentes.Tag      = "lbl.idiomasExistentes";

            // ?? listBoxIdiomas ???????????????????????????????????????????????
            // Read-only display — SelectionMode.None prevents user interaction.
            this.listBoxIdiomas.FormattingEnabled = true;
            this.listBoxIdiomas.Location          = new System.Drawing.Point(470, 28);
            this.listBoxIdiomas.Name              = "listBoxIdiomas";
            this.listBoxIdiomas.SelectionMode     = System.Windows.Forms.SelectionMode.None;
            this.listBoxIdiomas.Size              = new System.Drawing.Size(466, 82);
            this.listBoxIdiomas.TabIndex          = 5;

            // ?? labelTraducciones ????????????????????????????????????????????
            this.labelTraducciones.AutoSize = true;
            this.labelTraducciones.Location = new System.Drawing.Point(12, 132);
            this.labelTraducciones.Name     = "labelTraducciones";
            this.labelTraducciones.Size     = new System.Drawing.Size(74, 13);
            this.labelTraducciones.TabIndex = 6;
            this.labelTraducciones.Text     = "Traducciones";
            this.labelTraducciones.Tag      = "lbl.traducciones";

            // ?? dataGridViewTraducciones ?????????????????????????????????????
            // Columns are added in ConfigurarGrilla() — not here.
            this.dataGridViewTraducciones.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTraducciones.Location = new System.Drawing.Point(12, 152);
            this.dataGridViewTraducciones.Name     = "dataGridViewTraducciones";
            this.dataGridViewTraducciones.Size     = new System.Drawing.Size(934, 310);
            this.dataGridViewTraducciones.TabIndex = 7;

            // ?? buttonGuardar ????????????????????????????????????????????????
            this.buttonGuardar.Location = new System.Drawing.Point(746, 482);
            this.buttonGuardar.Name     = "buttonGuardar";
            this.buttonGuardar.Size     = new System.Drawing.Size(96, 28);
            this.buttonGuardar.TabIndex = 8;
            this.buttonGuardar.Text     = "Guardar";
            this.buttonGuardar.Tag      = "btn.guardar";
            this.buttonGuardar.UseVisualStyleBackColor = true;
            this.buttonGuardar.Click += new System.EventHandler(this.buttonGuardar_Click);

            // ?? buttonCancelar ???????????????????????????????????????????????
            this.buttonCancelar.Location = new System.Drawing.Point(852, 482);
            this.buttonCancelar.Name     = "buttonCancelar";
            this.buttonCancelar.Size     = new System.Drawing.Size(96, 28);
            this.buttonCancelar.TabIndex = 9;
            this.buttonCancelar.Text     = "Cancelar";
            this.buttonCancelar.Tag      = "btn.cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);

            // ?? AgregarIdioma (form) ?????????????????????????????????????????
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(960, 524);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.buttonGuardar);
            this.Controls.Add(this.dataGridViewTraducciones);
            this.Controls.Add(this.labelTraducciones);
            this.Controls.Add(this.listBoxIdiomas);
            this.Controls.Add(this.labelIdiomasExistentes);
            this.Controls.Add(this.textBoxCodigo);
            this.Controls.Add(this.labelCodigo);
            this.Controls.Add(this.textBoxNombre);
            this.Controls.Add(this.labelNombre);
            this.Name = "AgregarIdioma";
            this.Text = "Agregar Idioma";
            this.Tag  = "titulo.agregarIdioma";
            this.Load += new System.EventHandler(this.AgregarIdioma_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTraducciones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label    labelNombre;
        private System.Windows.Forms.TextBox  textBoxNombre;
        private System.Windows.Forms.Label    labelCodigo;
        private System.Windows.Forms.TextBox  textBoxCodigo;
        private System.Windows.Forms.Label    labelIdiomasExistentes;
        private System.Windows.Forms.ListBox  listBoxIdiomas;
        private System.Windows.Forms.Label    labelTraducciones;
        private System.Windows.Forms.DataGridView dataGridViewTraducciones;
        private System.Windows.Forms.Button   buttonGuardar;
        private System.Windows.Forms.Button   buttonCancelar;
    }
}
```

### Phase 4 verification

Build the full solution. It must compile with zero errors. The `AgregarIdioma` form does not appear in the UI yet — navigation is wired in Phase 5.

---

## Phase 5 — Wire Navigation in `UI/Main.cs`

Two changes to `Main.cs`:

1. Subscribe to `IdiomaService.OnIdiomaAgregado` so the menu refreshes automatically when the user saves a new language.
2. Add an "Agregar idioma" item (with a separator) at the bottom of the dynamically built `idiomaMenu`.

**Replace the entire `UI/Main.cs` file** with the version below. Differences from the previous guide's version are marked with `// ? NEW` comments.

```csharp
using BLL;
using System;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class Main : TranslatableForm
    {
        private BLL.SessionManager sm = SessionManager.GetInstance();

        // ? NEW: stored so we can unsubscribe cleanly in Dispose.
        private EventHandler<BE.Idioma> _onIdiomaAgregadoHandler;

        public Main()
        {
            InitializeComponent();

            // ? NEW: subscribe before building the menu so any language added
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

            form.TopLevel        = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock            = DockStyle.Fill;
            panelContenido.Controls.Add(form);
            form.Show();
        }

        /// <summary>
        /// Builds (or rebuilds) the language switcher menu from the database.
        /// Called once at startup and again each time a new language is added.
        /// Rebuilding removes the old menu item first to avoid duplicates.
        /// </summary>
        private void CargarMenuIdiomas()
        {
            // Remove existing Idioma menu to allow a clean rebuild.  ? NEW block
            var existing = menuStrip1.Items
                .OfType<ToolStripMenuItem>()
                .FirstOrDefault(i => (i.Tag as string) == "menu.idioma");
            if (existing != null)
                menuStrip1.Items.Remove(existing);

            var svc     = BLL.IdiomaService.GetInstance();
            var idiomas = svc.ListarIdiomas();

            var idiomaMenu = new ToolStripMenuItem();
            idiomaMenu.Tag  = "menu.idioma";
            idiomaMenu.Text = svc.Traducir("menu.idioma");

            foreach (var idioma in idiomas)
            {
                var capturedId = idioma.Id;
                var item       = new ToolStripMenuItem(idioma.Nombre);
                item.Click    += (s, e) => svc.CambiarIdioma(capturedId);
                idiomaMenu.DropDownItems.Add(item);
            }

            // ? NEW: separator + "Agregar idioma" shortcut at bottom of menu.
            idiomaMenu.DropDownItems.Add(new ToolStripSeparator());
            var agregarItem  = new ToolStripMenuItem();
            agregarItem.Tag  = "menu.agregarIdioma";
            agregarItem.Text = svc.Traducir("menu.agregarIdioma");
            agregarItem.Click += (s, e) => LoadForm(new AgregarIdioma(this));
            idiomaMenu.DropDownItems.Add(agregarItem);

            menuStrip1.Items.Add(idiomaMenu);
        }

        // ?? Cleanup ????????????????????????????????????????????????????????????

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                BLL.IdiomaService.GetInstance().OnIdiomaAgregado -=
                    _onIdiomaAgregadoHandler; // ? NEW
            base.Dispose(disposing); // TranslatableForm.Dispose handles IdiomaService
        }

        // ?? Menu event handlers ????????????????????????????????????????????????

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
```

No changes needed to `Main.Designer.cs` — the "Agregar idioma" item is created in code, not in the Designer.

### Phase 5 verification

Build the full solution. It must compile with zero errors.

---

## Phase 6 — End-to-End Verification

Run these checks manually in order.

**6.1 — Form opens correctly**
- Log in and navigate to `Idioma > Agregar idioma`.
- The form appears embedded in `panelContenido`.
- The grid is populated with all existing translation keys and their Spanish reference text.
- The "Idiomas existentes" list shows "Español (es-AR)" and "English (en)".
- All labels and buttons display in the active language.

**6.2 — Translation works on the new form**
- While the form is open, switch the active language via the menu (e.g. `Idioma > English`).
- All labels on the form update immediately: "Nombre" ? "Name", "Guardar" ? "Save", grid column headers update.
- The reference column still shows Spanish text (this is correct by design — it is always the reference language).

**6.3 — Validation**
- Click "Guardar" / "Save" with the name field empty. A warning MessageBox appears; no DB call is made.
- Fill in the name but leave the code empty. Same warning for the code field.
- Fill in both fields with data matching an existing language code (e.g. `es-AR`). A warning about duplicate code appears.

**6.4 — Add a new language (French)**
- Name: `Français`, Code: `fr`
- Fill in at least 5 translation values in the third column.
- Leave some rows empty.
- Click "Guardar". A success dialog shows the count of saved translations.
- The name and code fields clear; the "Idiomas existentes" list now shows the French entry.

**6.5 — Menu refreshes automatically**
- Without navigating away, open the "Idioma" menu.
- "Français" appears as a new item in the switcher, above the separator.
- No application restart was needed.

**6.6 — New language is switchable**
- Click `Idioma > Français`.
- Controls that had translations filled in update to the French text.
- Controls whose rows were left empty fall back to displaying their key name (e.g. `menu.sesion`) — this is the expected fallback behaviour from `TraduccionService.Traducir`.

**6.8 — Cancel navigates home**
- Open `Idioma > Agregar idioma` again.
- Click "Cancelar" / "Cancel".
- The `Inicio` panel reappears.

---

## Notes for future developers

**Adding a new translation key to a new form:** insert one row per language via `EXEC InsertarTraduccion`. When the `AgregarIdioma` form is next opened, the new key appears automatically in the grid because it reads from the database every time.

**Changing the reference language:** the constant `IDIOMA_REFERENCIA_ID = 1` in `AgregarIdioma.cs` is the only place to update. The reference language is Spanish because all keys were originally seeded in Spanish. If a third language were made the reference, it would need complete coverage of all keys.

**Partial translations are intentional:** `TraduccionService.Traducir` falls back to the key name if a translation is missing. This means a language can be added with partial coverage — untranslated controls show their key, which is meaningful to developers and unambiguous to administrators. Warn the user of this in the form (optional UX improvement).

**The `OnIdiomaAgregado` event vs re-querying:** `Main` rebuilds the menu by calling `ListarIdiomas()` (a DB read) inside `CargarMenuIdiomas`. An alternative is to receive the new `BE.Idioma` directly from the event args and append only the new item. The current approach is simpler and the DB round-trip cost is negligible for a menu that rebuilds infrequently.