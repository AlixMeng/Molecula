using System;
using System.ComponentModel;
using Molecula.Abstractions.Workflows.Nodes;

namespace Molecula.Abstractions.Workflows.Core
{
    public interface IDesignerNode : INotifyPropertyChanged
    {
        Type NodeType { get; set; }

        IBaseNode Node { get; }

        string Namespace { get; }
    }
}