using System;
using System.Windows.Forms;

namespace UI
{
    public class TranslatableForm : Form, BLL.IIdiomaObserver
    {
        private readonly BLL.IdiomaService _service = BLL.IdiomaService.GetInstance();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _service.Suscribir(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _service.Desuscribir(this);
            base.Dispose(disposing);
        }

        public virtual void CambiarNombre(BE.Idioma idioma)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => CambiarNombre(idioma)));
                return;
            }

            if (Tag is string formKey && !string.IsNullOrEmpty(formKey))
                Text = _service.Traducir(formKey);

            AplicarTraducciones(Controls);
        }

        private void AplicarTraducciones(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control.Tag is string clave && !string.IsNullOrEmpty(clave))
                    control.Text = _service.Traducir(clave);

                if (control is ToolStrip toolStrip)
                    AplicarTraduccionesToolStrip(toolStrip.Items);

                if (control.HasChildren)
                    AplicarTraducciones(control.Controls);
            }
        }

        private void AplicarTraduccionesToolStrip(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                if (item.Tag is string clave && !string.IsNullOrEmpty(clave))
                    item.Text = _service.Traducir(clave);

                if (item is ToolStripMenuItem menuItem && menuItem.HasDropDownItems)
                    AplicarTraduccionesToolStrip(menuItem.DropDownItems);
            }
        }
    }
}
