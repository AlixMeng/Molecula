using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Molecula.Abstractions.Workflows.Core
{
    public interface IWorkspace : IWorkflowItemContainer, INotifyPropertyChanged
    {
        IEnumerable<IWorkflowItem> Items { get; }
        double HorizontalOffset { get; set; }
        double VerticalOffset { get; set; }
        double ViewWidth { get; set; }
        double ViewHeight { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        string Name { get; set; }
        INodeLinkPreview NodeLinkPreview { get; }

        ICommand ProcessIsCheckedChangedCommand { get; }
        ICommand ProcessKeyUpCommand { get; }
        ICommand ProcessKeyDownCommand { get; }
        ICommand DropNodeCommand { get; }
        ICommand ClearSelectionCommand { get; }

        void AddNode(Type nodeType);
        void DeleteSelectedItems();
    }
}