﻿using System;
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
        ICommand ProcessKeyCommand { get; }
        ICommand StartConnectionCommand { get; }
        ICommand MoveConnectionCommand { get; }
        ICommand StopConnectionCommand { get; }
        ICommand MoveNodeCommand { get; }
        string Name { get; set; }

        void AddNode(Type nodeType);
        void DeleteSelectedItems();
    }
}