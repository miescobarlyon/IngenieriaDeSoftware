namespace UI
{
    partial class GestionRoles
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBoxRoles = new System.Windows.Forms.ListBox();
            this.txtCodigoRol = new System.Windows.Forms.TextBox();
            this.txtNombreRol = new System.Windows.Forms.TextBox();
            this.btnCrearRol = new System.Windows.Forms.Button();
            this.btnEliminarRol = new System.Windows.Forms.Button();
            this.clbPermisos = new System.Windows.Forms.CheckedListBox();
            this.btnGuardarPermisos = new System.Windows.Forms.Button();
            this.cboUsuarios = new System.Windows.Forms.ComboBox();
            this.btnAsignarRol = new System.Windows.Forms.Button();
            this.btnQuitarRol = new System.Windows.Forms.Button();
            this.lblUsuarios = new System.Windows.Forms.Label();
            this.lblRoles = new System.Windows.Forms.Label();
            this.lblPermisos = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxRoles
            // 
            this.listBoxRoles.FormattingEnabled = true;
            this.listBoxRoles.Location = new System.Drawing.Point(12, 41);
            this.listBoxRoles.Name = "listBoxRoles";
            this.listBoxRoles.Size = new System.Drawing.Size(282, 121);
            this.listBoxRoles.TabIndex = 0;
            this.listBoxRoles.SelectedIndexChanged += new System.EventHandler(this.listBoxRoles_SelectedIndexChanged_1);
            // 
            // txtCodigoRol
            // 
            this.txtCodigoRol.Location = new System.Drawing.Point(330, 29);
            this.txtCodigoRol.Name = "txtCodigoRol";
            this.txtCodigoRol.Size = new System.Drawing.Size(100, 20);
            this.txtCodigoRol.TabIndex = 1;
            // 
            // txtNombreRol
            // 
            this.txtNombreRol.Location = new System.Drawing.Point(330, 71);
            this.txtNombreRol.Name = "txtNombreRol";
            this.txtNombreRol.Size = new System.Drawing.Size(100, 20);
            this.txtNombreRol.TabIndex = 2;
            // 
            // btnCrearRol
            // 
            this.btnCrearRol.Location = new System.Drawing.Point(321, 116);
            this.btnCrearRol.Name = "btnCrearRol";
            this.btnCrearRol.Size = new System.Drawing.Size(75, 23);
            this.btnCrearRol.TabIndex = 3;
            this.btnCrearRol.Tag = "btn.crearRol";
            this.btnCrearRol.Text = "button1";
            this.btnCrearRol.UseVisualStyleBackColor = true;
            this.btnCrearRol.Click += new System.EventHandler(this.btnCrearRol_Click);
            // 
            // btnEliminarRol
            // 
            this.btnEliminarRol.Location = new System.Drawing.Point(402, 116);
            this.btnEliminarRol.Name = "btnEliminarRol";
            this.btnEliminarRol.Size = new System.Drawing.Size(75, 23);
            this.btnEliminarRol.TabIndex = 4;
            this.btnEliminarRol.Tag = "btn.eliminarRol";
            this.btnEliminarRol.Text = "button2";
            this.btnEliminarRol.UseVisualStyleBackColor = true;
            this.btnEliminarRol.Click += new System.EventHandler(this.btnEliminarRol_Click);
            // 
            // clbPermisos
            // 
            this.clbPermisos.FormattingEnabled = true;
            this.clbPermisos.Location = new System.Drawing.Point(12, 219);
            this.clbPermisos.Name = "clbPermisos";
            this.clbPermisos.Size = new System.Drawing.Size(267, 184);
            this.clbPermisos.TabIndex = 5;
            // 
            // btnGuardarPermisos
            // 
            this.btnGuardarPermisos.Location = new System.Drawing.Point(321, 294);
            this.btnGuardarPermisos.Name = "btnGuardarPermisos";
            this.btnGuardarPermisos.Size = new System.Drawing.Size(75, 23);
            this.btnGuardarPermisos.TabIndex = 6;
            this.btnGuardarPermisos.Tag = "btn.guardarPermisos";
            this.btnGuardarPermisos.Text = "button1";
            this.btnGuardarPermisos.UseVisualStyleBackColor = true;
            this.btnGuardarPermisos.Click += new System.EventHandler(this.btnGuardarPermisos_Click);
            // 
            // cboUsuarios
            // 
            this.cboUsuarios.FormattingEnabled = true;
            this.cboUsuarios.Location = new System.Drawing.Point(569, 296);
            this.cboUsuarios.Name = "cboUsuarios";
            this.cboUsuarios.Size = new System.Drawing.Size(121, 21);
            this.cboUsuarios.TabIndex = 7;
            // 
            // btnAsignarRol
            // 
            this.btnAsignarRol.Location = new System.Drawing.Point(557, 336);
            this.btnAsignarRol.Name = "btnAsignarRol";
            this.btnAsignarRol.Size = new System.Drawing.Size(75, 23);
            this.btnAsignarRol.TabIndex = 8;
            this.btnAsignarRol.Tag = "btn.asignarRol";
            this.btnAsignarRol.Text = "button1";
            this.btnAsignarRol.UseVisualStyleBackColor = true;
            this.btnAsignarRol.Click += new System.EventHandler(this.btnAsignarRol_Click);
            // 
            // btnQuitarRol
            // 
            this.btnQuitarRol.Location = new System.Drawing.Point(638, 336);
            this.btnQuitarRol.Name = "btnQuitarRol";
            this.btnQuitarRol.Size = new System.Drawing.Size(75, 23);
            this.btnQuitarRol.TabIndex = 9;
            this.btnQuitarRol.Tag = "btn.quitarRol";
            this.btnQuitarRol.Text = "button1";
            this.btnQuitarRol.UseVisualStyleBackColor = true;
            this.btnQuitarRol.Click += new System.EventHandler(this.btnQuitarRol_Click);
            // 
            // lblUsuarios
            // 
            this.lblUsuarios.AutoSize = true;
            this.lblUsuarios.Location = new System.Drawing.Point(610, 258);
            this.lblUsuarios.Name = "lblUsuarios";
            this.lblUsuarios.Size = new System.Drawing.Size(35, 13);
            this.lblUsuarios.TabIndex = 10;
            this.lblUsuarios.Tag = "lbl.asignarUsuario";
            this.lblUsuarios.Text = "label1";
            // 
            // lblRoles
            // 
            this.lblRoles.AutoSize = true;
            this.lblRoles.Location = new System.Drawing.Point(27, 9);
            this.lblRoles.Name = "lblRoles";
            this.lblRoles.Size = new System.Drawing.Size(35, 13);
            this.lblRoles.TabIndex = 11;
            this.lblRoles.Tag = "lbl.roles";
            this.lblRoles.Text = "label1";
            // 
            // lblPermisos
            // 
            this.lblPermisos.AutoSize = true;
            this.lblPermisos.Location = new System.Drawing.Point(12, 193);
            this.lblPermisos.Name = "lblPermisos";
            this.lblPermisos.Size = new System.Drawing.Size(35, 13);
            this.lblPermisos.TabIndex = 12;
            this.lblPermisos.Tag = "lbl.permisos";
            this.lblPermisos.Text = "label1";
            // 
            // GestionRoles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblPermisos);
            this.Controls.Add(this.lblRoles);
            this.Controls.Add(this.lblUsuarios);
            this.Controls.Add(this.btnQuitarRol);
            this.Controls.Add(this.btnAsignarRol);
            this.Controls.Add(this.cboUsuarios);
            this.Controls.Add(this.btnGuardarPermisos);
            this.Controls.Add(this.clbPermisos);
            this.Controls.Add(this.btnEliminarRol);
            this.Controls.Add(this.btnCrearRol);
            this.Controls.Add(this.txtNombreRol);
            this.Controls.Add(this.txtCodigoRol);
            this.Controls.Add(this.listBoxRoles);
            this.Name = "GestionRoles";
            this.Tag = "titulo.gestionRoles";
            this.Text = "GestionRoles";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxRoles;
        private System.Windows.Forms.TextBox txtCodigoRol;
        private System.Windows.Forms.TextBox txtNombreRol;
        private System.Windows.Forms.Button btnCrearRol;
        private System.Windows.Forms.Button btnEliminarRol;
        private System.Windows.Forms.CheckedListBox clbPermisos;
        private System.Windows.Forms.Button btnGuardarPermisos;
        private System.Windows.Forms.ComboBox cboUsuarios;
        private System.Windows.Forms.Button btnAsignarRol;
        private System.Windows.Forms.Button btnQuitarRol;
        private System.Windows.Forms.Label lblUsuarios;
        private System.Windows.Forms.Label lblRoles;
        private System.Windows.Forms.Label lblPermisos;
    }
}