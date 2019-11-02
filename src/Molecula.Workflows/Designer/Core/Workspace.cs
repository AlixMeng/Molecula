using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Molecula.Abstractions.Workflows.Core;
using Molecula.Core.Extensions;
using Molecula.Workflows.Designer.Nodes;
using Pamucuk.Mvvm.Commands;
using Pamucuk.Mvvm.Observables;

namespace Molecula.Workflows.Designer.Core
{
    public class Workspace : ObservableObject, IWorkspace
    {
        private readonly ObservableCollection<IWorkflowItem> _items = new ObservableCollection<IWorkflowItem>();
        public IEnumerable<IWorkflowItem> Items => _items;

        private double _horizontalOffset;

        public double HorizontalOffset
        {
            get => _horizontalOffset;
            set => Set(ref _horizontalOffset, value);
        }

        private double _verticalOffset;

        public double VerticalOffset
        {
            get => _verticalOffset;
            set => Set(ref _verticalOffset, value);
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private double? _linkPreviewStartX;

        public double? LinkPreviewStartX
        {
            get => _linkPreviewStartX;
            set => Set(ref _linkPreviewStartX, value);
        }

        private double? _linkPreviewStartY;

        public double? LinkPreviewStartY
        {
            get => _linkPreviewStartY;
            set => Set(ref _linkPreviewStartY, value);
        }

        private double? _linkPreviewEndX;

        public double? LinkPreviewEndX
        {
            get => _linkPreviewEndX;
            set => Set(ref _linkPreviewEndX, value);
        }

        private double? _linkPreviewEndY;

        public double? LinkPreviewEndY
        {
            get => _linkPreviewEndY;
            set => Set(ref _linkPreviewEndY, value);
        }


        private ICommand _startConnectionCommand;

        public ICommand StartConnectionCommand =>
            _startConnectionCommand ??=
                new RelayCommand<(double HorizontalOffset, double VerticalOffset, double X, double Y, double Width, double Height, object Item)>(StartConnection);

        private void StartConnection((double HorizontalOffset, double VerticalOffset, double X, double Y, double Width, double Height, object Item) connectStart)
        {
            var startX = connectStart.X + connectStart.Width;
            var startY = connectStart.Y + (connectStart.Height / 2);

            if (!IsLinkAllowed(startX, startY, node => node.HasOutput)) 
                return;
            
            LinkPreviewStartX = startX;
            LinkPreviewStartY = startY;
            LinkPreviewEndX = LinkPreviewStartX;
            LinkPreviewEndY = LinkPreviewStartY;
        }

        private ICommand _moveConnectionCommand;

        public ICommand MoveConnectionCommand =>
            _moveConnectionCommand ??= new RelayCommand<(double HorizontalChange, double VerticalChange, double X, double Y, double Width, double Height, object Item)>(MoveConnection);

        private void MoveConnection((double HorizontalChange, double VerticalChange, double X, double Y, double Width, double Height, object Item) connectDelta)
        {
            if (!IsLinkAllowed(LinkPreviewStartX, LinkPreviewStartY, node => node.HasOutput))
                return;

            LinkPreviewEndX = connectDelta.X + connectDelta.Width + connectDelta.HorizontalChange;
            LinkPreviewEndY = connectDelta.Y + (connectDelta.Height / 2) + connectDelta.VerticalChange;
        }

        private ICommand _stopConnectionCommand;

        public ICommand StopConnectionCommand =>
            _stopConnectionCommand ??= new RelayCommand<(double HorizontalChange, double VerticalChange, double X, double Y, double Width, double Height, object Item)>(StopConnection);

        private void StopConnection((double HorizontalChange, double VerticalChange, double X, double Y, double Width, double Height, object Item) connectComplete)
        {
            var startNode = FindNode(LinkPreviewStartX - 5, LinkPreviewStartY);
            var endNode = FindNode(LinkPreviewEndX, LinkPreviewEndY);

            if (startNode != null 
                && endNode != null
                && startNode != endNode
                && startNode.HasOutput
                && endNode.HasInput)
            {
                AddLink(startNode, endNode);
            }

            ClearSelection();

            LinkPreviewStartX = default;
            LinkPreviewStartY = default;
            LinkPreviewEndX = default;
            LinkPreviewEndY = default;
        }

        private ICommand _moveCommand;
        public ICommand MoveCommand => _moveCommand ??= new RelayCommand<(double HorizontalChange, double VerticalChange, double X, double Y, double Width, double Height, object Item)>(Move);

        private void Move((double HorizontalChange, double VerticalChange, double X, double Y, double Width, double Height, object Item) moveDelta)
        {
            _items
                .OfType<BaseNode>()
                .Where(node => node == moveDelta.Item || node.IsChecked)
                .ForEach(node =>
                {
                    node.X += moveDelta.HorizontalChange;
                    node.Y += moveDelta.VerticalChange;
                });
        }

        public Workspace(ICommandFactory commandFactory)
        {
            AddNodeCommand = commandFactory.Create<Type>(AddNode);
            ProcessKeyCommand = commandFactory.Create<(Key Key, ModifierKeys Modifiers)?>(ProcessKey);
            _items.Add(new NodeLinkPreview());
        }

        public ICommand AddNodeCommand { get; }

        public void AddNode(Type nodeType)
        {
            var node = (BaseNode) Activator.CreateInstance(nodeType);

            node.X = HorizontalOffset + 10;
            node.Y = VerticalOffset + 10;

            Add(node);
        }

        public ICommand ProcessKeyCommand { get; }
        
        private void ProcessKey((Key Key, ModifierKeys Modifiers)? parameter)
        {
            var key = parameter?.Key ?? Key.None;
            switch (key)
            {
                case Key.Delete:
                    DeleteSelectedItems();
                    break;
                case Key.Escape:
                    ClearSelection();
                    break;
            }

        }

        public void DeleteSelectedItems()
            => Items
                .Where(item => item.IsChecked)
                .ToArray()
                .ForEach(Remove);

        private void ClearSelection()
        {
            _items.ForEach(item => item.IsChecked = false);
        }

        private bool IsLinkAllowed(double? startX, double? startY, Func<BaseNode, bool> isAllowed)
        {
            var node = FindNode(startX, startY);
            return isAllowed(node);
        }

        private BaseNode FindNode(double? x, double? y)
            => _items.OfType<BaseNode>().FirstOrDefault(item => IsNodeHit(item, x, y));

        private static bool IsNodeHit(BaseNode node, double? x, double? y)
            => node.X <= x
               && x <= node.X + node.Width
               && node.Y <= y
               && y <= node.Y + node.Height;

        private void Remove(IWorkflowItem item)
        {
            _items.Remove(item);
            PostProcessRemove((dynamic) item);
        }

        private static void PostProcessRemove(NodeLink link)
        {
            link.StartNode.OutputNodes.Remove(link.EndNode.ItemId);
        }

        private void PostProcessRemove(BaseNode node)
        {
            Items
                .OfType<NodeLink>()
                .Where(link => link.StartNode == node || link.EndNode == node)
                .ToArray()
                .ForEach(Remove);
        }

        private void AddOutputLinksFromNode(BaseNode node)
            => node.OutputNodes.ForEach(nodeId => AddLink(node, nodeId));

        private void AddLink(BaseNode startNode, Guid endNodeId)
        {
            var endNode = FindNodeById(endNodeId);
            AddLink(startNode, endNode);
        }

        private void AddLink(BaseNode startNode, BaseNode endNode)
        {
            if (startNode.OutputType == NodeOutputType.Single)
            {
                RemoveLinks(startNode);
            }
            
            var link = CreateLink(startNode, endNode);
            Add(link);
        }

        private void RemoveLinks(BaseNode startNode)
        {
            var links = _items.OfType<NodeLink>().Where(link => link.StartNode == startNode).ToList();
            links.ForEach(link => _items.Remove(link));
        }

        private BaseNode FindNodeById(Guid endNodeId)
            => Items
                .OfType<BaseNode>()
                .FirstOrDefault(node => node.ItemId == endNodeId);

        private static NodeLink CreateLink(BaseNode startNode, BaseNode endNode)
            => new NodeLink
            {
                StartNode = startNode,
                EndNode = endNode,
            };

        private void Add(WorkflowItem item)
            => _items.Add(item);
    }
}
