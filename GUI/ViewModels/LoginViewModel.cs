using System.Windows;
using BLL;
using ENTITY;
using GUI.Commands;
using GUI.Views;

namespace GUI.ViewModels;

public enum ModoLogin
{
    Login,
    Registro
}

public class LoginViewModel : ViewModelBase
{
    private readonly UsuarioServicio _servicio = new();
    private readonly LogServicio _logSvc = new();

    private ModoLogin _modo = ModoLogin.Login;
    private string _username = "";
    private string _password = "";
    private string _errorMessage = "";
    private string _identificacion = "";
    private string _nombre1 = "";
    private string _nombre2 = "";
    private string _apellido1 = "";
    private string _apellido2 = "";
    private string _correo = "";
    private DateTime _fechaNacimiento = DateTime.Today.AddYears(-25);

    public ModoLogin Modo
    {
        get => _modo;
        set
        {
            SetProperty(ref _modo, value);
            OnPropertyChanged(nameof(IsLogin));
            OnPropertyChanged(nameof(IsRegistro));
            OnPropertyChanged(nameof(Titulo));
            OnPropertyChanged(nameof(Subtitulo));
            OnPropertyChanged(nameof(BotonPrincipalTexto));
            OnPropertyChanged(nameof(BotonSecundarioTexto));
        }
    }

    public string Username { get => _username; set => SetProperty(ref _username, value); }
    public string Password { get => _password; set => SetProperty(ref _password, value); }
    public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }
    public string Identificacion { get => _identificacion; set => SetProperty(ref _identificacion, value); }
    public string Nombre1 { get => _nombre1; set => SetProperty(ref _nombre1, value); }
    public string Nombre2 { get => _nombre2; set => SetProperty(ref _nombre2, value); }
    public string Apellido1 { get => _apellido1; set => SetProperty(ref _apellido1, value); }
    public string Apellido2 { get => _apellido2; set => SetProperty(ref _apellido2, value); }
    public string Correo { get => _correo; set => SetProperty(ref _correo, value); }
    public DateTime FechaNacimiento { get => _fechaNacimiento; set => SetProperty(ref _fechaNacimiento, value); }

    public bool IsLogin => Modo == ModoLogin.Login;
    public bool IsRegistro => Modo == ModoLogin.Registro;
    public string Titulo => IsLogin ? "Iniciar sesión" : "Crear cuenta";
    public string Subtitulo => IsLogin ? "Ingresa con tus credenciales para continuar." : "Completa tus datos para activar tu acceso a Casino Royal.";
    public string BotonPrincipalTexto => IsLogin ? "Ingresar" : "Registrar cuenta";
    public string BotonSecundarioTexto => IsLogin ? "Crear cuenta" : "Volver";

    public RelayCommand LoginCommand { get; }
    public RelayCommand CambiarModoCommand { get; }

    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(_ => EjecutarAccionPrincipal());
        CambiarModoCommand = new RelayCommand(_ =>
        {
            Modo = IsLogin ? ModoLogin.Registro : ModoLogin.Login;
            ErrorMessage = "";
        });
    }

    private void EjecutarAccionPrincipal()
    {
        if (IsLogin)
            IniciarSesion();
        else
            RegistrarUsuario();
    }

    private void IniciarSesion()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Complete todos los campos.";
            return;
        }

        Usuario? u = _servicio.Login(Username.Trim(), Password);
        if (u == null)
        {
            ErrorMessage = "Usuario o contraseña incorrectos.";
            return;
        }

        Window? ventanaActual = ObtenerVentanaActual();
        if (u.IdRol == 1)
        {
            AdminWindow adminWin = new AdminWindow(u);
            adminWin.Show();
        }
        else
        {
            MainWindow mainWin = new MainWindow(u);
            mainWin.Show();
        }

        ventanaActual?.Close();
    }

    private void RegistrarUsuario()
    {
        if (string.IsNullOrWhiteSpace(Identificacion) ||
            string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Password) ||
            string.IsNullOrWhiteSpace(Nombre1) ||
            string.IsNullOrWhiteSpace(Apellido1) ||
            string.IsNullOrWhiteSpace(Correo))
        {
            ErrorMessage = "Por favor, complete todos los campos obligatorios.";
            return;
        }

        if (!int.TryParse(Identificacion.Trim(), out int idUsuario))
        {
            ErrorMessage = "La identificación debe ser numérica.";
            return;
        }

        Usuario usuario = new Usuario
        {
            IdUsuario = idUsuario,
            Username = Username.Trim(),
            Password = Password,
            Nombre1 = Nombre1.Trim(),
            Nombre2 = string.IsNullOrWhiteSpace(Nombre2) ? null : Nombre2.Trim(),
            Apellido1 = Apellido1.Trim(),
            Apellido2 = string.IsNullOrWhiteSpace(Apellido2) ? null : Apellido2.Trim(),
            Correo = Correo.Trim(),
            FechaNacimiento = FechaNacimiento,
            Saldo = 0,
            IdRol = 2
        };

        if (!usuario.EsMayorDeEdad())
        {
            ErrorMessage = "Debes ser mayor de edad para registrarte.";
            return;
        }

        string resultado = _servicio.Registrar(usuario);
        bool ok = resultado == "Guardado correctamente.";

        if (ok)
        {
            MessageBox.Show("Usuario registrado correctamente.", "Éxito",
                MessageBoxButton.OK, MessageBoxImage.Information);
            LimpiarRegistro();
            Modo = ModoLogin.Login;
        }
        else
        {
            ErrorMessage = resultado;
        }
    }

    private void LimpiarRegistro()
    {
        Identificacion = "";
        Username = "";
        Password = "";
        Nombre1 = "";
        Nombre2 = "";
        Apellido1 = "";
        Apellido2 = "";
        Correo = "";
        FechaNacimiento = DateTime.Today.AddYears(-25);
        ErrorMessage = "";
    }

    private static Window? ObtenerVentanaActual()
    {
        foreach (Window w in Application.Current.Windows)
            if (w.IsActive) return w;
        return Application.Current.MainWindow;
    }
}
