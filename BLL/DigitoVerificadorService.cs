using BE;
using BLL;
using DAL;
using System.Text;

public class DigitoVerificadorService
{
    private readonly MP_Usuario mpUsuario = new MP_Usuario();
    private readonly MP_DigitoVerificador mpDV = new MP_DigitoVerificador();
    private readonly SecurityService hashService = new SecurityService();

    private string ConcatenarUsuario(Usuario u)
    {
        return $"{u.Id}|{u.Nombre}|{u.Apellido}|{u.User}|{u.PasswordHash}|{u.Salt}|{u.Borrado}|{u.Bloqueado}";
    }

    public void Recalcular()
    {
        var usuarios = mpUsuario.Listar();
        var concatenacion = new StringBuilder();

        foreach (var u in usuarios)
        {
            string dvh = hashService.Hash(ConcatenarUsuario(u));
            mpDV.ActualizarDVHUsuario(u.Id, dvh);
            concatenacion.Append(dvh);
        }

        string dvv = hashService.Hash(concatenacion.ToString());
        mpDV.ActualizarDVV("USUARIO", dvv);
        mpDV.ActualizarBackup();
    }
    public void Restaurar()
    {
        mpDV.RestaurarDesdeBackup();
        Recalcular();
    }

    public ResultadoVerificacion Verificar()
    {
        var resultado = new ResultadoVerificacion();
        string dvvGuardado = mpDV.TraerDVV("USUARIO");

        if (string.IsNullOrEmpty(dvvGuardado))
        {
            Recalcular();
            resultado.EsValido = true;
            return resultado;
        }

        var usuarios = mpUsuario.Listar();
        var concatenacion = new StringBuilder();


        foreach (var u in usuarios)
        {
            string concat = ConcatenarUsuario(u);
            string dvhEsperado = hashService.Hash(concat);

            if (dvhEsperado != u.Dvh)
                resultado.IdsCorruptos.Add(u.Id);

            concatenacion.Append(dvhEsperado);
        }
        string dvvCalculado = hashService.Hash(concatenacion.ToString());
      
        resultado.DvvCorrupto = dvvCalculado != dvvGuardado;
        resultado.EsValido = resultado.IdsCorruptos.Count == 0 && !resultado.DvvCorrupto;
        return resultado;
    }
}