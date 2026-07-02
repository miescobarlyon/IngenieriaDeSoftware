using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public sealed class IdiomaService
    {
        private static IdiomaService _instance;
        private static readonly object _padlock = new object();

        private IdiomaService() { }

        public static IdiomaService GetInstance()
        {
            lock (_padlock)
            {
                if (_instance == null)
                    _instance = new IdiomaService();
                return _instance;
            }
        }

        private readonly List<IIdiomaObserver> _observers = new List<IIdiomaObserver>();
        private readonly TraduccionService _tradService   = new TraduccionService();
        private BE.Idioma _idiomaActual;

        public BE.Idioma IdiomaActual => _idiomaActual;
        public event EventHandler<BE.Idioma> OnIdiomaAgregado;

        public void Suscribir(IIdiomaObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            if (_idiomaActual != null)
                observer.CambiarIdioma(_idiomaActual);
        }

        public void Desuscribir(IIdiomaObserver observer)
        {
            _observers.Remove(observer);
        }

        public void CambiarIdioma(int idiomaId)
        {
            _idiomaActual = new DAL.MP_Idioma().TraerPorId(idiomaId);
            if (_idiomaActual == null) return;

            _tradService.CargarCache(idiomaId);

            foreach (var observer in _observers.ToList())
                observer.CambiarIdioma(_idiomaActual);
        }

        public string Traducir(string clave) => _tradService.Traducir(clave);

        public List<BE.Idioma> ListarIdiomas() => new DAL.MP_Idioma().Listar();

        public BE.Idioma Guardar(BE.Idioma i)
        {
            if (i.Id == 0)
            {
                BE.Idioma idioma = new BE.Idioma { Nombre = i.Nombre, Codigo = i.Codigo };
                idioma = new DAL.MP_Idioma().AgregarYObtener(idioma);
                OnIdiomaAgregado?.Invoke(this, idioma);
                return idioma;
            }
            else
            {
                ModificarIdioma(i);
                return i;
            }

        }

        public void ModificarIdioma(BE.Idioma idioma)
        {
            if (idioma == null) throw new ArgumentNullException(nameof(idioma));
            new DAL.MP_Idioma().Modificar(idioma);
            if (_idiomaActual != null && _idiomaActual.Id == idioma.Id)
                _idiomaActual = idioma;
        }

        public void Guardar(List<BE.Traduccion> traducciones)
        {
            if (traducciones == null || traducciones.Count == 0) return;
            var mapper = new DAL.MP_Traduccion();
            foreach (var t in traducciones)
            {
                if (!string.IsNullOrWhiteSpace(t.Texto) && t.Id == 0)
                {
                    mapper.Agregar(t);
                }
                else if (!string.IsNullOrWhiteSpace(t.Texto))
                {
                    mapper.Modificar(t);
                }
            }
            _tradService.InvalidarCache();
        }

        public void ModificarTraducciones(List<BE.Traduccion> traducciones)
        {
            if (traducciones == null || traducciones.Count == 0) return;
            var mapper = new DAL.MP_Traduccion();
            foreach (var t in traducciones)
            {
                if (!string.IsNullOrWhiteSpace(t.Texto))
                    mapper.Modificar(t);
            }
            _tradService.InvalidarCache();
        }

        public List<BE.Traduccion> ListarTraducciones(int idiomaId)
        {
            return new DAL.MP_Traduccion().Listar(idiomaId);
        }
    }
}
