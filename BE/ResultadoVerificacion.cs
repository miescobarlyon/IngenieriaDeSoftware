using System.Collections.Generic;

namespace BE
{
    public class ResultadoVerificacion
    {
        public bool EsValido { get; set; }
        public List<int> IdsCorruptos { get; set; } = new List<int>();
        public bool DvvCorrupto { get; set; }
    }
}