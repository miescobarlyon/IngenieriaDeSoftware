﻿using BLL;
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
        BLL.SessionManager sm = SessionManager.GetInstance();
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sm.Logout();

            var log = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (log == null)
            {
                log = new Form1();
            }

            log.Show();
            log.BringToFront();
            this.Close();
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
