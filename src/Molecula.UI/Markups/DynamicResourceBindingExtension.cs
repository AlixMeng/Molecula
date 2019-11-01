using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Pamucuk.Mvvm.Observables;

namespace Molecula.UI.Markups
{
    [MarkupExtensionReturnType(typeof(object))]
    [Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable, Readability = Readability.Unreadable)]
    public class DynamicResourceBindingExtension : MarkupExtension
    {
        public PropertyPath Path { get; set; }
        public object Source { get; set; }
        public RelativeSource RelativeSource { get; set; }
        public string ElementName { get; set; }
        
        public IValueConverter Converter { get; set; }
        public object ConverterParameter { get; set; }
        public CultureInfo ConverterCulture { get; set; } = CultureInfo.CurrentUICulture;
        public object TargetNullValue { get; set; }
        public string StringFormat { get; set; }
        public int Delay { get; set; }
        public bool IsAsync { get; set; }

        public IValueConverter KeyConverter { get; set; }
        public object KeyConverterParameter { get; set; }
        public CultureInfo KeyConverterCulture { get; set; } = CultureInfo.CurrentUICulture;
        public object KeyTargetNullValue { get; set; }
        public string KeyStringFormat { get; set; }
        public int KeyDelay { get; set; }
        public bool KeyIsAsync { get; set; }

        private static readonly DependencyProperty ResourceProxiesProperty =
            DependencyProperty.RegisterAttached(
                "ResourceProxies",
                typeof(ObservableDictionary<object, ResourceProxy>),
                typeof(DynamicResourceBindingExtension),
                new PropertyMetadata(default));

        internal static ObservableDictionary<object, ResourceProxy> GetResourceProxies(FrameworkElement element)
            => (ObservableDictionary<object, ResourceProxy>) element.GetValue(ResourceProxiesProperty);

        internal static void SetResourceProxies(FrameworkElement element, ObservableDictionary<object, ResourceProxy> value)
            => element.SetValue(ResourceProxiesProperty, value);

        public DynamicResourceBindingExtension()
        {

        }

        public DynamicResourceBindingExtension(PropertyPath path)
        {
            Path = path;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTarget = (IProvideValueTarget) serviceProvider.GetService(typeof(IProvideValueTarget));
            var targetProperty = GetTargetProperty((dynamic) provideValueTarget.TargetObject, provideValueTarget.TargetProperty);

            var resourceKeyBinding = CreateResourceKeyBinding();
            var resourceValueBinding = CreateResourceValueBinding(targetProperty);
            var resourceTargetBinding = CreateResourceTargetBinding();

            var multiBinding = new MultiBinding
            {
                Bindings =
                {
                    resourceTargetBinding,
                    resourceValueBinding,
                },
                Converter = MultiBindingConverter.Instance,
                ConverterParameter = (Binding: resourceKeyBinding, Property: targetProperty),
                Mode = BindingMode.OneWay
            };

            return multiBinding.ProvideValue(serviceProvider);
        }

        private static object GetTargetProperty(Setter targetObject, object targetProperty) => targetObject.Property;

        private static object GetTargetProperty(object targetObject, object targetProperty) => targetProperty;
 
        private Binding CreateResourceKeyBinding()
        {
            var resourceKeyBinding = new Binding
            {
                Path = Path,
                Mode = BindingMode.OneWay,
                Converter = KeyConverter,
                ConverterParameter = KeyConverterParameter,
                ConverterCulture = KeyConverterCulture,
                StringFormat = KeyStringFormat,
                Delay = KeyDelay,
                IsAsync = KeyIsAsync,
            };

            if (KeyTargetNullValue != null)
                resourceKeyBinding.TargetNullValue = KeyTargetNullValue;

            if (Source != null)
                resourceKeyBinding.Source = Source;
            else if (ElementName != null)
                resourceKeyBinding.ElementName = ElementName;
            else if (RelativeSource != null)
                resourceKeyBinding.RelativeSource = RelativeSource;

            return resourceKeyBinding;
        }
       
        private Binding CreateResourceValueBinding(object targetProperty)
        {
            var resourceValueBinding = new Binding
            {
                Path = new PropertyPath("(0)[(1)].(2)", ResourceProxiesProperty, targetProperty,
                    ResourceProxy.ResourceValueProperty),
                RelativeSource = new RelativeSource(RelativeSourceMode.Self),
                Converter = Converter,
                ConverterParameter = ConverterParameter,
                ConverterCulture = ConverterCulture,
                StringFormat = StringFormat,
                Delay = Delay,
                IsAsync = IsAsync,
                Mode = BindingMode.OneWay,
            };
            if (TargetNullValue != null)
                resourceValueBinding.TargetNullValue = TargetNullValue;
            return resourceValueBinding;
        }

        private static Binding CreateResourceTargetBinding()
            => new Binding
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.Self),
                Mode = BindingMode.OneTime
            };

        private class MultiBindingConverter : IMultiValueConverter
        {
            public static IMultiValueConverter Instance { get; } = new MultiBindingConverter();

            private MultiBindingConverter()
            {
            }

            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                if (!(values[0] is FrameworkElement element)) return null;
                var proxies = GetResourceProxies(element);
                if (proxies == null)
                {
                    SetResourceProxies(element, proxies = new ObservableDictionary<object, ResourceProxy>(true));
                }
                var (resourceKeyBinding, property) = ((Binding, object)) parameter;
                
                if (property != null && !proxies.ContainsKey(property))
                {
                    var proxy = new ResourceProxy();
                    element.Resources[property] = proxy;
                    proxies[property] = proxy;
                    BindingOperations.SetBinding(proxy, ResourceProxy.ResourceKeyProperty, resourceKeyBinding);
                    return proxy.ResourceValue;
                }
                return values[1] ?? (property != null ? ((ResourceProxy) element.Resources[property]).ResourceValue : null);
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        internal class ResourceProxy : Freezable
        {
            public static readonly DependencyProperty ResourceKeyProperty =
                DependencyProperty.Register(nameof(ResourceKey), typeof(object), typeof(ResourceProxy), 
                    new PropertyMetadata(default, OnResourceKeyChanged));

            public object ResourceKey
            {
                get => GetValue(ResourceKeyProperty);
                set => SetValue(ResourceKeyProperty, value);
            }

            private static void OnResourceKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                if (e.NewValue == null || e.NewValue == DependencyProperty.UnsetValue)
                {
                    d.ClearValue(ResourceValueProperty);
                }
                else
                {
                    d.SetValue(ResourceValueProperty, new DynamicResourceExtension(e.NewValue.ToString()).ProvideValue(null));
                }
            }

            public static readonly DependencyProperty ResourceValueProperty =
                DependencyProperty.Register(nameof(ResourceValue), typeof(object), typeof(ResourceProxy), 
                    new PropertyMetadata(default));

            public object ResourceValue
            {
                get => GetValue(ResourceValueProperty);
                set => SetValue(ResourceValueProperty, value);
            }

            protected override Freezable CreateInstanceCore() => new ResourceProxy();
        }
    }
}
