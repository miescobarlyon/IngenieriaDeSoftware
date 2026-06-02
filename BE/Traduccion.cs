namespace BE
{
    public class Traduccion
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string clave;

        public string Clave
        {
            get { return clave; }
            set { clave = value; }
        }

        private int idiomaId;
        public int IdiomaId
        {
            get { return idiomaId; }
            set { idiomaId = value; }
        }

        private string texto;
        public string Texto
        {
            get { return texto; }
            set { texto = value; }
        }
    }
}