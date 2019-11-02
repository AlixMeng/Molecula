using System;
using Molecula.Abstractions.Workflows.Core;
using Pamucuk.Mvvm.Observables;

namespace Molecula.Workflows.Designer.Nodes
{
    public class DesignerNode : ObservableObject, IDesignerNode
    {
        private Type _nodeType;
        public Type NodeType
        {
            get => _nodeType;
            set
            {
                if (Set(ref _nodeType, value))
                {
                    OnPropertyChanged(nameof(Namespace));
                }
            }
        }

        public string Namespace => _nodeType.Namespace;
    }
}
