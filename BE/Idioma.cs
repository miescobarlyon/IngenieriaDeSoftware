using System;

namespace BE
{
    public class Idioma
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

        private string codigo;

        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public override string ToString()
        {
            return nombre;
        }
    }
}