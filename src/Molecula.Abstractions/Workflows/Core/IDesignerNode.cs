using System;
using System.ComponentModel;

namespace Molecula.Abstractions.Workflows.Core
{
    public interface IDesignerNode : INotifyPropertyChanged
    {
        Type NodeType { get; set; }

        string Namespace { get; }
    }
}