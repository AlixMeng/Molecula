using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Molecula.Abstractions.Workflows.Core
{
    public interface IWorkspace : INotifyPropertyChanged
    {
        IEnumerable<IWorkflowItem> Items { get; }
        double HorizontalOffset { get; set; }
        double VerticalOffset { get; set; }
        ICommand AddNodeCommand { get; }
        ICommand DeleteSelectedItemsCommand { get; }
        string Name { get; set; }

        void AddNode(Type nodeType);
        void DeleteSelectedItems();
    }
}