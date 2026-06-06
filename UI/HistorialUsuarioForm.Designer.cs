namespace UI
{
    partial class HistorialUsuarioForm
    {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.labelEncabezado = new System.Windows.Forms.Label();
            this.dataGridViewHistorial = new System.Windows.Forms.DataGridView();
            this.buttonRevertir = new System.Windows.Forms.Button();
            this.buttonVolver = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistorial)).BeginInit();
            this.SuspendLayout();

            // labelEncabezado
            this.labelEncabezado.AutoSize = true;
            this.labelEncabezado.Font = new System.Drawing.Font(
                "Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.labelEncabezado.Location = new System.Drawing.Point(12, 12);
            this.labelEncabezado.Name = "labelEncabezado";
            this.labelEncabezado.Size = new System.Drawing.Size(200, 17);
            this.labelEncabezado.TabIndex = 0;
            this.labelEncabezado.Text = "Historial de:";

            // dataGridViewHistorial
            this.dataGridViewHistorial.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHistorial.Location = new System.Drawing.Point(12, 38);
            this.dataGridViewHistorial.Name = "dataGridViewHistorial";
            this.dataGridViewHistorial.Size = new System.Drawing.Size(934, 416);
            this.dataGridViewHistorial.TabIndex = 1;

            // buttonRevertir
            this.buttonRevertir.Location = new System.Drawing.Point(634, 464);
            this.buttonRevertir.Name = "buttonRevertir";
            this.buttonRevertir.Size = new System.Drawing.Size(200, 28);
            this.buttonRevertir.TabIndex = 2;
            this.buttonRevertir.Text = "Revertir";
            this.buttonRevertir.Tag = "btn.revertir";
            this.buttonRevertir.UseVisualStyleBackColor = true;
            this.buttonRevertir.Click += new System.EventHandler(this.buttonRevertir_Click);

            // buttonVolver
            this.buttonVolver.Location = new System.Drawing.Point(844, 464);
            this.buttonVolver.Name = "buttonVolver";
            this.buttonVolver.Size = new System.Drawing.Size(100, 28);
            this.buttonVolver.TabIndex = 3;
            this.buttonVolver.Text = "Volver";
            this.buttonVolver.Tag = "btn.volver";
            this.buttonVolver.UseVisualStyleBackColor = true;
            this.buttonVolver.Click += new System.EventHandler(this.buttonVolver_Click);

            // HistorialUsuarioForm (form)
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 504);
            this.Controls.Add(this.buttonVolver);
            this.Controls.Add(this.buttonRevertir);
            this.Controls.Add(this.dataGridViewHistorial);
            this.Controls.Add(this.labelEncabezado);
            this.Name = "HistorialUsuarioForm";
            this.Text = "Historial de Cambios";
            this.Tag = "titulo.historial";
            this.Load += new System.EventHandler(this.HistorialUsuarioForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistorial)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelEncabezado;
        private System.Windows.Forms.DataGridView dataGridViewHistorial;
        private System.Windows.Forms.Button buttonRevertir;
        private System.Windows.Forms.Button buttonVolver;
    }
}