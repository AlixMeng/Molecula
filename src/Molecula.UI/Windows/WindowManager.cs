using System;
using System.Linq;
using System.Windows;
using Molecula.Abstractions.Services;
using Pamucuk.Mvvm.ViewModels;

namespace Molecula.UI.Windows
{
    public class WindowManager : IWindowManager
    {
        private readonly Func<Window> _createWindow;

        public WindowManager(Func<Window> createWindow)
        {
            _createWindow = createWindow;
        }

        public void OpenWindow(IViewModelBase viewModel)
        {
            var window = CreateWindow(viewModel);
            window.Show();
        }

        public void CloseWindow(IViewModelBase viewModel)
        {
            var window = FindWindow(viewModel);
            window.Close();
        }

        public bool? OpenDialog(IViewModelBase viewModel, IViewModelBase viewModelOwner)
        {
            var window = CreateWindow(viewModel);
            window.Owner = FindWindow(viewModelOwner);
            return window.ShowDialog();
        }

        public void CloseDialog(IViewModelBase viewModel, bool? dialogResult)
        {
            var window = FindWindow(viewModel);
            window.DialogResult = dialogResult;
            window.Close();
        }

        private Window CreateWindow(IViewModelBase viewModel)
        {
            var window = _createWindow();
            window.SetResourceReference(Window.TitleProperty, $"{viewModel.ViewModelId}.Window.Title");
            window.SetResourceReference(FrameworkElement.StyleProperty, $"{viewModel.ViewModelId}.Window.Style");
            window.DataContext = viewModel;
            window.Content = viewModel;
            return window;
        }

        private Window FindWindow(IViewModelBase viewModel)
            => Application.Current.Windows
                .OfType<Window>()
                .First(wnd => ReferenceEquals(wnd.DataContext, viewModel));
    }
}
