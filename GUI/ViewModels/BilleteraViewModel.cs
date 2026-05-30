using System.Collections.ObjectModel;
using BLL;
using ENTITY;
using GUI.Commands;

namespace GUI.ViewModels;

public class BilleteraViewModel : ViewModelBase
{
    private readonly Usuario _usuario;
    private readonly MainViewModel _mainVM;
    private readonly GestionFinancieraServicio _svc = new();
    private readonly UsuarioServicio _usuarioSvc = new();
    private readonly WompiServicio _wompiSvc = new();

    private string _saldoTexto = "";
    private decimal _montoDeposito;
    private decimal _montoRetiro;
    private string _statusDeposito = "";
    private string _statusRetiro = "";
    private bool _isLoadingBancos;
    private ObservableCollection<BancoWompi> _bancos = new();
    private BancoWompi? _bancoSeleccionado;
    private DatosBancarios? _datosActuales;
    private ObservableCollection<DatosBancarios> _datosGuardados = new();
    private bool _mostrarRegistroDatosBancarios;
    private string _tipoDocSeleccionado = "CC";
    private string _tipoCuentaSeleccionada = "CORRIENTE";
    private string _numeroDoc = "";
    private string _nombreTitular = "";
    private string _numeroCuenta = "";
    private string _statusRegistro = "";
    private int _depositoPendienteId;
    private string? _referenciaPendiente;
    private bool _hayDepositoPendiente;
    private bool _confirmacionEnProceso;

    public string SaldoTexto { get => _saldoTexto; set => SetProperty(ref _saldoTexto, value); }
    public decimal MontoDeposito { get => _montoDeposito; set => SetProperty(ref _montoDeposito, value); }
    public decimal MontoRetiro { get => _montoRetiro; set => SetProperty(ref _montoRetiro, value); }
    public string StatusDeposito { get => _statusDeposito; set => SetProperty(ref _statusDeposito, value); }
    public string StatusRetiro { get => _statusRetiro; set => SetProperty(ref _statusRetiro, value); }
    public bool IsLoadingBancos { get => _isLoadingBancos; set => SetProperty(ref _isLoadingBancos, value); }
    public ObservableCollection<BancoWompi> Bancos { get => _bancos; set => SetProperty(ref _bancos, value); }
    public BancoWompi? BancoSeleccionado { get => _bancoSeleccionado; set => SetProperty(ref _bancoSeleccionado, value); }
    public DatosBancarios? DatosActuales { get => _datosActuales; set => SetProperty(ref _datosActuales, value); }
    public ObservableCollection<DatosBancarios> DatosGuardados { get => _datosGuardados; set => SetProperty(ref _datosGuardados, value); }
    public bool MostrarRegistroDatosBancarios { get => _mostrarRegistroDatosBancarios; set => SetProperty(ref _mostrarRegistroDatosBancarios, value); }
    public string TipoDocSeleccionado { get => _tipoDocSeleccionado; set => SetProperty(ref _tipoDocSeleccionado, value); }
    public string TipoCuentaSeleccionada { get => _tipoCuentaSeleccionada; set => SetProperty(ref _tipoCuentaSeleccionada, value); }
    public string NumeroDoc { get => _numeroDoc; set => SetProperty(ref _numeroDoc, value); }
    public string NombreTitular { get => _nombreTitular; set => SetProperty(ref _nombreTitular, value); }
    public string NumeroCuenta { get => _numeroCuenta; set => SetProperty(ref _numeroCuenta, value); }
    public string StatusRegistro { get => _statusRegistro; set => SetProperty(ref _statusRegistro, value); }
    public bool HayDepositoPendiente { get => _hayDepositoPendiente; set => SetProperty(ref _hayDepositoPendiente, value); }

    public decimal TotalDepositos { get; private set; }
    public decimal TotalRetiros { get; private set; }
    public int TotalMovimientos { get; private set; }

    public RelayCommand DepositarCommand { get; }
    public RelayCommand RetirarCommand { get; }
    public RelayCommand AbrirRegistroDatosCommand { get; }
    public RelayCommand GuardarDatosCommand { get; }
    public RelayCommand CancelarRegistroCommand { get; }
    public RelayCommand VerificarPagoCommand { get; }

    public event EventHandler? SaldoActualizado;

    public BilleteraViewModel(Usuario usuario, MainViewModel mainVM)
    {
        _usuario = usuario;
        _mainVM = mainVM;

        DepositarCommand = new RelayCommand(_ => Depositar());
        RetirarCommand = new RelayCommand(async _ => await Task.Run(() => RetirarConWompiAsync()));
        AbrirRegistroDatosCommand = new RelayCommand(_ => AbrirRegistroDatos());
        GuardarDatosCommand = new RelayCommand(_ => GuardarDatos());
        CancelarRegistroCommand = new RelayCommand(_ => CancelarRegistro());
        VerificarPagoCommand = new RelayCommand(async _ => await VerificarPagoManualAsync());

        CargarDatos();
    }

    private void CargarDatos()
    {
        try
        {
            Usuario u = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
            if (u != null) _usuario.Saldo = u.Saldo;
            SaldoTexto = $"Saldo: ${_usuario.Saldo:N2}";

            CargarDatosGuardados();
            CargarBancosAsync();
        }
        catch { }
    }

    private void CargarDatosGuardados()
    {
        try
        {
            var datos = _svc.ObtenerDatosBancarios(_usuario.IdUsuario);
            DatosGuardados.Clear();
            foreach (var d in datos)
                DatosGuardados.Add(d);

            var activo = _svc.ObtenerDatosBancariosActivos(_usuario.IdUsuario);
            DatosActuales = activo;
        }
        catch { }
    }

    private async void CargarBancosAsync()
    {
        try
        {
            IsLoadingBancos = true;
            var bancos = await _wompiSvc.ObtenerBancosAsync();
            Bancos = new ObservableCollection<BancoWompi>(bancos.OrderBy(b => b.Nombre));
        }
        catch
        {
            Bancos = new ObservableCollection<BancoWompi>(BancosWompiDefecto.ObtenerListaDefecto().OrderBy(b => b.Nombre));
        }
        finally
        {
            IsLoadingBancos = false;
        }
    }

    private void AbrirRegistroDatos()
    {
        LimpiarFormulario();
        MostrarRegistroDatosBancarios = true;
    }

    private void GuardarDatos()
    {
        if (!ValidarFormulario())
            return;

        try
        {
            StatusRegistro = "Guardando...";
            var datos = new DatosBancarios
            {
                IdUsuario = _usuario.IdUsuario,
                BancoId = BancoSeleccionado?.Id,
                BancoNombre = BancoSeleccionado?.Nombre,
                TipoCuenta = TipoCuentaSeleccionada,
                NumeroCuenta = NumeroCuenta,
                TipoDoc = TipoDocSeleccionado,
                NumeroDoc = NumeroDoc,
                NombreTitular = NombreTitular,
                Activo = true,
                FechaRegistro = DateTime.Now
            };

            string resultado = _svc.GuardarDatosBancarios(datos);
            if (resultado.Contains("correctamente"))
            {
                StatusRegistro = "Datos guardados correctamente.";
                CargarDatosGuardados();
                MostrarRegistroDatosBancarios = false;
                LimpiarFormulario();
            }
            else
            {
                StatusRegistro = $"Error: {resultado}";
            }
        }
        catch (Exception ex)
        {
            StatusRegistro = $"Error: {ex.Message}";
        }
    }

    private void CancelarRegistro()
    {
        MostrarRegistroDatosBancarios = false;
        LimpiarFormulario();
    }

    private bool ValidarFormulario()
    {
        if (BancoSeleccionado == null)
        {
            StatusRegistro = "Selecciona un banco.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(NumeroDoc))
        {
            StatusRegistro = "Ingresa tu número de documento.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(NombreTitular))
        {
            StatusRegistro = "Ingresa el nombre del titular.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(NumeroCuenta))
        {
            StatusRegistro = "Ingresa el número de cuenta.";
            return false;
        }
        if (NumeroCuenta.Length < 10 || NumeroCuenta.Length > 20)
        {
            StatusRegistro = "El número de cuenta debe tener entre 10 y 20 dígitos.";
            return false;
        }

        return true;
    }

    private void LimpiarFormulario()
    {
        NumeroDoc = "";
        NombreTitular = "";
        NumeroCuenta = "";
        TipoDocSeleccionado = "CC";
        TipoCuentaSeleccionada = "CORRIENTE";
        BancoSeleccionado = null;
        StatusRegistro = "";
    }

    private async void Depositar()
    {
        if (MontoDeposito <= 0)
        {
            StatusDeposito = "Ingrese un monto válido.";
            return;
        }

        StatusDeposito = "Creando solicitud...";
        try
        {
            var (msg, idDeposito) = _svc.SolicitarDeposito(_usuario.IdUsuario, MontoDeposito, null, null);
            if (!msg.Contains("correctamente"))
            {
                StatusDeposito = $"Error: {msg}";
                return;
            }

            string referencia = $"DEP-{idDeposito}-{DateTime.Now:yyyyMMddHHmmss}";
            StatusDeposito = "Creando link de pago en Wompi...";

            var (ok, linkUrl, idLink, error) = await _wompiSvc.CrearLinkPagoAsync(MontoDeposito, referencia);

            if (!ok || string.IsNullOrEmpty(linkUrl))
            {
                StatusDeposito = $"Error Wompi: {error}";
                _svc.RechazarDeposito(idDeposito, "Link Wompi no creado");
                return;
            }

            // Guardar referencia en BD para consulta posterior
            _svc.ActualizarReferenciaWompiDeposito(idDeposito, referencia);

            // Guardar datos del depósito pendiente (para verificación manual)
            _depositoPendienteId = idDeposito;
            _referenciaPendiente = referencia;
            HayDepositoPendiente = true;

            // Abrir checkout en el navegador
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = linkUrl,
                UseShellExecute = true
            });
            StatusDeposito = "Esperando pago...";

            // Iniciar polling para verificar el pago
            await PollWompiAsync(referencia, idDeposito);
        }
        catch (System.Exception ex)
        {
            StatusDeposito = $"Error: {ex.Message}";
        }
    }

    private async Task PollWompiAsync(string referencia, int idDeposito)
    {
        int maxIntentos = 45;
        for (int i = 1; i <= maxIntentos; i++)
        {
            await Task.Delay(2000);

            try
            {
                var (ok, transacciones, error) = await _wompiSvc.ConsultarTransaccionesPorReferenciaAsync(referencia);

                if (!ok || transacciones == null || transacciones.Count == 0)
                {
                    StatusDeposito = $"Esperando pago... ({i * 2}s)";
                    continue;
                }

                var data = transacciones[0];
                string estado = data.Status;

                if (estado == "APPROVED")
                {
                    // Guarda crítica: evitar doble confirmación
                    if (_confirmacionEnProceso)
                    {
                        StatusDeposito = "Confirmación ya en proceso, espere...";
                        return;
                    }
                    _confirmacionEnProceso = true;

                    // Guardar datos locales ANTES de limpiar el estado
                    int idDepositoLocal = _depositoPendienteId;
                    
                    // Limpiar estado pendiente INMEDIATAMENTE para evitar re-entrada
                    MontoDeposito = 0;
                    _depositoPendienteId = 0;
                    _referenciaPendiente = null;
                    HayDepositoPendiente = false;

                    try
                    {
                        string resultado = _svc.ConfirmarDeposito(idDepositoLocal, data.Id);
                        if (resultado.Contains("correctamente"))
                        {
                            StatusDeposito = $"Pago confirmado. Transacción #{data.Id}";
                        }
                        else
                        {
                            StatusDeposito = $"Pago recibido pero error al confirmar: {resultado}";
                        }

                        // Refrescar saldo local
                        Usuario u = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
                        if (u != null) _usuario.Saldo = u.Saldo;
                        SaldoTexto = $"Saldo: ${_usuario.Saldo:N2}";
                        _mainVM.ActualizarSaldo();
                        SaldoActualizado?.Invoke(this, EventArgs.Empty);
                    }
                    finally
                    {
                        _confirmacionEnProceso = false;
                    }
                    return;
                }

                if (estado == "DECLINED" || estado == "ERROR")
                {
                    StatusDeposito = "Pago rechazado o cancelado.";
                    _svc.RechazarDeposito(idDeposito, $"Pago {estado} en Wompi");
                    _depositoPendienteId = 0;
                    _referenciaPendiente = null;
                    HayDepositoPendiente = false;
                    return;
                }

                StatusDeposito = $"Esperando pago... ({i * 2}s)";
            }
            catch
            {
                StatusDeposito = $"Verificando pago... ({i * 2}s)";
            }
        }

        StatusDeposito = "Tiempo de espera agotado. Usa el botón 'Verificar pago'.";
    }

    private async Task VerificarPagoManualAsync()
    {
        if (_depositoPendienteId == 0 || string.IsNullOrEmpty(_referenciaPendiente))
        {
            StatusDeposito = "No hay un depósito pendiente por verificar.";
            HayDepositoPendiente = false;
            return;
        }

        if (_confirmacionEnProceso)
        {
            StatusDeposito = "Verificación ya en proceso, espere...";
            return;
        }

        StatusDeposito = "Verificando pago manualmente...";
        await PollWompiAsync(_referenciaPendiente, _depositoPendienteId);
    }

    private async void RetirarConWompiAsync()
    {
        if (MontoRetiro <= 0)
        {
            StatusRetiro = "Ingrese un monto válido.";
            return;
        }

        if (DatosActuales == null)
        {
            StatusRetiro = "Debes registrar un método de pago antes de retirar.";
            return;
        }

        StatusRetiro = "Procesando retiro con Wompi...";

        try
        {
            var (ok, mensaje, idRetiro) = await _svc.SolicitarRetiroConWompiAsync(
                _usuario.IdUsuario, MontoRetiro, null);

            if (ok)
            {
                StatusRetiro = mensaje;
                MontoRetiro = 0;

                // Actualizar saldo en UI refrescando desde BD
                Usuario u = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
                if (u != null) 
                    _usuario.Saldo = u.Saldo;
                
                SaldoTexto = $"Saldo: ${_usuario.Saldo:N2}";
                _mainVM.ActualizarSaldo();
                SaldoActualizado?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                StatusRetiro = $"Error: {mensaje}";
            }
        }
        catch (System.Exception ex)
        {
            StatusRetiro = $"Error inesperado: {ex.Message}";
        }
    }
}
