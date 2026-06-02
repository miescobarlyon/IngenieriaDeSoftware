# Multi-Language System — Implementation Guide

## Overview

This guide implements a dynamic multi-language (multi-idioma) system using the **Observer pattern** across the `IngenieriaDeSoftware` solution. The system allows any form to receive language updates automatically without per-form wiring. New languages and translations are added exclusively through the database — zero code changes required.

### How it works

- `IdiomaService` (singleton) is the **subject**. It holds the active language and a list of subscribers.
- Every form that inherits `TranslatableForm` implements `IIdiomaObserver` and **auto-subscribes on load**, **auto-unsubscribes on close**.
- Controls that need translation have their `Tag` property set to a **translation key** (e.g. `"btn.iniciarSesion"`).
- On language change, `IdiomaService` calls `OnIdiomaCambiado()` on every subscriber. Each form iterates its own control tree and looks up each `Tag` key in an in-memory dictionary — **one DB round-trip per language change, cached thereafter**.

### Architecture map

```
UI (TranslatableForm) ??subscribes??? BLL (IdiomaService, IIdiomaObserver)
                                            ?
                                     uses  ?
                                   BLL (TraduccionService)
                                            ?
                                     uses  ?
                                   DAL (MP_Traduccion, MP_Idioma)
                                            ?
                                     reads ?
                                   DB  (IDIOMA, TRADUCCION)
```

### Files to create (8 new files)

| File | Purpose |
|---|---|
| `BE/Idioma.cs` | Entity for the IDIOMA table |
| `BE/Traduccion.cs` | Entity for the TRADUCCION table |
| `DAL/MP_Idioma.cs` | Mapper for IDIOMA |
| `DAL/MP_Traduccion.cs` | Mapper for TRADUCCION |
| `BLL/IIdiomaObserver.cs` | Observer interface |
| `BLL/TraduccionService.cs` | Cache + lookup logic |
| `BLL/IdiomaService.cs` | Singleton subject |
| `UI/TranslatableForm.cs` | Base Form with auto-wiring |

### Files to modify (7 files)

`UI/Log_In.cs`, `UI/Log_In.Designer.cs`, `UI/Main.cs`, `UI/Main.Designer.cs`,
`UI/Inicio.cs` (full replacement), `UI/Bitacora.cs`, `UI/Bitacora.Designer.cs`)

---

## Phase 1 - Will be executed by the instructor

## Phase 2 — Business Entities (BE)

### 2.1 — Create `BE/Idioma.cs`

```csharp
using System;

namespace BE
{
    public class Idioma
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string codigo;
        /// <summary>
        /// BCP-47 locale code, e.g. "es-AR", "en".
        /// Used to identify the language without relying on the numeric Id.
        /// </summary>
        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public override string ToString()
        {
            return nombre;
        }
    }
}
```

### 2.2 — Create `BE/Traduccion.cs`

```csharp
namespace BE
{
    public class Traduccion
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string clave;
        /// <summary>
        /// Dot-separated key, e.g. "btn.iniciarSesion".
        /// Must match the Tag property set on the WinForms control.
        /// </summary>
        public string Clave
        {
            get { return clave; }
            set { clave = value; }
        }

        private int idiomaId;
        public int IdiomaId
        {
            get { return idiomaId; }
            set { idiomaId = value; }
        }

        private string texto;
        public string Texto
        {
            get { return texto; }
            set { texto = value; }
        }
    }
}
```

### Phase 2 verification

Build the `BE` project alone (`Build > Build BE`). It must compile with zero errors before proceeding.

---

## Phase 3 — Data Access Layer (DAL)

### 3.1 — Create `DAL/MP_Idioma.cs`

```csharp
using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MP_Idioma : MAPPER<Idioma>
    {
        public override int Agregar(Idioma obj)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@nombre", obj.Nombre));
            parametros.Add(acceso.CrearParameter("@codigo", obj.Codigo));
            int res = acceso.Escribir("InsertarIdioma", parametros);
            acceso.Cerrar();
            return res;
        }

        public override int Modificar(Idioma obj)
        {
            throw new NotImplementedException();
        }

        public override int Eliminar(Idioma obj)
        {
            throw new NotImplementedException();
        }

        public override List<Idioma> Listar()
        {
            acceso = new Acceso();
            acceso.Abrir();
            DataTable dt = acceso.Leer("ListarIdiomas");
            acceso.Cerrar();

            List<Idioma> lista = new List<Idioma>();
            if (dt == null) return lista;

            foreach (DataRow row in dt.Rows)
            {
                Idioma i = new Idioma();
                i.Id     = Convert.ToInt32(row["IDIOMA_ID"]);
                i.Nombre = row["NOMBRE"].ToString();
                i.Codigo = row["CODIGO"].ToString();
                lista.Add(i);
            }
            return lista;
        }

        public Idioma TraerPorId(int id)
        {
            acceso = new Acceso();
            try
            {
                acceso.Abrir();
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acceso.CrearParameter("@id", id));
                DataTable dt = acceso.Leer("TraerIdiomaPorId", parametros);

                if (dt == null || dt.Rows.Count == 0) return null;

                DataRow row = dt.Rows[0];
                Idioma i = new Idioma();
                i.Id     = Convert.ToInt32(row["IDIOMA_ID"]);
                i.Nombre = row["NOMBRE"].ToString();
                i.Codigo = row["CODIGO"].ToString();
                return i;
            }
            finally
            {
                acceso.Cerrar();
            }
        }
    }
}
```

### 3.2 — Create `DAL/MP_Traduccion.cs`

```csharp
using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MP_Traduccion : MAPPER<Traduccion>
    {
        public override int Agregar(Traduccion obj)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParameter("@clave",    obj.Clave));
            parametros.Add(acceso.CrearParameter("@idiomaId", obj.IdiomaId));
            parametros.Add(acceso.CrearParameter("@texto",    obj.Texto));
            int res = acceso.Escribir("InsertarTraduccion", parametros);
            acceso.Cerrar();
            return res;
        }

        public override int Modificar(Traduccion obj)
        {
            throw new NotImplementedException();
        }

        public override int Eliminar(Traduccion obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not used directly — always call Listar(int idiomaId) instead.
        /// </summary>
        public override List<Traduccion> Listar()
        {
            throw new InvalidOperationException(
                "Use Listar(int idiomaId) to fetch translations for a specific language.");
        }

        /// <summary>
        /// Returns all translations for a given language in a single DB round-trip.
        /// </summary>
        public List<Traduccion> Listar(int idiomaId)
        {
            acceso = new Acceso();
            try
            {
                acceso.Abrir();
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acceso.CrearParameter("@idiomaId", idiomaId));
                DataTable dt = acceso.Leer("ListarTraduccionesPorIdioma", parametros);

                List<Traduccion> lista = new List<Traduccion>();
                if (dt == null) return lista;

                foreach (DataRow row in dt.Rows)
                {
                    Traduccion t = new Traduccion();
                    t.Id       = Convert.ToInt32(row["TRADUCCION_ID"]);
                    t.Clave    = row["CLAVE"].ToString();
                    t.IdiomaId = Convert.ToInt32(row["IDIOMA_ID"]);
                    t.Texto    = row["TEXTO"].ToString();
                    lista.Add(t);
                }
                return lista;
            }
            finally
            {
                acceso.Cerrar();
            }
        }
    }
}
```

### Phase 3 verification

Build the `DAL` project alone. It must compile with zero errors.

---

## Phase 4 — Business Logic Layer (BLL)

### 4.1 — Create `BLL/IIdiomaObserver.cs`

```csharp
namespace BLL
{
    /// <summary>
    /// Observer interface. Implemented by any class that must react to language changes.
    /// In this system, all forms that inherit TranslatableForm implement this automatically.
    /// </summary>
    public interface IIdiomaObserver
    {
        void OnIdiomaCambiado(BE.Idioma idioma);
    }
}
```

### 4.2 — Create `BLL/TraduccionService.cs`

```csharp
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    /// <summary>
    /// Loads all translations for a language into an in-memory dictionary.
    /// A single DB round-trip occurs per language switch; subsequent lookups are instant.
    /// </summary>
    public class TraduccionService
    {
        private Dictionary<string, string> _cache = new Dictionary<string, string>();
        private int _cachedIdiomaId = -1;

        /// <summary>
        /// Fetches translations from DB and replaces the cache.
        /// No-op if the requested language is already cached.
        /// </summary>
        public void CargarCache(int idiomaId)
        {
            if (_cachedIdiomaId == idiomaId) return;

            var lista = new DAL.MP_Traduccion().Listar(idiomaId);
            _cache = lista.ToDictionary(t => t.Clave, t => t.Texto);
            _cachedIdiomaId = idiomaId;
        }

        /// <summary>
        /// Returns the translated text for a key.
        /// Falls back to the key itself if no translation exists,
        /// so untranslated controls always show something meaningful.
        /// </summary>
        public string Traducir(string clave)
        {
            if (string.IsNullOrEmpty(clave)) return clave;
            return _cache.TryGetValue(clave, out var texto) ? texto : clave;
        }

        /// <summary>Invalidates the cache, forcing a reload on the next CambiarIdioma call.</summary>
        public void InvalidarCache() => _cachedIdiomaId = -1;
    }
}
```

### 4.3 — Create `BLL/IdiomaService.cs`

```csharp
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    /// <summary>
    /// Singleton subject in the Observer pattern.
    /// Manages the active language and notifies all registered IIdiomaObserver instances.
    /// </summary>
    public sealed class IdiomaService
    {
        // ?? Singleton ??????????????????????????????????????????????????????????
        private static IdiomaService _instance;
        private static readonly object _padlock = new object();

        private IdiomaService() { }

        public static IdiomaService GetInstance()
        {
            lock (_padlock)
            {
                if (_instance == null)
                    _instance = new IdiomaService();
                return _instance;
            }
        }

        // ?? State ??????????????????????????????????????????????????????????????
        private readonly List<IIdiomaObserver> _observers = new List<IIdiomaObserver>();
        private readonly TraduccionService _tradService   = new TraduccionService();
        private BE.Idioma _idiomaActual;

        /// <summary>The language currently active in the system.</summary>
        public BE.Idioma IdiomaActual => _idiomaActual;

        // ?? Observer management ????????????????????????????????????????????????

        /// <summary>
        /// Registers an observer. If a language is already active the observer
        /// receives an immediate OnIdiomaCambiado call so the form is translated
        /// as soon as it opens.
        /// </summary>
        public void Suscribir(IIdiomaObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            // Push current state immediately — new forms get translated at open time.
            if (_idiomaActual != null)
                observer.OnIdiomaCambiado(_idiomaActual);
        }

        public void Desuscribir(IIdiomaObserver observer)
        {
            _observers.Remove(observer);
        }

        // ?? Core action ????????????????????????????????????????????????????????

        /// <summary>
        /// Switches the active language and notifies all subscribers.
        /// Translations for the new language are loaded in a single DB call
        /// and cached; the cache is reused for every subsequent label update.
        /// </summary>
        public void CambiarIdioma(int idiomaId)
        {
            _idiomaActual = new DAL.MP_Idioma().TraerPorId(idiomaId);
            if (_idiomaActual == null) return;

            _tradService.CargarCache(idiomaId);

            // ToList() creates a snapshot — safe if a form closes mid-notification.
            foreach (var observer in _observers.ToList())
                observer.OnIdiomaCambiado(_idiomaActual);
        }

        /// <summary>
        /// Convenience wrapper so UI code never needs to reference TraduccionService directly.
        /// </summary>
        public string Traducir(string clave) => _tradService.Traducir(clave);

        /// <summary>Returns all available languages from the database.</summary>
        public List<BE.Idioma> ListarIdiomas() => new DAL.MP_Idioma().Listar();
    }
}
```

### Phase 4 verification

Build the `BLL` project alone. It must compile with zero errors.

---

## Phase 5 — UI Infrastructure

### 5.1 — Create `UI/TranslatableForm.cs`

This is the base class that all forms will inherit. It wires the observer subscription automatically on `OnLoad` and cleans up on `Dispose`. The `AplicarTraducciones` method walks the full control tree — including panels, group boxes, and menu strips — so you never need to call it manually.

```csharp
using System;
using System.Windows.Forms;

namespace UI
{
    /// <summary>
    /// Base Form that participates in the multi-language observer system.
    ///
    /// USAGE:
    ///   1. Change your form's base class from Form to TranslatableForm.
    ///   2. In the Designer, set the Tag property of each translatable control
    ///      to its translation key (e.g. "btn.iniciarSesion").
    ///   3. Set this.Tag on the form itself for the window title.
    ///   4. Done. No further wiring needed.
    ///
    /// ADDING A NEW CONTROL:
    ///   Only step 2 is needed — set its Tag in the Designer.
    ///
    /// ADDING A NEW LANGUAGE:
    ///   Insert rows into the IDIOMA and TRADUCCION tables. Zero code changes.
    /// </summary>
    public abstract class TranslatableForm : Form, BLL.IIdiomaObserver
    {
        private readonly BLL.IdiomaService _service = BLL.IdiomaService.GetInstance();

        // ?? Lifecycle ??????????????????????????????????????????????????????????

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Subscribing here guarantees the form is fully initialised before
            // receiving its first translation push.
            _service.Suscribir(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _service.Desuscribir(this);
            base.Dispose(disposing);
        }

        // ?? IIdiomaObserver ????????????????????????????????????????????????????

        /// <summary>
        /// Called by IdiomaService whenever the active language changes.
        /// Thread-safe: marshals to the UI thread automatically if needed.
        /// Override in subclasses to handle dynamic (non-Tag) content,
        /// but always call base.OnIdiomaCambiado(idioma) first.
        /// </summary>
        public virtual void OnIdiomaCambiado(BE.Idioma idioma)
        {
            // InvokeRequired is true when called from a non-UI thread.
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnIdiomaCambiado(idioma)));
                return;
            }

            // Translate the window title using the form's own Tag.
            if (Tag is string formKey && !string.IsNullOrEmpty(formKey))
                Text = _service.Traducir(formKey);

            // Translate all controls recursively.
            AplicarTraducciones(Controls);
        }

        // ?? Internal helpers ???????????????????????????????????????????????????

        /// <summary>
        /// Recursively walks the control tree. Handles both regular controls
        /// (Label, Button, CheckBox, …) and ToolStrip/MenuStrip hierarchies.
        /// </summary>
        private void AplicarTraducciones(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                // Translate the control's Text if it has a string Tag.
                if (control.Tag is string clave && !string.IsNullOrEmpty(clave))
                    control.Text = _service.Traducir(clave);

                // MenuStrip inherits from ToolStrip; its items are NOT in .Controls.
                if (control is ToolStrip toolStrip)
                    AplicarTraduccionesToolStrip(toolStrip.Items);

                // Recurse into panels, GroupBoxes, TabPages, etc.
                if (control.HasChildren)
                    AplicarTraducciones(control.Controls);
            }
        }

        /// <summary>
        /// Walks ToolStripItem collections (MenuStrip items, sub-menus).
        /// ToolStripItem does not inherit from Control, so it needs a separate pass.
        /// </summary>
        private void AplicarTraduccionesToolStrip(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                if (item.Tag is string clave && !string.IsNullOrEmpty(clave))
                    item.Text = _service.Traducir(clave);

                // Recurse into sub-menus (DropDownItems).
                if (item is ToolStripMenuItem menuItem && menuItem.HasDropDownItems)
                    AplicarTraduccionesToolStrip(menuItem.DropDownItems);
            }
        }
    }
}
```

### 5.2 — Replace `UI/Main.cs`

Two changes from the original:

1. The base class changes from `Form` to `TranslatableForm`.
2. `LoadForm` is updated to `Dispose` the previous embedded form (preventing stale observer subscriptions from leaking memory).
3. A new private method `CargarMenuIdiomas` dynamically builds the language switcher from the database.

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

        public Main()
        {
            InitializeComponent();
            CargarMenuIdiomas();
            LoadForm(new Inicio(this));

            // Start in Spanish (ID = 1) if no language has been set yet.
            if (BLL.IdiomaService.GetInstance().IdiomaActual == null)
                BLL.IdiomaService.GetInstance().CambiarIdioma(1);
        }

        /// <summary>
        /// Embeds a child form inside panelContenido.
        /// Disposes the previous form first to release its observer subscription.
        /// </summary>
        public void LoadForm(Form form)
        {
            // Dispose existing child forms so they unsubscribe from IdiomaService.
            foreach (Control c in panelContenido.Controls)
                c.Dispose();
            panelContenido.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(form);
            form.Show();
        }

        /// <summary>
        /// Reads available languages from the DB and adds them as menu items
        /// under the "Idioma" menu. Language names are never translated —
        /// they always appear in their native script.
        /// </summary>
        private void CargarMenuIdiomas()
        {
            var idiomas = BLL.IdiomaService.GetInstance().ListarIdiomas();
            if (idiomas == null || idiomas.Count == 0) return;

            var idiomaMenu = new ToolStripMenuItem();
            idiomaMenu.Tag = "menu.idioma"; // translated by TranslatableForm

            foreach (var idioma in idiomas)
            {
                // Capture loop variable for the closure.
                var capturedId = idioma.Id;
                var item = new ToolStripMenuItem(idioma.Nombre);
                // No Tag — language names always display in their native name.
                item.Click += (s, e) =>
                    BLL.IdiomaService.GetInstance().CambiarIdioma(capturedId);
                idiomaMenu.DropDownItems.Add(item);
            }

            menuStrip1.Items.Add(idiomaMenu);
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

### 5.3 — Update `UI/Main.Designer.cs`

Add `Tag` assignments to the existing menu strip items so `TranslatableForm.AplicarTraduccionesToolStrip` can translate them. These lines belong inside `InitializeComponent()`, immediately after each menu item's existing `Text` assignment.

Locate each block and add the `Tag` line shown:

```csharp
// After:  this.inicioToolStripMenuItem.Text = "Inicio";
this.inicioToolStripMenuItem.Tag = "menu.inicio";

// After:  this.bitácoraToolStripMenuItem.Text = "Bitácora";
this.bitácoraToolStripMenuItem.Tag = "menu.bitacora";

// After:  this.sesionToolStripMenuItem.Text = "Sesión";
this.sesionToolStripMenuItem.Tag = "menu.sesion";

// After:  this.cerrarSesiónToolStripMenuItem.Text = "Cerrar Sesión";
this.cerrarSesiónToolStripMenuItem.Tag = "menu.cerrarSesion";

// On the Main form itself, after:  this.Text = "Sistema Principal";
this.Tag = "titulo.main";
```

### Phase 5 verification

Build the entire solution. It must compile with zero errors before proceeding to Phase 6.

---

## Phase 6 — Migrate existing forms

Each migration follows the same three steps:

1. Change `Form` ? `TranslatableForm` in the `.cs` file.
2. Add `Tag` property assignments in the `.Designer.cs` file (inside `InitializeComponent()`).
3. Handle any dynamic content (content whose text depends on runtime data, not just translation).

### 6.1 — `UI/Log_In.cs`

**Change only the class declaration line:**

```csharp
// BEFORE:
public partial class Form1 : Form

// AFTER:
public partial class Form1 : TranslatableForm
```

No other changes needed. The form has no dynamic text (all labels are static UI chrome).

### 6.2 — `UI/Log_In.Designer.cs`

Add the following `Tag` assignments inside `InitializeComponent()`, immediately after the existing text assignments for each control:

```csharp
// After:  this.button1.Text = "Iniciar Sesión";
this.button1.Tag = "btn.iniciarSesion";

// After:  this.button2.Text = "Salir";
this.button2.Tag = "btn.salir";

// After:  this.checkBox1.Text = "Ocultar";
this.checkBox1.Tag = "chk.ocultar";

// After:  this.Text = "Log In";   (the Form itself, at the bottom of InitializeComponent)
this.Tag = "titulo.login";
```

### 6.3 — `UI/Inicio.cs`

`Inicio` has dynamic content: `labelSaludo` displays the user's name alongside the "Bienvenido" text. Because this text is assembled at runtime, `labelSaludo` does **not** get a `Tag` in the Designer. Instead, `OnIdiomaCambiado` is overridden to rebuild the greeting whenever the language changes.

**Replace the entire file:**

```csharp
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class Inicio : TranslatableForm
    {
        private Main principal;

        public Inicio(Main main)
        {
            InitializeComponent();
            principal = main;
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            ActualizarSaludo();
        }

        /// <summary>
        /// Overrides base so the dynamic greeting is rebuilt every time
        /// the language changes. base.OnIdiomaCambiado() is called first
        /// to translate any other tagged controls on this form.
        /// </summary>
        public override void OnIdiomaCambiado(BE.Idioma idioma)
        {
            base.OnIdiomaCambiado(idioma);
            ActualizarSaludo();
        }

        /// <summary>
        /// Builds the personalised greeting using the translated base word
        /// ("Bienvenido" / "Welcome") and the logged-in user's first name.
        /// </summary>
        private void ActualizarSaludo()
        {
            var usuario = BLL.SessionManager.GetInstance().GetUsuario();
            string base_text = BLL.IdiomaService.GetInstance().Traducir("lbl.bienvenido");

            labelSaludo.Text = usuario != null
                ? $"{base_text}, {usuario.Nombre}!"
                : base_text;
        }
    }
}
```

**In `UI/Inicio.Designer.cs`**, add only the form title tag (do **not** add a Tag to `labelSaludo` — it is handled in code):

```csharp
// After:  this.Text = "Inicio";   (at the bottom of InitializeComponent)
this.Tag = "titulo.inicio";
```

### 6.4 — `UI/Bitacora.cs`

**Change only the class declaration line:**

```csharp
// BEFORE:
public partial class Bitacora : Form

// AFTER:
public partial class Bitacora : TranslatableForm
```

No other changes. All labels are static and will be handled by `TranslatableForm`.

### 6.5 — `UI/Bitacora.Designer.cs`

Add `Tag` assignments inside `InitializeComponent()`, immediately after the existing `Text` assignment for each label:

```csharp
// After:  this.label1.Text = "Filtros";
this.label1.Tag = "lbl.filtros";

// After:  this.label2.Text = "Usuarios";
this.label2.Tag = "lbl.usuarios";

// After:  this.label3.Text = "Desde";
this.label3.Tag = "lbl.desde";

// After:  this.label4.Text = "Hasta";
this.label4.Tag = "lbl.hasta";

// After:  this.label5.Text = "Actividad";
this.label5.Tag = "lbl.actividad";

// After:  this.label6.Text = "Criticidad";
this.label6.Tag = "lbl.criticidad";

// After:  this.label7.Text = "Registros";
this.label7.Tag = "lbl.registros";

// After:  this.Text = "Bitacora";   (the Form itself)
this.Tag = "titulo.bitacora";
```

### Phase 6 verification

Build the full solution. It must compile with zero errors and zero warnings related to the new files.

---

## Phase 7 — End-to-End Verification

Run these checks manually after completing all phases:

**7.1 Application start**
- Launch the app. The login form appears with Spanish labels.
- The console / Output window shows no exceptions.

**7.2 Login flow**
- Log in with a valid user. The `Main` form appears with the "Idioma" menu populated with "Español" and "English".
- The `Inicio` panel greets the user by name: "Bienvenido, {nombre}!".

**7.3 Language switch — English**
- Click `Idioma > English`.
- All visible labels on `Main` and the embedded `Inicio` update immediately without reloading.
- Expected: title becomes "Main System", menu items become "Home", "Activity Log", "Session", "Log Out", "Language"; greeting becomes "Welcome, {nombre}!".

**7.4 Language switch — back to Spanish**
- Click `Idioma > Español`.
- All labels revert to Spanish without any DB call delay (served from cache).

**7.5 Navigation**
- Navigate to `Bitácora` / `Activity Log`. Confirm all filter labels appear in the active language immediately (no second switch needed — the form receives the current language on `OnLoad`).
- Navigate back to `Inicio` / `Home`. Greeting is still in the active language.

**7.6 Re-login**
- Close the Main form (Cerrar Sesión / Log Out). The Login form appears.
- Confirm Login form is in Spanish (default set in `Main()` constructor at first start).

**7.7 Adding a new translation key (regression test)**
- In SQL Server: `EXEC InsertarTraduccion 'lbl.prueba', 1, N'Prueba';` and `EXEC InsertarTraduccion 'lbl.prueba', 2, N'Test';`
- Add a new `Label` to any existing form in the Designer. Set `label.Tag = "lbl.prueba"`.
- Run the app and switch languages. The new label translates automatically.
- This confirms no code changes are ever needed for new keys.

---

## Quick-reference: translation key catalogue

| Key | Español | English | Form |
|---|---|---|---|
| `titulo.login` | Log In | Log In | Form1 |
| `titulo.main` | Sistema Principal | Main System | Main |
| `titulo.inicio` | Inicio | Home | Inicio |
| `titulo.bitacora` | Bitácora | Activity Log | Bitacora |
| `btn.iniciarSesion` | Iniciar Sesión | Log In | Form1 |
| `btn.salir` | Salir | Exit | Form1 |
| `chk.ocultar` | Ocultar | Hide | Form1 |
| `lbl.bienvenido` | Bienvenido | Welcome | Inicio (dynamic) |
| `lbl.filtros` | Filtros | Filters | Bitacora |
| `lbl.usuarios` | Usuarios | Users | Bitacora |
| `lbl.desde` | Desde | From | Bitacora |
| `lbl.hasta` | Hasta | To | Bitacora |
| `lbl.actividad` | Actividad | Activity | Bitacora |
| `lbl.criticidad` | Criticidad | Severity | Bitacora |
| `lbl.registros` | Registros | Records | Bitacora |
| `menu.inicio` | Inicio | Home | Main (menu) |
| `menu.bitacora` | Bitácora | Activity Log | Main (menu) |
| `menu.sesion` | Sesión | Session | Main (menu) |
| `menu.cerrarSesion` | Cerrar Sesión | Log Out | Main (menu) |
| `menu.idioma` | Idioma | Language | Main (menu) |

---

## Notes for future developers

**Adding a new form:** inherit `TranslatableForm`, set `Tag` on controls in the Designer, set `this.Tag` for the title. Nothing else.

**Adding a new language:** insert one row into `IDIOMA` and one row per key into `TRADUCCION`. No recompile.

**Adding a new control to an existing form:** set its `Tag` in the Designer. `TranslatableForm.AplicarTraducciones` will pick it up automatically on the next language change.

**Dynamic content** (text assembled at runtime from data): do not use `Tag`. Override `OnIdiomaCambiado`, call `base.OnIdiomaCambiado(idioma)`, then re-apply the dynamic text using `BLL.IdiomaService.GetInstance().Traducir("your.key")`. See `Inicio.cs` for the pattern.

**Controls without translation** (text boxes, data grids, date pickers): simply leave `Tag` null or empty. The base class ignores them.