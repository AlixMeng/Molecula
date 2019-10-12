using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Molecula.UI.Extensions
{
    public static class FocusTraversalExtension
    {
        static FocusTraversalExtension()
        {
            Registry = new List<(Type Type, Delegate Action)>
            {
                (typeof(Panel), new Action<Panel>(panel => panel.BringIntoView())),
                (typeof(TextBox), new Action<TextBox>(textBox => textBox.CaretIndex = textBox.Text.Length)),
            };
        }

        public static Action GetTraversalAction(this DependencyObject element)
        {
            var actions = Registry
                .Where(entry => entry.Type.IsInstanceOfType(element))
                .ToList();

            if (actions.Count == 0)
                return null;

            return () => actions.ForEach(entry => entry.Action.DynamicInvoke(element));
        }

        private static readonly List<(Type Type, Delegate Action)> Registry;
    }
}