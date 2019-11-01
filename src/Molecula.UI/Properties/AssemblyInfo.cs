using System.Windows.Markup;

[assembly: XmlnsDefinition("http://schemas.molecula.com/molecula/2019/converter/xaml", nameof(Molecula) + "." + nameof(Molecula.UI) + "." + nameof(Molecula.UI.Converter))]
[assembly: XmlnsDefinition("http://schemas.molecula.com/molecula/2019/extensions/xaml", nameof(Molecula) + "." + nameof(Molecula.UI) + "." + nameof(Molecula.UI.Extensions))]
[assembly: XmlnsDefinition("http://schemas.molecula.com/molecula/2019/markups/xaml", nameof(Molecula) + "." + nameof(Molecula.UI) + "." + nameof(Molecula.UI.Markups))]

[assembly: XmlnsPrefix("http://schemas.molecula.com/molecula/2019/converter/xaml", "molconv")]
[assembly: XmlnsPrefix("http://schemas.molecula.com/molecula/2019/extensions/xaml", "molext")]
[assembly: XmlnsPrefix("http://schemas.molecula.com/molecula/2019/markups/xaml", "molmark")]
