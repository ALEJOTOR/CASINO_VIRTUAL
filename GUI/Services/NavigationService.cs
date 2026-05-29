using GUI.ViewModels;

namespace GUI.Services;

public class NavigationService : ViewModelBase, INavigationService
{
    private readonly Func<Type, ViewModelBase> _factory;
    private ViewModelBase? _current;

    public ViewModelBase? CurrentViewModel
    {
        get => _current;
        private set => SetProperty(ref _current, value);
    }

    public NavigationService(Func<Type, ViewModelBase> factory) => _factory = factory;

    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        => CurrentViewModel = _factory(typeof(TViewModel));

    public void SetViewModel(ViewModelBase viewModel)
        => CurrentViewModel = viewModel;
}
