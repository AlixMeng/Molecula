using Pamucuk.Mvvm.ViewModels;

namespace Molecula.Abstractions.Services
{
    public interface IWindowManager
    {
        void OpenWindow(IViewModelBase viewModel);

        void CloseWindow(IViewModelBase viewModel);

        bool? OpenDialog(IViewModelBase viewModel, IViewModelBase viewModelOwner);

        void CloseDialog(IViewModelBase viewModel, bool? dialogResult);
    }
}