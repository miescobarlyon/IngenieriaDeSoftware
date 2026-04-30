using BLL;
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
    public partial class Form1 : Form
    {
        
        BLL.UsuarioService userservice = new BLL.UsuarioService();
        BLL.ErrorManagerService errorManager = ErrorManagerService.GetInstance();
       
        public Form1()
        {
            InitializeComponent();
            errorManager.OnOcurrioError += ErrorManager_OnOcurrioError;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            textBox2.UseSystemPasswordChar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = textBox1.Text;
            string password = textBox2.Text;

            try
            {
                if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0)
                {
                    errorManager.ManejarError("Por favor, complete todos campos.", BE.EnumError.Advertencia);
                    return;
                }

                bool ok = userservice.Login(user, password);
                
                if (ok == true)
                {
                    var main = new Main();
                    main.StartPosition = FormStartPosition.CenterScreen;
                    main.Show();
                    this.Hide();
                }
        
            }
            catch (Exception ex)
            {
                errorManager.ManejarError(ex, BE.EnumError.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ErrorManager_OnOcurrioError(object sender, BE.Error e)
        {
            MessageBoxIcon icon;

            switch (e.Tipo)
            {
                case BE.EnumError.Info:
                    icon = MessageBoxIcon.Information;
                    break;
                case BE.EnumError.Advertencia:
                    icon = MessageBoxIcon.Warning;
                    break;
                case BE.EnumError.Error:
                    icon = MessageBoxIcon.Error;
                    break;
                case BE.EnumError.Critico:
                    icon = MessageBoxIcon.Stop;
                    break;
                default:
                    icon = MessageBoxIcon.None;
                    break;
            }

            MessageBox.Show(e.Mensaje, "Notificación", MessageBoxButtons.OK, icon);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = true;
            }
            else
            {
                textBox2.UseSystemPasswordChar = false;
            }
        }
    }
}
