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
       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = textBox1.Text;
            string password = textBox2.Text;

            try
            {
                bool ok = userservice.Login(user, password);
                if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0)
                {
                    throw new Exception("Los campos no pueden estar vacíos!");
                }
                if (ok != true)
                {
                   throw new Exception("Los campos son incorrectos!");
                }
                
              
                var main = new Main();
                main.StartPosition = FormStartPosition.CenterScreen;
                main.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
