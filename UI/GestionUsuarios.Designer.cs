namespace UI
{
    partial class GestionUsuarios
    {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.buttonAgregar = new System.Windows.Forms.Button();
            this.buttonEditar = new System.Windows.Forms.Button();
            this.buttonBloquear = new System.Windows.Forms.Button();
            this.buttonVerHistorial = new System.Windows.Forms.Button();
            this.dataGridViewUsuarios = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsuarios)).BeginInit();
            this.SuspendLayout();

            // buttonAgregar
            this.buttonAgregar.Location = new System.Drawing.Point(12, 12);
            this.buttonAgregar.Name = "buttonAgregar";
            this.buttonAgregar.Size = new System.Drawing.Size(96, 28);
            this.buttonAgregar.TabIndex = 0;
            this.buttonAgregar.Text = "Agregar";
            this.buttonAgregar.Tag = "btn.agregar";
            this.buttonAgregar.UseVisualStyleBackColor = true;
            this.buttonAgregar.Click += new System.EventHandler(this.buttonAgregar_Click);

            // buttonEditar
            this.buttonEditar.Enabled = false;
            this.buttonEditar.Location = new System.Drawing.Point(118, 12);
            this.buttonEditar.Name = "buttonEditar";
            this.buttonEditar.Size = new System.Drawing.Size(96, 28);
            this.buttonEditar.TabIndex = 1;
            this.buttonEditar.Text = "Editar";
            this.buttonEditar.Tag = "btn.editar";
            this.buttonEditar.UseVisualStyleBackColor = true;
            this.buttonEditar.Click += new System.EventHandler(this.buttonEditar_Click);

            // buttonBloquear
            this.buttonBloquear.Enabled = false;
            this.buttonBloquear.Location = new System.Drawing.Point(224, 12);
            this.buttonBloquear.Name = "buttonBloquear";
            this.buttonBloquear.Size = new System.Drawing.Size(120, 28);
            this.buttonBloquear.TabIndex = 2;
            this.buttonBloquear.Text = "Bloquear";
            this.buttonBloquear.Tag = "btn.bloquear";
            this.buttonBloquear.UseVisualStyleBackColor = true;
            this.buttonBloquear.Click += new System.EventHandler(this.buttonBloquear_Click);

            // buttonVerHistorial
            this.buttonVerHistorial.Enabled = false;
            this.buttonVerHistorial.Location = new System.Drawing.Point(354, 12);
            this.buttonVerHistorial.Name = "buttonVerHistorial";
            this.buttonVerHistorial.Size = new System.Drawing.Size(130, 28);
            this.buttonVerHistorial.TabIndex = 3;
            this.buttonVerHistorial.Text = "Ver historial";
            this.buttonVerHistorial.Tag = "btn.verHistorial";
            this.buttonVerHistorial.UseVisualStyleBackColor = true;
            this.buttonVerHistorial.Click += new System.EventHandler(this.buttonVerHistorial_Click);

            // dataGridViewUsuarios
            this.dataGridViewUsuarios.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUsuarios.Location = new System.Drawing.Point(12, 50);
            this.dataGridViewUsuarios.Name = "dataGridViewUsuarios";
            this.dataGridViewUsuarios.Size = new System.Drawing.Size(934, 454);
            this.dataGridViewUsuarios.TabIndex = 4;
            this.dataGridViewUsuarios.SelectionChanged +=
                new System.EventHandler(this.dataGridViewUsuarios_SelectionChanged);

            // GestionUsuarios (form)
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 516);
            this.Controls.Add(this.dataGridViewUsuarios);
            this.Controls.Add(this.buttonVerHistorial);
            this.Controls.Add(this.buttonBloquear);
            this.Controls.Add(this.buttonEditar);
            this.Controls.Add(this.buttonAgregar);
            this.Name = "GestionUsuarios";
            this.Text = "Gestión de Usuarios";
            this.Tag = "titulo.gestionUsuarios";
            this.Load += new System.EventHandler(this.GestionUsuarios_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsuarios)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button buttonAgregar;
        private System.Windows.Forms.Button buttonEditar;
        private System.Windows.Forms.Button buttonBloquear;
        private System.Windows.Forms.Button buttonVerHistorial;
        private System.Windows.Forms.DataGridView dataGridViewUsuarios;
    }
}