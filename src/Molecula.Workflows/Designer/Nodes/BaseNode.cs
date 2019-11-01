using System;
using System.Collections.ObjectModel;
using Molecula.Abstractions.Workflows.Core;
using Molecula.Abstractions.Workflows.Nodes;
using Molecula.Workflows.Designer.Core;

namespace Molecula.Workflows.Designer.Nodes
{
    public class BaseNode : WorkflowItem, IBaseNode
    {
        private string _iconId;
        public string IconId
        {
            get => _iconId;
            set => Set(ref _iconId, value);
        }

        private string _text;
        public string Text
        {
            get => _text;
            set => Set(ref _text, value);
        }

        private bool _hasInput = true;
        public bool HasInput
        {
            get => _hasInput;
            protected set => Set(ref _hasInput, value);
        }

        private bool _isRemovable = true;
        public bool IsRemovable
        {
            get => _isRemovable;
            protected set => Set(ref _isRemovable, value);
        }

        private NodeOutputType _outputType = NodeOutputType.Single;
        public NodeOutputType OutputType
        {
            get => _outputType;
            protected set
            {
                if(Set(ref _outputType, value))
                {
                    OnPropertyChanged(nameof(HasOutput));
                }
            }
        }

        public bool HasOutput => _outputType != NodeOutputType.None;

        private ObservableCollection<Guid> _outputNodes;
        public ObservableCollection<Guid> OutputNodes => _outputNodes ??= new ObservableCollection<Guid>();

        private double _x;
        public double X
        {
            get => _x;
            set => Set(ref _x, value);
        }

        private double _y;
        public double Y
        {
            get => _y;
            set => Set(ref _y, value);
        }

        private double _width = 150;
        public double Width
        {
            get => _width;
            set => Set(ref _width, value);
        }

        private double _height = 25;
        public double Height
        {
            get => _height;
            set => Set(ref _height, value);
        }

        private double? _connectStartX;
        public double? ConnectStartX
        {
            get => _connectStartX;
            set => Set(ref _connectStartX, value);
        }

        private double? _connectStartY;
        public double? ConnectStartY
        {
            get => _connectStartY;
            set => Set(ref _connectStartY, value);
        }

        private double? _connectEndX;
        public double? ConnectEndX
        {
            get => _connectEndX;
            set => Set(ref _connectEndX, value);
        }

        private double? _connectEndY;
        public double? ConnectEndY
        {
            get => _connectEndY;
            set => Set(ref _connectEndY, value);
        }

        protected BaseNode()
        {
            IconId = ItemType + ".Icon";
        }
    }
}
