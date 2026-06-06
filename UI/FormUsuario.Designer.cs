namespace UI
{
    partial class FormUsuario
    {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.labelNombre = new System.Windows.Forms.Label();
            this.textBoxNombre = new System.Windows.Forms.TextBox();
            this.labelApellido = new System.Windows.Forms.Label();
            this.textBoxApellido = new System.Windows.Forms.TextBox();
            this.labelUsuario = new System.Windows.Forms.Label();
            this.textBoxUsuario = new System.Windows.Forms.TextBox();
            this.labelContrasena = new System.Windows.Forms.Label();
            this.textBoxContrasena = new System.Windows.Forms.TextBox();
            this.labelConfirmar = new System.Windows.Forms.Label();
            this.textBoxConfirmar = new System.Windows.Forms.TextBox();
            this.labelNotaContrasena = new System.Windows.Forms.Label();
            this.buttonGuardar = new System.Windows.Forms.Button();
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // labelNombre
            this.labelNombre.AutoSize = true;
            this.labelNombre.Location = new System.Drawing.Point(12, 32);
            this.labelNombre.Name = "labelNombre";
            this.labelNombre.Size = new System.Drawing.Size(46, 13);
            this.labelNombre.TabIndex = 0;
            this.labelNombre.Text = "Nombre";
            this.labelNombre.Tag = "lbl.nombre";

            // textBoxNombre
            this.textBoxNombre.Location = new System.Drawing.Point(160, 28);
            this.textBoxNombre.Name = "textBoxNombre";
            this.textBoxNombre.Size = new System.Drawing.Size(260, 22);
            this.textBoxNombre.TabIndex = 1;

            // labelApellido
            this.labelApellido.AutoSize = true;
            this.labelApellido.Location = new System.Drawing.Point(12, 72);
            this.labelApellido.Name = "labelApellido";
            this.labelApellido.Size = new System.Drawing.Size(49, 13);
            this.labelApellido.TabIndex = 2;
            this.labelApellido.Text = "Apellido";
            this.labelApellido.Tag = "lbl.apellido";

            // textBoxApellido
            this.textBoxApellido.Location = new System.Drawing.Point(160, 68);
            this.textBoxApellido.Name = "textBoxApellido";
            this.textBoxApellido.Size = new System.Drawing.Size(260, 22);
            this.textBoxApellido.TabIndex = 3;

            // labelUsuario
            this.labelUsuario.AutoSize = true;
            this.labelUsuario.Location = new System.Drawing.Point(12, 112);
            this.labelUsuario.Name = "labelUsuario";
            this.labelUsuario.Size = new System.Drawing.Size(46, 13);
            this.labelUsuario.TabIndex = 4;
            this.labelUsuario.Text = "Usuario";
            this.labelUsuario.Tag = "lbl.usuario";

            // textBoxUsuario
            this.textBoxUsuario.Location = new System.Drawing.Point(160, 108);
            this.textBoxUsuario.Name = "textBoxUsuario";
            this.textBoxUsuario.Size = new System.Drawing.Size(260, 22);
            this.textBoxUsuario.TabIndex = 5;

            // labelContrasena
            this.labelContrasena.AutoSize = true;
            this.labelContrasena.Location = new System.Drawing.Point(12, 152);
            this.labelContrasena.Name = "labelContrasena";
            this.labelContrasena.Size = new System.Drawing.Size(62, 13);
            this.labelContrasena.TabIndex = 6;
            this.labelContrasena.Text = "Contraseña";
            this.labelContrasena.Tag = "lbl.contrasena";

            // textBoxContrasena
            this.textBoxContrasena.Location = new System.Drawing.Point(160, 148);
            this.textBoxContrasena.Name = "textBoxContrasena";
            this.textBoxContrasena.Size = new System.Drawing.Size(260, 22);
            this.textBoxContrasena.TabIndex = 7;
            this.textBoxContrasena.UseSystemPasswordChar = true;

            // labelConfirmar
            this.labelConfirmar.AutoSize = true;
            this.labelConfirmar.Location = new System.Drawing.Point(12, 192);
            this.labelConfirmar.Name = "labelConfirmar";
            this.labelConfirmar.Size = new System.Drawing.Size(110, 13);
            this.labelConfirmar.TabIndex = 8;
            this.labelConfirmar.Text = "Confirmar contraseña";
            this.labelConfirmar.Tag = "lbl.confirmarContrasena";

            // textBoxConfirmar
            this.textBoxConfirmar.Location = new System.Drawing.Point(160, 188);
            this.textBoxConfirmar.Name = "textBoxConfirmar";
            this.textBoxConfirmar.Size = new System.Drawing.Size(260, 22);
            this.textBoxConfirmar.TabIndex = 9;
            this.textBoxConfirmar.UseSystemPasswordChar = true;

            // labelNotaContrasena  (only visible in edit mode — set in FormUsuario_Load)
            this.labelNotaContrasena.AutoSize = true;
            this.labelNotaContrasena.ForeColor = System.Drawing.Color.Gray;
            this.labelNotaContrasena.Location = new System.Drawing.Point(160, 214);
            this.labelNotaContrasena.Name = "labelNotaContrasena";
            this.labelNotaContrasena.Size = new System.Drawing.Size(260, 13);
            this.labelNotaContrasena.TabIndex = 10;
            this.labelNotaContrasena.Text = "Dejar vacío para conservar la contraseña actual";
            this.labelNotaContrasena.Tag = "lbl.notaContrasena";
            this.labelNotaContrasena.Visible = false;

            // buttonGuardar
            this.buttonGuardar.Location = new System.Drawing.Point(258, 260);
            this.buttonGuardar.Name = "buttonGuardar";
            this.buttonGuardar.Size = new System.Drawing.Size(96, 28);
            this.buttonGuardar.TabIndex = 11;
            this.buttonGuardar.Text = "Guardar";
            this.buttonGuardar.Tag = "btn.guardar";
            this.buttonGuardar.UseVisualStyleBackColor = true;
            this.buttonGuardar.Click += new System.EventHandler(this.buttonGuardar_Click);

            // buttonCancelar
            this.buttonCancelar.Location = new System.Drawing.Point(364, 260);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(96, 28);
            this.buttonCancelar.TabIndex = 12;
            this.buttonCancelar.Text = "Cancelar";
            this.buttonCancelar.Tag = "btn.cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);

            // FormUsuario (form)
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 310);
            this.Controls.Add(this.labelNotaContrasena);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.buttonGuardar);
            this.Controls.Add(this.textBoxConfirmar);
            this.Controls.Add(this.labelConfirmar);
            this.Controls.Add(this.textBoxContrasena);
            this.Controls.Add(this.labelContrasena);
            this.Controls.Add(this.textBoxUsuario);
            this.Controls.Add(this.labelUsuario);
            this.Controls.Add(this.textBoxApellido);
            this.Controls.Add(this.labelApellido);
            this.Controls.Add(this.textBoxNombre);
            this.Controls.Add(this.labelNombre);
            this.Name = "FormUsuario";
            this.Text = "Usuario";
            this.Load += new System.EventHandler(this.FormUsuario_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelNombre;
        private System.Windows.Forms.TextBox textBoxNombre;
        private System.Windows.Forms.Label labelApellido;
        private System.Windows.Forms.TextBox textBoxApellido;
        private System.Windows.Forms.Label labelUsuario;
        private System.Windows.Forms.TextBox textBoxUsuario;
        private System.Windows.Forms.Label labelContrasena;
        private System.Windows.Forms.TextBox textBoxContrasena;
        private System.Windows.Forms.Label labelConfirmar;
        private System.Windows.Forms.TextBox textBoxConfirmar;
        private System.Windows.Forms.Label labelNotaContrasena;
        private System.Windows.Forms.Button buttonGuardar;
        private System.Windows.Forms.Button buttonCancelar;
    }
}