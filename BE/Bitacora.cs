using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Bitacora
    {
		private int id;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		private Usuario usuario;

		public Usuario Usuario
		{
			get { return usuario; }
			set { usuario = value; }
		}

		private DateTime fecha;

		public DateTime Fecha
		{
			get { return fecha; }
			set { fecha = value; }
		}

		private string actividad;

		public string Actividad
		{
			get { return actividad; }
			set { actividad = value; }
		}

		private EnumCriticidad criticidad;

		public EnumCriticidad Criticidad
		{
			get { return criticidad; }
			set { criticidad = value; }
		}

	}
}
