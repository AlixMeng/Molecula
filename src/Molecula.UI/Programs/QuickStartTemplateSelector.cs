using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Molecula.Abstractions.Dtos;

namespace Molecula.UI.Programs
{
    public class QuickStartTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is ProgramSetting setting)) return null;
            if (!(container is FrameworkElement element)) return null;
            if (!(element.TryFindResource("MainMenu.QuickStart.Button.Template") is DataTemplate baseTemplate)) return null;

            var builder = new StringBuilder(1024);
            using(var writer = new StringWriter(builder))
            {
                XamlWriter.Save(baseTemplate, writer);
            }
            builder.Replace("=\"{DynamicResource QuickStartButton.", "=\"{DynamicResource " + setting.ProgramId + ".QuickStartButton.");
            builder.Replace("<Button ", "<Button Command=\"{Binding DataContext.StartProgramCommand, RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type ItemsControl}}}\" CommandParameter=\"" + setting.ProgramId + "\" ");

            var context = new ParserContext {XamlTypeMapper = new XamlTypeMapper(new string[0])};
            var template = (DataTemplate)XamlReader.Parse(builder.ToString(), context);
            return template;
        }
    }
}
