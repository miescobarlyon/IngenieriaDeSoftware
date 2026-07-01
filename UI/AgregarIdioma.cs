using BE;
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
    public partial class AgregarIdioma : TranslatableForm
    {
        private const int IDIOMA_REFERENCIA_ID = 1;

        private readonly Main principal;
        private readonly BLL.ErrorManagerService errorManager =
            BLL.ErrorManagerService.GetInstance();

        public AgregarIdioma(Main main)
        {
            InitializeComponent();
            principal = main;
            errorManager.OnOcurrioError += ErrorManager_OnOcurrioError;
            ConfigurarGrilla();
        }

        private void AgregarIdioma_Load(object sender, EventArgs e)
        {
            CargarIdiomasExistentes();
            CargarTraduccionesReferencia();
            ActualizarEncabezados();
        }

        public override void CambiarIdioma(BE.Idioma idioma)
        {
            base.CambiarIdioma(idioma);
            ActualizarEncabezados();
        }

        private void ConfigurarGrilla()
        {
            dataGridViewTraducciones.Columns.Clear();
            dataGridViewTraducciones.AllowUserToAddRows = false;
            dataGridViewTraducciones.AllowUserToDeleteRows = false;
            dataGridViewTraducciones.RowHeadersVisible = false;
            dataGridViewTraducciones.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTraducciones.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None;

            var svc = BLL.IdiomaService.GetInstance();

            var colClave = new DataGridViewTextBoxColumn();
            colClave.Name = "Clave";
            colClave.HeaderText = svc.Traducir("lbl.clave");
            colClave.ReadOnly = true;
            colClave.Width = 180;
            colClave.SortMode = DataGridViewColumnSortMode.NotSortable;

            var colRef = new DataGridViewTextBoxColumn();
            colRef.Name = "Referencia";
            colRef.HeaderText = svc.Traducir("lbl.referencia");
            colRef.ReadOnly = true;
            colRef.Width = 290;
            colRef.SortMode = DataGridViewColumnSortMode.NotSortable;

            var colTrad = new DataGridViewTextBoxColumn();
            colTrad.Name = "Traduccion";
            colTrad.HeaderText = svc.Traducir("lbl.traduccion");
            colTrad.ReadOnly = false;
            colTrad.Width = 290;
            colTrad.SortMode = DataGridViewColumnSortMode.NotSortable;

            dataGridViewTraducciones.Columns.Add(colClave);
            dataGridViewTraducciones.Columns.Add(colRef);
            dataGridViewTraducciones.Columns.Add(colTrad);
        }

        private void ActualizarEncabezados()
        {
            var svc = BLL.IdiomaService.GetInstance();
            var cols = dataGridViewTraducciones.Columns;

            if (cols["Clave"] != null) cols["Clave"].HeaderText = svc.Traducir("lbl.clave");
            if (cols["Referencia"] != null) cols["Referencia"].HeaderText = svc.Traducir("lbl.referencia");
            if (cols["Traduccion"] != null) cols["Traduccion"].HeaderText = svc.Traducir("lbl.traduccion");
        }

        private void CargarIdiomasExistentes()
        {
            listBoxIdiomas.DataSource = null;
            List<BE.Idioma> idiomas = BLL.IdiomaService.GetInstance().ListarIdiomas();
            listBoxIdiomas.DataSource = idiomas;
        }

        private void CargarTraduccionesReferencia()
        {
            dataGridViewTraducciones.Rows.Clear();
            var referencia = BLL.IdiomaService.GetInstance()
                                .ListarTraducciones(IDIOMA_REFERENCIA_ID);
            foreach (var t in referencia)
                dataGridViewTraducciones.Rows.Add(t.Clave, t.Texto, string.Empty);
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;

            try
            {
                var idiomas = BLL.IdiomaService.GetInstance().ListarIdiomas();
                bool codigoExiste = idiomas.Exists(i =>
                i.Codigo.Equals(textBoxCodigo.Text.Trim(),
                                StringComparison.OrdinalIgnoreCase));

                BE.Idioma idiomaAGuardar = new BE.Idioma();

                if (codigoExiste)
                {
                    idiomaAGuardar = BLL.IdiomaService.GetInstance().ListarIdiomas()
                        .FirstOrDefault(i => i.Codigo.Equals(textBoxCodigo.Text.Trim(), StringComparison.OrdinalIgnoreCase));
                }

                idiomaAGuardar.Nombre = textBoxNombre.Text.Trim();
                idiomaAGuardar.Codigo = textBoxCodigo.Text.Trim();

                var nuevoIdioma = BLL.IdiomaService.GetInstance().Guardar(
                    idiomaAGuardar
                );

                var traducciones = new List<Traduccion>();
                var existentes = BLL.IdiomaService.GetInstance()
                    .ListarTraducciones(nuevoIdioma.Id);

                foreach (DataGridViewRow row in dataGridViewTraducciones.Rows)
                {
                    if (row.IsNewRow) continue;

                    string texto = row.Cells["Traduccion"].Value?.ToString()?.Trim();
                    if (string.IsNullOrEmpty(texto)) continue;

                    string clave = row.Cells["Clave"].Value.ToString();

                    bool existe = existentes.Any(t => t.Clave == clave);

                    if (!existe)
                    {
                        traducciones.Add(new Traduccion
                        {
                            IdiomaId = nuevoIdioma.Id,
                            Clave = clave,
                            Texto = texto
                        });
                    }
                    else
                    {
                        var traduccion = existentes.First(t => t.Clave == clave);
                        traduccion.Texto = texto;

                        traducciones.Add(traduccion);
                    }
                }

                BLL.IdiomaService.GetInstance().Guardar(traducciones);

                MessageBox.Show(
                    $"Idioma '{nuevoIdioma.Nombre}' agregado con " +
                    $"{traducciones.Count} traducciones.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LimpiarFormulario();
                CambiarIdioma(nuevoIdioma);
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

            LimpiarResaltado();
            int vacias = 0;
            foreach (DataGridViewRow row in dataGridViewTraducciones.Rows)
            {
                if (row.IsNewRow) continue;
                string texto = row.Cells["Traduccion"].Value?.ToString()?.Trim();
                if (string.IsNullOrEmpty(texto))
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                    vacias++;
                }
            }

            if (vacias > 0)
            {
                errorManager.ManejarError(
                    $"Faltan {vacias} traducciones. Complete los campos resaltados en rojo antes de guardar.",
                    EnumError.Advertencia);
                return false;
            }

            return true;
        }

        private void LimpiarResaltado()
        {

            foreach (DataGridViewRow row in dataGridViewTraducciones.Rows)
                if (!row.IsNewRow)
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.Empty;
        }

        private void LimpiarFormulario()
        {
            LimpiarResaltado();
            textBoxNombre.Clear();
            textBoxCodigo.Clear();

            foreach (DataGridViewRow row in dataGridViewTraducciones.Rows)
                if (!row.IsNewRow)
                    row.Cells["Traduccion"].Value = string.Empty;

            CargarIdiomasExistentes();
            textBoxNombre.Focus();
        }

        private void ErrorManager_OnOcurrioError(object sender, BE.Error e)
        {
            MessageBoxIcon icon;
            switch (e.Tipo)
            {
                case EnumError.Info: icon = MessageBoxIcon.Information; break;
                case EnumError.Advertencia: icon = MessageBoxIcon.Warning; break;
                case EnumError.Error: icon = MessageBoxIcon.Error; break;
                case EnumError.Critico: icon = MessageBoxIcon.Stop; break;
                default: icon = MessageBoxIcon.None; break;
            }
            MessageBox.Show(e.Mensaje, "Notificación", MessageBoxButtons.OK, icon);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();                               
                errorManager.OnOcurrioError -= ErrorManager_OnOcurrioError;
            }
            base.Dispose(disposing);
        }

        private void listBoxIdiomas_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBoxIdiomas.SelectedItem != null)
            {
                BE.Idioma idioma = listBoxIdiomas.SelectedItem as BE.Idioma;

                textBoxCodigo.Text = idioma.Codigo;
                textBoxNombre.Text = idioma.Nombre;

                dataGridViewTraducciones.Rows.Clear();

                var referencia = BLL.IdiomaService.GetInstance()
                                    .ListarTraducciones(IDIOMA_REFERENCIA_ID);

                var traducciones = BLL.IdiomaService.GetInstance()
                                    .ListarTraducciones(idioma.Id);

                var diccionario = traducciones
                    .GroupBy(t => t.Clave)
                    .ToDictionary(g => g.Key, g => g.First().Texto);

                foreach (var t in referencia)
                {
                    string traduccion = diccionario.ContainsKey(t.Clave)
                        ? diccionario[t.Clave]
                        : string.Empty;

                    dataGridViewTraducciones.Rows.Add(
                        t.Clave,
                        t.Texto,
                        traduccion);
                }
            }
        }
    }
}
