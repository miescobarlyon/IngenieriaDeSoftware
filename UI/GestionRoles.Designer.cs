namespace UI
{
    partial class GestionRoles
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.treeRoles = new System.Windows.Forms.TreeView();
            this.lstDisponibles = new System.Windows.Forms.ListBox();
            this.txtCodigoRol = new System.Windows.Forms.TextBox();
            this.txtNombreRol = new System.Windows.Forms.TextBox();
            this.btnCrearRol = new System.Windows.Forms.Button();
            this.btnEliminarRol = new System.Windows.Forms.Button();
            this.btnAgregarHijo = new System.Windows.Forms.Button();
            this.btnQuitarHijo = new System.Windows.Forms.Button();
            this.cboUsuarios = new System.Windows.Forms.ComboBox();
            this.btnAsignarRol = new System.Windows.Forms.Button();
            this.btnQuitarRol = new System.Windows.Forms.Button();
            this.lblUsuarios = new System.Windows.Forms.Label();
            this.lblRoles = new System.Windows.Forms.Label();
            this.lblPermisos = new System.Windows.Forms.Label();
            this.lblCodigoRol = new System.Windows.Forms.Label();
            this.lblNombreRol = new System.Windows.Forms.Label();
            this.treeUsuario = new System.Windows.Forms.TreeView();
            this.lblPermisosUsuario = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // treeRoles
            // 
            this.treeRoles.Location = new System.Drawing.Point(15, 30);
            this.treeRoles.Name = "treeRoles";
            this.treeRoles.Size = new System.Drawing.Size(420, 480);
            this.treeRoles.TabIndex = 0;
            this.treeRoles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeRoles_AfterSelect);
            // 
            // lblRoles
            // 
            this.lblRoles.AutoSize = true;
            this.lblRoles.Location = new System.Drawing.Point(15, 9);
            this.lblRoles.Name = "lblRoles";
            this.lblRoles.Size = new System.Drawing.Size(35, 13);
            this.lblRoles.TabIndex = 1;
            this.lblRoles.Tag = "lbl.roles";
            this.lblRoles.Text = "Roles";
            // 
            // lblCodigoRol
            // 
            this.lblCodigoRol.AutoSize = true;
            this.lblCodigoRol.Location = new System.Drawing.Point(465, 33);
            this.lblCodigoRol.Name = "lblCodigoRol";
            this.lblCodigoRol.Size = new System.Drawing.Size(35, 13);
            this.lblCodigoRol.TabIndex = 2;
            this.lblCodigoRol.Tag = "lbl.codigoRol";
            this.lblCodigoRol.Text = "Código";
            // 
            // txtCodigoRol
            // 
            this.txtCodigoRol.Location = new System.Drawing.Point(560, 30);
            this.txtCodigoRol.Name = "txtCodigoRol";
            this.txtCodigoRol.Size = new System.Drawing.Size(160, 20);
            this.txtCodigoRol.TabIndex = 3;
            // 
            // lblNombreRol
            // 
            this.lblNombreRol.AutoSize = true;
            this.lblNombreRol.Location = new System.Drawing.Point(465, 62);
            this.lblNombreRol.Name = "lblNombreRol";
            this.lblNombreRol.Size = new System.Drawing.Size(35, 13);
            this.lblNombreRol.TabIndex = 4;
            this.lblNombreRol.Tag = "lbl.nombreRol";
            this.lblNombreRol.Text = "Nombre";
            // 
            // txtNombreRol
            // 
            this.txtNombreRol.Location = new System.Drawing.Point(560, 59);
            this.txtNombreRol.Name = "txtNombreRol";
            this.txtNombreRol.Size = new System.Drawing.Size(160, 20);
            this.txtNombreRol.TabIndex = 5;
            // 
            // btnCrearRol
            // 
            this.btnCrearRol.Location = new System.Drawing.Point(560, 90);
            this.btnCrearRol.Name = "btnCrearRol";
            this.btnCrearRol.Size = new System.Drawing.Size(75, 23);
            this.btnCrearRol.TabIndex = 6;
            this.btnCrearRol.Tag = "btn.crearRol";
            this.btnCrearRol.Text = "Crear";
            this.btnCrearRol.UseVisualStyleBackColor = true;
            this.btnCrearRol.Click += new System.EventHandler(this.btnCrearRol_Click);
            // 
            // btnEliminarRol
            // 
            this.btnEliminarRol.Location = new System.Drawing.Point(645, 90);
            this.btnEliminarRol.Name = "btnEliminarRol";
            this.btnEliminarRol.Size = new System.Drawing.Size(75, 23);
            this.btnEliminarRol.TabIndex = 7;
            this.btnEliminarRol.Tag = "btn.eliminarRol";
            this.btnEliminarRol.Text = "Eliminar";
            this.btnEliminarRol.UseVisualStyleBackColor = true;
            this.btnEliminarRol.Click += new System.EventHandler(this.btnEliminarRol_Click);
            // 
            // lblPermisos
            // 
            this.lblPermisos.AutoSize = true;
            this.lblPermisos.Location = new System.Drawing.Point(465, 140);
            this.lblPermisos.Name = "lblPermisos";
            this.lblPermisos.Size = new System.Drawing.Size(35, 13);
            this.lblPermisos.TabIndex = 8;
            this.lblPermisos.Tag = "lbl.permisos";
            this.lblPermisos.Text = "Disponibles";
            // 
            // lstDisponibles
            // 
            this.lstDisponibles.FormattingEnabled = true;
            this.lstDisponibles.Location = new System.Drawing.Point(465, 160);
            this.lstDisponibles.Name = "lstDisponibles";
            this.lstDisponibles.Size = new System.Drawing.Size(255, 199);
            this.lstDisponibles.TabIndex = 9;
            // 
            // btnAgregarHijo
            // 
            this.btnAgregarHijo.Location = new System.Drawing.Point(465, 365);
            this.btnAgregarHijo.Name = "btnAgregarHijo";
            this.btnAgregarHijo.Size = new System.Drawing.Size(125, 23);
            this.btnAgregarHijo.TabIndex = 10;
            this.btnAgregarHijo.Tag = "btn.agregarHijo";
            this.btnAgregarHijo.Text = "Agregar hijo";
            this.btnAgregarHijo.UseVisualStyleBackColor = true;
            this.btnAgregarHijo.Click += new System.EventHandler(this.btnAgregarHijo_Click);
            // 
            // btnQuitarHijo
            // 
            this.btnQuitarHijo.Location = new System.Drawing.Point(595, 365);
            this.btnQuitarHijo.Name = "btnQuitarHijo";
            this.btnQuitarHijo.Size = new System.Drawing.Size(125, 23);
            this.btnQuitarHijo.TabIndex = 11;
            this.btnQuitarHijo.Tag = "btn.quitarHijo";
            this.btnQuitarHijo.Text = "Quitar hijo";
            this.btnQuitarHijo.UseVisualStyleBackColor = true;
            this.btnQuitarHijo.Click += new System.EventHandler(this.btnQuitarHijo_Click);
            // 
            // lblUsuarios
            // 
            this.lblUsuarios.AutoSize = true;
            this.lblUsuarios.Location = new System.Drawing.Point(465, 420);
            this.lblUsuarios.Name = "lblUsuarios";
            this.lblUsuarios.Size = new System.Drawing.Size(35, 13);
            this.lblUsuarios.TabIndex = 12;
            this.lblUsuarios.Tag = "lbl.asignarUsuario";
            this.lblUsuarios.Text = "Asignar a usuario";
            // 
            // cboUsuarios
            // 
            this.cboUsuarios.FormattingEnabled = true;
            this.cboUsuarios.Location = new System.Drawing.Point(465, 440);
            this.cboUsuarios.Name = "cboUsuarios";
            this.cboUsuarios.Size = new System.Drawing.Size(255, 21);
            this.cboUsuarios.TabIndex = 13;
            // 
            // btnAsignarRol
            // 
            this.btnAsignarRol.Location = new System.Drawing.Point(465, 470);
            this.btnAsignarRol.Name = "btnAsignarRol";
            this.btnAsignarRol.Size = new System.Drawing.Size(125, 23);
            this.btnAsignarRol.TabIndex = 14;
            this.btnAsignarRol.Tag = "btn.asignarRol";
            this.btnAsignarRol.Text = "Asignar";
            this.btnAsignarRol.UseVisualStyleBackColor = true;
            this.btnAsignarRol.Click += new System.EventHandler(this.btnAsignarRol_Click);
            // 
            // btnQuitarRol
            // 
            this.btnQuitarRol.Location = new System.Drawing.Point(595, 470);
            this.btnQuitarRol.Name = "btnQuitarRol";
            this.btnQuitarRol.Size = new System.Drawing.Size(125, 23);
            this.btnQuitarRol.TabIndex = 15;
            this.btnQuitarRol.Tag = "btn.quitarRol";
            this.btnQuitarRol.Text = "Quitar";
            this.btnQuitarRol.UseVisualStyleBackColor = true;
            this.btnQuitarRol.Click += new System.EventHandler(this.btnQuitarRol_Click);
            // 
            // lblPermisosUsuario
            // 
            this.lblPermisosUsuario.AutoSize = true;
            this.lblPermisosUsuario.Location = new System.Drawing.Point(745, 9);
            this.lblPermisosUsuario.Name = "lblPermisosUsuario";
            this.lblPermisosUsuario.Size = new System.Drawing.Size(35, 13);
            this.lblPermisosUsuario.TabIndex = 16;
            this.lblPermisosUsuario.Text = "Permisos del usuario seleccionado";
            // 
            // treeUsuario
            // 
            this.treeUsuario.Location = new System.Drawing.Point(745, 30);
            this.treeUsuario.Name = "treeUsuario";
            this.treeUsuario.Size = new System.Drawing.Size(250, 480);
            this.treeUsuario.TabIndex = 17;
            // 
            // GestionRoles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 530);
            this.Controls.Add(this.lblPermisosUsuario);
            this.Controls.Add(this.treeUsuario);
            this.Controls.Add(this.btnQuitarRol);
            this.Controls.Add(this.btnAsignarRol);
            this.Controls.Add(this.cboUsuarios);
            this.Controls.Add(this.lblUsuarios);
            this.Controls.Add(this.btnQuitarHijo);
            this.Controls.Add(this.btnAgregarHijo);
            this.Controls.Add(this.lstDisponibles);
            this.Controls.Add(this.lblPermisos);
            this.Controls.Add(this.btnEliminarRol);
            this.Controls.Add(this.btnCrearRol);
            this.Controls.Add(this.txtNombreRol);
            this.Controls.Add(this.lblNombreRol);
            this.Controls.Add(this.txtCodigoRol);
            this.Controls.Add(this.lblCodigoRol);
            this.Controls.Add(this.lblRoles);
            this.Controls.Add(this.treeRoles);
            this.Name = "GestionRoles";
            this.Tag = "titulo.gestionRoles";
            this.Text = "GestionRoles";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TreeView treeRoles;
        private System.Windows.Forms.ListBox lstDisponibles;
        private System.Windows.Forms.TextBox txtCodigoRol;
        private System.Windows.Forms.TextBox txtNombreRol;
        private System.Windows.Forms.Button btnCrearRol;
        private System.Windows.Forms.Button btnEliminarRol;
        private System.Windows.Forms.Button btnAgregarHijo;
        private System.Windows.Forms.Button btnQuitarHijo;
        private System.Windows.Forms.ComboBox cboUsuarios;
        private System.Windows.Forms.Button btnAsignarRol;
        private System.Windows.Forms.Button btnQuitarRol;
        private System.Windows.Forms.Label lblUsuarios;
        private System.Windows.Forms.Label lblRoles;
        private System.Windows.Forms.Label lblPermisos;
        private System.Windows.Forms.Label lblCodigoRol;
        private System.Windows.Forms.Label lblNombreRol;
        private System.Windows.Forms.TreeView treeUsuario;
        private System.Windows.Forms.Label lblPermisosUsuario;
    }
}