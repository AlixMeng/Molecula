using System.Linq;
using System.Windows;
using Pamucuk.UI.Extensions;

namespace Molecula.UI.Extensions
{
    public static class FocusExtension
    {
        public static readonly DependencyProperty FocusIdProperty =
            DependencyProperty.RegisterAttached("FocusId", typeof(string), typeof(FocusExtension),
                new PropertyMetadata("", OnFocusIdChanged));

        public static string GetFocusId(FrameworkElement element)
            => (string)element.GetValue(FocusIdProperty);

        public static void SetFocusId(FrameworkElement element, string value)
            => element.SetValue(FocusIdProperty, value);

        private static void OnFocusIdChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
            => CheckAndSetFocus(obj);

        public static readonly DependencyProperty CurrentFocusIdProperty =
            DependencyProperty.RegisterAttached("CurrentFocusId", typeof(string), typeof(FocusExtension),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.Inherits, OnCurrentFocusIdChanged));

        public static string GetCurrentFocusId(FrameworkElement element)
            => (string)element.GetValue(CurrentFocusIdProperty);

        public static void SetCurrentFocusId(FrameworkElement element, string value)
            => element.SetValue(CurrentFocusIdProperty, value);

        private static void OnCurrentFocusIdChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
            => CheckAndSetFocus(obj);

        private static void CheckAndSetFocus(DependencyObject obj)
        {
            if (!(obj is FrameworkElement element)) return;
            if (!string.Equals(GetFocusId(element), GetCurrentFocusId(element))) return;
            PrepareTraversal(element);
            element.Focus();
        }

        private static void PrepareTraversal(DependencyObject focusTarget)
        {
            var window = focusTarget.FindParent<Window>();
            focusTarget.GetPathFromParent(window)
                .Select(element => element.GetTraversalAction())
                .Where(action => action != null)
                .ToList()
                .ForEach(action => action());
        }
    }
}
