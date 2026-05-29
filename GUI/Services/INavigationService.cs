using System.ComponentModel;

namespace GUI.Services;

public interface INavigationService : INotifyPropertyChanged
{
    ViewModels.ViewModelBase? CurrentViewModel { get; }
    void NavigateTo<TViewModel>() where TViewModel : ViewModels.ViewModelBase;
    void SetViewModel(ViewModels.ViewModelBase viewModel);
}
