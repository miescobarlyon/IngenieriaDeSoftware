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
            // 
            // labelNombre
            // 
            this.labelNombre.AutoSize = true;
            this.labelNombre.Location = new System.Drawing.Point(18, 34);
            this.labelNombre.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNombre.Name = "labelNombre";
            this.labelNombre.Size = new System.Drawing.Size(65, 20);
            this.labelNombre.TabIndex = 0;
            this.labelNombre.Tag = "lbl.nombreIdioma";
            this.labelNombre.Text = "Nombre";
            // 
            // textBoxNombre
            // 
            this.textBoxNombre.Location = new System.Drawing.Point(99, 28);
            this.textBoxNombre.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxNombre.Name = "textBoxNombre";
            this.textBoxNombre.Size = new System.Drawing.Size(298, 26);
            this.textBoxNombre.TabIndex = 1;
            // 
            // labelCodigo
            // 
            this.labelCodigo.AutoSize = true;
            this.labelCodigo.Location = new System.Drawing.Point(423, 34);
            this.labelCodigo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCodigo.Name = "labelCodigo";
            this.labelCodigo.Size = new System.Drawing.Size(59, 20);
            this.labelCodigo.TabIndex = 2;
            this.labelCodigo.Tag = "lbl.codigoIdioma";
            this.labelCodigo.Text = "Código";
            // 
            // textBoxCodigo
            // 
            this.textBoxCodigo.Location = new System.Drawing.Point(501, 28);
            this.textBoxCodigo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxCodigo.Name = "textBoxCodigo";
            this.textBoxCodigo.Size = new System.Drawing.Size(148, 26);
            this.textBoxCodigo.TabIndex = 3;
            // 
            // labelIdiomasExistentes
            // 
            this.labelIdiomasExistentes.AutoSize = true;
            this.labelIdiomasExistentes.Location = new System.Drawing.Point(705, 15);
            this.labelIdiomasExistentes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIdiomasExistentes.Name = "labelIdiomasExistentes";
            this.labelIdiomasExistentes.Size = new System.Drawing.Size(141, 20);
            this.labelIdiomasExistentes.TabIndex = 4;
            this.labelIdiomasExistentes.Tag = "lbl.idiomasExistentes";
            this.labelIdiomasExistentes.Text = "Idiomas existentes";
            // 
            // listBoxIdiomas
            // 
            this.listBoxIdiomas.FormattingEnabled = true;
            this.listBoxIdiomas.ItemHeight = 20;
            this.listBoxIdiomas.Location = new System.Drawing.Point(705, 43);
            this.listBoxIdiomas.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBoxIdiomas.Name = "listBoxIdiomas";
            this.listBoxIdiomas.Size = new System.Drawing.Size(697, 124);
            this.listBoxIdiomas.TabIndex = 5;
            this.listBoxIdiomas.SelectedValueChanged += new System.EventHandler(this.listBoxIdiomas_SelectedValueChanged);
            // 
            // labelTraducciones
            // 
            this.labelTraducciones.AutoSize = true;
            this.labelTraducciones.Location = new System.Drawing.Point(18, 203);
            this.labelTraducciones.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTraducciones.Name = "labelTraducciones";
            this.labelTraducciones.Size = new System.Drawing.Size(104, 20);
            this.labelTraducciones.TabIndex = 6;
            this.labelTraducciones.Tag = "lbl.traducciones";
            this.labelTraducciones.Text = "Traducciones";
            // 
            // dataGridViewTraducciones
            // 
            this.dataGridViewTraducciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTraducciones.Location = new System.Drawing.Point(18, 234);
            this.dataGridViewTraducciones.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridViewTraducciones.Name = "dataGridViewTraducciones";
            this.dataGridViewTraducciones.RowHeadersWidth = 62;
            this.dataGridViewTraducciones.Size = new System.Drawing.Size(1401, 477);
            this.dataGridViewTraducciones.TabIndex = 7;
            // 
            // buttonGuardar
            // 
            this.buttonGuardar.Location = new System.Drawing.Point(1119, 742);
            this.buttonGuardar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonGuardar.Name = "buttonGuardar";
            this.buttonGuardar.Size = new System.Drawing.Size(144, 43);
            this.buttonGuardar.TabIndex = 8;
            this.buttonGuardar.Tag = "btn.guardar";
            this.buttonGuardar.Text = "Guardar";
            this.buttonGuardar.UseVisualStyleBackColor = true;
            this.buttonGuardar.Click += new System.EventHandler(this.buttonGuardar_Click);
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.Location = new System.Drawing.Point(1278, 742);
            this.buttonCancelar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(144, 43);
            this.buttonCancelar.TabIndex = 9;
            this.buttonCancelar.Tag = "btn.cancelar";
            this.buttonCancelar.Text = "Cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);
            // 
            // AgregarIdioma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1440, 806);
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
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AgregarIdioma";
            this.Tag = "titulo.agregarIdioma";
            this.Text = "Agregar Idioma";
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