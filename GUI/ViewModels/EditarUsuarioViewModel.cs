using System.Security.Cryptography;
using System.Text;
using System.Windows;
using BLL;
using ENTITY;
using GUI.Commands;

namespace GUI.ViewModels;

public class EditarUsuarioViewModel : ViewModelBase
{
    private readonly Usuario _usuarioOriginal;
    private readonly UsuarioServicio _usuarioSvc = new();

    private string _username = "";
    private string _password = "";
    private string _nombre1 = "";
    private string _nombre2 = "";
    private string _apellido1 = "";
    private string _apellido2 = "";
    private string _correo = "";
    private DateTime _fechaNacimiento;
    private string _estado = "";
    private string _idValor = "";
    private string _errorMessage = "";
    private bool _resultadoOk;

    public string Username { get => _username; set => SetProperty(ref _username, value); }
    public string Password { get => _password; set => SetProperty(ref _password, value); }
    public string Nombre1 { get => _nombre1; set => SetProperty(ref _nombre1, value); }
    public string Nombre2 { get => _nombre2; set => SetProperty(ref _nombre2, value); }
    public string Apellido1 { get => _apellido1; set => SetProperty(ref _apellido1, value); }
    public string Apellido2 { get => _apellido2; set => SetProperty(ref _apellido2, value); }
    public string Correo { get => _correo; set => SetProperty(ref _correo, value); }
    public DateTime FechaNacimiento { get => _fechaNacimiento; set => SetProperty(ref _fechaNacimiento, value); }
    public string Estado { get => _estado; set => SetProperty(ref _estado, value); }
    public string IdValor { get => _idValor; set => SetProperty(ref _idValor, value); }
    public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }
    public bool ResultadoOk { get => _resultadoOk; set => SetProperty(ref _resultadoOk, value); }

    public string[] Estados { get; } = { "activo", "suspendido", "inactivo" };

    public RelayCommand GuardarCommand { get; }
    public RelayCommand CancelarCommand { get; }

    public event EventHandler? DialogClosed;

    public EditarUsuarioViewModel(Usuario usuario)
    {
        _usuarioOriginal = usuario;
        IdValor = usuario.IdUsuario.ToString();
        Username = usuario.Username ?? "";
        Nombre1 = usuario.Nombre1 ?? "";
        Nombre2 = usuario.Nombre2 ?? "";
        Apellido1 = usuario.Apellido1 ?? "";
        Apellido2 = usuario.Apellido2 ?? "";
        Correo = usuario.Correo ?? "";
        FechaNacimiento = usuario.FechaNacimiento != default ? usuario.FechaNacimiento : DateTime.Today;
        Estado = usuario.Estado ?? "activo";

        GuardarCommand = new RelayCommand(_ => Guardar());
        CancelarCommand = new RelayCommand(_ =>
        {
            ResultadoOk = false;
            DialogClosed?.Invoke(this, EventArgs.Empty);
        });
    }

    private void Guardar()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            ErrorMessage = "El username es obligatorio.";
            return;
        }
        if (string.IsNullOrWhiteSpace(Nombre1))
        {
            ErrorMessage = "El primer nombre es obligatorio.";
            return;
        }
        if (string.IsNullOrWhiteSpace(Apellido1))
        {
            ErrorMessage = "El primer apellido es obligatorio.";
            return;
        }
        if (string.IsNullOrWhiteSpace(Correo))
        {
            ErrorMessage = "El correo es obligatorio.";
            return;
        }

        int edad = DateTime.Today.Year - FechaNacimiento.Year;
        if (FechaNacimiento.Date > DateTime.Today.AddYears(-edad)) edad--;
        if (edad < 18)
        {
            ErrorMessage = "El usuario debe ser mayor de edad (18+).";
            return;
        }

        _usuarioOriginal.Username = Username.Trim();
        if (!string.IsNullOrWhiteSpace(Password))
            _usuarioOriginal.Password = HashPassword(Password);
        _usuarioOriginal.Nombre1 = Nombre1.Trim();
        _usuarioOriginal.Nombre2 = string.IsNullOrWhiteSpace(Nombre2) ? null : Nombre2.Trim();
        _usuarioOriginal.Apellido1 = Apellido1.Trim();
        _usuarioOriginal.Apellido2 = string.IsNullOrWhiteSpace(Apellido2) ? null : Apellido2.Trim();
        _usuarioOriginal.Correo = Correo.Trim();
        _usuarioOriginal.FechaNacimiento = FechaNacimiento;
        _usuarioOriginal.Estado = Estado;

        string resultado = _usuarioSvc.Actualizar(_usuarioOriginal);

        if (resultado == "Guardado correctamente.")
        {
            ResultadoOk = true;
            DialogClosed?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            ErrorMessage = resultado;
        }
    }

    private static string HashPassword(string password)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
