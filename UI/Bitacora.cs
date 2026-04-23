using BE;
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
    public partial class Bitacora : Form
    {
        public Bitacora()
        {
            InitializeComponent();
            ConfigurarControles();
            ActualizarRegistros();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Bitacora_Load(object sender, EventArgs e)
        {

        }

        private void ConfigurarControles()
        {
            dateTimePickerHasta.Value = DateTime.Now;
            dateTimePickerHasta.MaxDate = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);

            dateTimePickerDesde.MaxDate = DateTime.Today.AddHours(0);
            dateTimePickerDesde.Value = DateTime.Now.AddDays(-7);

            listBoxUsuarios.Items.Clear();
            listBoxUsuarios.DataSource = BLL.UsuarioService.Listar();
            listBoxUsuarios.DisplayMember = "User";

            listBoxActividad.Items.Clear();

            listBoxCriticidad.Items.Clear();
            listBoxCriticidad.DataSource = Enum.GetValues(typeof(EnumCriticidad)).Cast<EnumCriticidad>().ToList();
        }

        private void ActualizarRegistros()
        {
            List<BE.Usuario> usuarios = ObtenerUsuarios();
            DateTime inicio = dateTimePickerDesde.Value;
            DateTime fin = dateTimePickerHasta.Value;
            List<string> actividades = ObtenerActividades();
            List<BE.EnumCriticidad> criticidad = ObtenerCriticidad();

            dataGridViewRegistros.DataSource = null;
            dataGridViewRegistros.DataSource = BLL.BitacoraService.Listar(usuarios, inicio, fin, actividades, criticidad);

        }

        private List<BE.Usuario> ObtenerUsuarios()
        {
            List<BE.Usuario> usuarios = listBoxUsuarios.SelectedItems
                .Cast<BE.Usuario>()
                .ToList();

            return usuarios;
        }

        private List<string> ObtenerActividades()
        {
            List<string> actividades = listBoxActividad.SelectedItems
                .Cast<string>()
                .ToList();

            return actividades;
        }

        private List<BE.EnumCriticidad> ObtenerCriticidad()
        {
            List<BE.EnumCriticidad> criticidad = listBoxCriticidad.SelectedItems
                .Cast<BE.EnumCriticidad>()
                .ToList();

            return criticidad;
        }

        private bool VerificarFechas()
        {
            return dateTimePickerDesde.Value <= dateTimePickerHasta.Value;
        }

        private void listBoxUsuarios_SelectedValueChanged(object sender, EventArgs e)
        {
            ActualizarRegistros();
        }

        private void dateTimePickerDesde_ValueChanged(object sender, EventArgs e)
        {
            if (VerificarFechas())
            {
                if (dateTimePickerDesde.Value.Date == dateTimePickerHasta.Value.Date)
                {
                    dateTimePickerDesde.Value = dateTimePickerDesde.Value.Date.AddHours(0);
                    dateTimePickerHasta.Value = dateTimePickerHasta.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                }

                ActualizarRegistros();
            }
            else
            {
                MessageBox.Show("Ingrese un rango de fechas válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void dateTimePickerHasta_ValueChanged(object sender, EventArgs e)
        {
            if (VerificarFechas())
            {
                if (dateTimePickerDesde.Value.Date == dateTimePickerHasta.Value.Date)
                {
                    dateTimePickerDesde.Value = dateTimePickerDesde.Value.Date.AddHours(0);
                    dateTimePickerHasta.Value = dateTimePickerHasta.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                }

                ActualizarRegistros();
            }
            else
            {
                MessageBox.Show("Ingrese un rango de fechas válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBoxActividad_SelectedValueChanged(object sender, EventArgs e)
        {
            ActualizarRegistros();
        }

        private void listBoxCriticidad_SelectedValueChanged(object sender, EventArgs e)
        {
            ActualizarRegistros();
        }
    }
}
