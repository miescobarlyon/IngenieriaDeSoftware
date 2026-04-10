using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Usuario
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        private string apellido;

        public string Apellido
        {
            get { return apellido; }
            set { apellido = value; }
        }
        private string user;

        public string User
        {
            get { return user; }
            set { user = value; }
        }
        private string passwordHash;

        public string PasswordHash
        {
            get { return passwordHash; }
            set { passwordHash = value; }
        }

        public byte Borrado { get; set; }
        
    }
}
