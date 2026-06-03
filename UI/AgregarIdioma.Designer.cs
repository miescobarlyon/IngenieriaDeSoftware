using System.Runtime.CompilerServices;

namespace UI
{
    partial class AgregarIdioma
    {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.labelNombre = new System.Windows.Forms.Label();
            this.textBoxNombre = new System.Windows.Forms.TextBox();
            this.labelCodigo = new System.Windows.Forms.Label();
            this.textBoxCodigo = new System.Windows.Forms.TextBox();
            this.labelIdiomasExistentes = new System.Windows.Forms.Label();
            this.listBoxIdiomas = new System.Windows.Forms.ListBox();
            this.labelTraducciones = new System.Windows.Forms.Label();
            this.dataGridViewTraducciones = new System.Windows.Forms.DataGridView();
            this.buttonGuardar = new System.Windows.Forms.Button();
            this.buttonCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTraducciones)).BeginInit();
            this.SuspendLayout();

            // ── labelNombre ──────────────────────────────────────────────────
            this.labelNombre.AutoSize = true;
            this.labelNombre.Location = new System.Drawing.Point(12, 22);
            this.labelNombre.Name = "labelNombre";
            this.labelNombre.Size = new System.Drawing.Size(46, 13);
            this.labelNombre.TabIndex = 0;
            this.labelNombre.Text = "Nombre";
            this.labelNombre.Tag = "lbl.nombreIdioma";

            // ── textBoxNombre ────────────────────────────────────────────────
            this.textBoxNombre.Location = new System.Drawing.Point(66, 18);
            this.textBoxNombre.Name = "textBoxNombre";
            this.textBoxNombre.Size = new System.Drawing.Size(200, 22);
            this.textBoxNombre.TabIndex = 1;

            // ── labelCodigo ──────────────────────────────────────────────────
            this.labelCodigo.AutoSize = true;
            this.labelCodigo.Location = new System.Drawing.Point(282, 22);
            this.labelCodigo.Name = "labelCodigo";
            this.labelCodigo.Size = new System.Drawing.Size(44, 13);
            this.labelCodigo.TabIndex = 2;
            this.labelCodigo.Text = "Código";
            this.labelCodigo.Tag = "lbl.codigoIdioma";

            // ── textBoxCodigo ────────────────────────────────────────────────
            // Intentionally narrow — codes like "es-AR", "en", "pt-BR" are short.
            this.textBoxCodigo.Location = new System.Drawing.Point(334, 18);
            this.textBoxCodigo.Name = "textBoxCodigo";
            this.textBoxCodigo.Size = new System.Drawing.Size(100, 22);
            this.textBoxCodigo.TabIndex = 3;

            // ── labelIdiomasExistentes ───────────────────────────────────────
            this.labelIdiomasExistentes.AutoSize = true;
            this.labelIdiomasExistentes.Location = new System.Drawing.Point(470, 10);
            this.labelIdiomasExistentes.Name = "labelIdiomasExistentes";
            this.labelIdiomasExistentes.Size = new System.Drawing.Size(110, 13);
            this.labelIdiomasExistentes.TabIndex = 4;
            this.labelIdiomasExistentes.Text = "Idiomas existentes";
            this.labelIdiomasExistentes.Tag = "lbl.idiomasExistentes";

            // ── listBoxIdiomas ───────────────────────────────────────────────
            // Read-only display — SelectionMode.None prevents user interaction.
            this.listBoxIdiomas.FormattingEnabled = true;
            this.listBoxIdiomas.Location = new System.Drawing.Point(470, 28);
            this.listBoxIdiomas.Name = "listBoxIdiomas";
            this.listBoxIdiomas.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBoxIdiomas.Size = new System.Drawing.Size(466, 82);
            this.listBoxIdiomas.TabIndex = 5;

            // ── labelTraducciones ────────────────────────────────────────────
            this.labelTraducciones.AutoSize = true;
            this.labelTraducciones.Location = new System.Drawing.Point(12, 132);
            this.labelTraducciones.Name = "labelTraducciones";
            this.labelTraducciones.Size = new System.Drawing.Size(74, 13);
            this.labelTraducciones.TabIndex = 6;
            this.labelTraducciones.Text = "Traducciones";
            this.labelTraducciones.Tag = "lbl.traducciones";

            // ── dataGridViewTraducciones ─────────────────────────────────────
            // Columns are added in ConfigurarGrilla() — not here.
            this.dataGridViewTraducciones.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTraducciones.Location = new System.Drawing.Point(12, 152);
            this.dataGridViewTraducciones.Name = "dataGridViewTraducciones";
            this.dataGridViewTraducciones.Size = new System.Drawing.Size(934, 310);
            this.dataGridViewTraducciones.TabIndex = 7;

            // ── buttonGuardar ────────────────────────────────────────────────
            this.buttonGuardar.Location = new System.Drawing.Point(746, 482);
            this.buttonGuardar.Name = "buttonGuardar";
            this.buttonGuardar.Size = new System.Drawing.Size(96, 28);
            this.buttonGuardar.TabIndex = 8;
            this.buttonGuardar.Text = "Guardar";
            this.buttonGuardar.Tag = "btn.guardar";
            this.buttonGuardar.UseVisualStyleBackColor = true;
            this.buttonGuardar.Click += new System.EventHandler(this.buttonGuardar_Click);

            // ── buttonCancelar ───────────────────────────────────────────────
            this.buttonCancelar.Location = new System.Drawing.Point(852, 482);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(96, 28);
            this.buttonCancelar.TabIndex = 9;
            this.buttonCancelar.Text = "Cancelar";
            this.buttonCancelar.Tag = "btn.cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);

            // ── AgregarIdioma (form) ─────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 524);
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
            this.Tag = "titulo.agregarIdioma";
            this.Load += new System.EventHandler(this.AgregarIdioma_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTraducciones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelNombre;
        private System.Windows.Forms.TextBox textBoxNombre;
        private System.Windows.Forms.Label labelCodigo;
        private System.Windows.Forms.TextBox textBoxCodigo;
        private System.Windows.Forms.Label labelIdiomasExistentes;
        private System.Windows.Forms.ListBox listBoxIdiomas;
        private System.Windows.Forms.Label labelTraducciones;
        private System.Windows.Forms.DataGridView dataGridViewTraducciones;
        private System.Windows.Forms.Button buttonGuardar;
        private System.Windows.Forms.Button buttonCancelar;
    }
}