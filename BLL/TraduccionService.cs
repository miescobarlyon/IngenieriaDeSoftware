using System.Collections.Generic;
using System.Linq;

namespace BLL
{

    public class TraduccionService
    {
        private Dictionary<string, string> _cache = new Dictionary<string, string>();
        private int _cachedIdiomaId = -1;

        public void CargarCache(int idiomaId)
        {
            if (_cachedIdiomaId == idiomaId) return;

            var lista = new DAL.MP_Traduccion().Listar(idiomaId);
            _cache = lista.ToDictionary(t => t.Clave, t => t.Texto);
            _cachedIdiomaId = idiomaId;
        }

        public string Traducir(string clave)
        {
            if (string.IsNullOrEmpty(clave)) return clave;
            return _cache.TryGetValue(clave, out var texto) ? texto : clave;
        }

        public void InvalidarCache() => _cachedIdiomaId = -1;
    }
}
