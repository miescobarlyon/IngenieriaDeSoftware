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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void buttonLogs_Click(object sender, EventArgs e)
        {
            Bitacora logs = new Bitacora();

            logs.StartPosition = FormStartPosition.CenterScreen;
            logs.Show();
            this.Hide();
        }
    }
}
