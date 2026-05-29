using BLL;
using ENTITY;

namespace GUI.ViewModels;

public class PerfilViewModel : ViewModelBase
{
    private readonly Usuario _usuario;
    private readonly UsuarioServicio _usuarioSvc = new();

    private string _nombreCompleto = "";
    private string _username = "";
    private string _correo = "";
    private string _saldoTexto = "";
    private string _identificacion = "";
    private string _fechaRegistro = "";

    public string NombreCompleto { get => _nombreCompleto; set => SetProperty(ref _nombreCompleto, value); }
    public string Username { get => _username; set => SetProperty(ref _username, value); }
    public string Correo { get => _correo; set => SetProperty(ref _correo, value); }
    public string SaldoTexto { get => _saldoTexto; set => SetProperty(ref _saldoTexto, value); }
    public string Identificacion { get => _identificacion; set => SetProperty(ref _identificacion, value); }
    public string FechaRegistro { get => _fechaRegistro; set => SetProperty(ref _fechaRegistro, value); }

    public PerfilViewModel(Usuario usuario)
    {
        _usuario = usuario;
        try
        {
            Usuario? u = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario) ?? usuario;
            NombreCompleto = $"{u.Nombre1} {u.Nombre2 ?? ""} {u.Apellido1} {u.Apellido2 ?? ""}".Trim();
            Username = u.Username ?? "";
            Correo = u.Correo ?? "";
            Identificacion = u.IdUsuario.ToString();
            SaldoTexto = $"${u.Saldo:N2}";
            FechaRegistro = u.FechaRegistro != default ? u.FechaRegistro.ToString("dd/MM/yyyy") : "N/A";
        }
        catch
        {
            NombreCompleto = $"{usuario.Nombre1} {usuario.Apellido1}";
        }
    }
}
