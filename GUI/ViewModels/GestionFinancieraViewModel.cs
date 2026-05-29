using ENTITY;

namespace GUI.ViewModels;

public class GestionFinancieraViewModel : ViewModelBase
{
    private readonly Usuario _admin;
    private string _titulo = "Gestión Financiera";

    public string Titulo { get => _titulo; set => SetProperty(ref _titulo, value); }

    public GestionFinancieraViewModel(Usuario admin)
    {
        _admin = admin;
    }
}
