namespace UI
{
    partial class RecuperacionDB
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
            this.labelTitulo = new System.Windows.Forms.Label();
            this.labelEstado = new System.Windows.Forms.Label();
            this.buttonRestaurar = new System.Windows.Forms.Button();
            this.textBoxDetalle = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelTitulo
            // 
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Location = new System.Drawing.Point(446, 18);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(35, 13);
            this.labelTitulo.TabIndex = 0;
            this.labelTitulo.Tag = "titulo.recuperacionDB";
            this.labelTitulo.Text = "label1";
            // 
            // labelEstado
            // 
            this.labelEstado.AutoSize = true;
            this.labelEstado.Location = new System.Drawing.Point(446, 112);
            this.labelEstado.Name = "labelEstado";
            this.labelEstado.Size = new System.Drawing.Size(35, 13);
            this.labelEstado.TabIndex = 1;
            this.labelEstado.Text = "label2";
            // 
            // buttonRestaurar
            // 
            this.buttonRestaurar.Location = new System.Drawing.Point(433, 491);
            this.buttonRestaurar.Name = "buttonRestaurar";
            this.buttonRestaurar.Size = new System.Drawing.Size(139, 58);
            this.buttonRestaurar.TabIndex = 2;
            this.buttonRestaurar.Tag = "btn.restaurar";
            this.buttonRestaurar.Text = "button1";
            this.buttonRestaurar.UseVisualStyleBackColor = true;
            this.buttonRestaurar.Click += new System.EventHandler(this.buttonRestaurar_Click_1);
            // 
            // textBoxDetalle
            // 
            this.textBoxDetalle.Location = new System.Drawing.Point(121, 169);
            this.textBoxDetalle.Multiline = true;
            this.textBoxDetalle.Name = "textBoxDetalle";
            this.textBoxDetalle.ReadOnly = true;
            this.textBoxDetalle.Size = new System.Drawing.Size(752, 287);
            this.textBoxDetalle.TabIndex = 3;
            // 
            // RecuperacionDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 578);
            this.Controls.Add(this.textBoxDetalle);
            this.Controls.Add(this.buttonRestaurar);
            this.Controls.Add(this.labelEstado);
            this.Controls.Add(this.labelTitulo);
            this.Name = "RecuperacionDB";
            this.Tag = "titulo.recuperacionDB";
            this.Text = "Recuperacion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.Label labelEstado;
        private System.Windows.Forms.Button buttonRestaurar;
        private System.Windows.Forms.TextBox textBoxDetalle;
    }
}