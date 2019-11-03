using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Input;
using Molecula.Abstractions.Workflows.Core;
using Molecula.Abstractions.Workflows.Nodes;
using Molecula.Core.Extensions;
using Pamucuk.Mvvm.Commands;
using Pamucuk.Mvvm.Observables;

namespace Molecula.Workflows.Designer.Core
{
    public class Workspace : ObservableObject, IWorkspace
    {
        private readonly CreateWorkflowItem<IBaseNode> _nodeFactory;
        private readonly CreateWorkflowItem<INodeLink> _linkFactory;
        private readonly CreateWorkflowItem<INodeLinkPreview> _linkPreviewFactory;
        private bool _isMultiSelectMode;
        private bool _isProcessIsCheckedDisabled;

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

        private double _viewWidth;
        public double ViewWidth
        {
            get => _viewWidth;
            set => Set(ref _viewWidth, value);
        }

        private double _viewHeight;
        public double ViewHeight
        {
            get => _viewHeight;
            set => Set(ref _viewHeight, value);
        }

        private int _width = 10_000;
        public int Width
        {
            get => _width;
            set => Set(ref _width, value);
        }

        private int _height = 10_000;
        public int Height
        {
            get => _height;
            set => Set(ref _height, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private INodeLinkPreview _nodeLinkPreview;
        public INodeLinkPreview NodeLinkPreview
        {
            get => _nodeLinkPreview;
            private set => Set(ref _nodeLinkPreview, value);
        }

        public ICommand ProcessIsCheckedChangedCommand { get; }
        public ICommand ProcessKeyDownCommand { get; }
        public ICommand ProcessKeyUpCommand { get; }
        public ICommand DropNodeCommand { get; }
        public ICommand StartConnectionCommand { get; }
        public ICommand MoveConnectionCommand { get; }
        public ICommand StopConnectionCommand { get; }
        public ICommand MoveNodeCommand { get; }
        public ICommand ClearSelectionCommand { get; }

        public Workspace(
            ICommandFactory commandFactory,
            Func<CreateWorkflowItem<IBaseNode>> getNodeFactory,
            Func<CreateWorkflowItem<INodeLink>> getLinkFactory,
            Func<CreateWorkflowItem<INodeLinkPreview>> getLinkPreviewFactory)
        {
            _nodeFactory = getNodeFactory();
            _linkFactory = getLinkFactory();
            _linkPreviewFactory = getLinkPreviewFactory();
            ProcessIsCheckedChangedCommand = commandFactory.Create<IWorkflowItem>(ProcessIsCheckedChanged, CanProcessIsCheckedChanged);
            ProcessKeyDownCommand = commandFactory.Create<(Key Key, ModifierKeys Modifiers)?>(ProcessKeyDown);
            ProcessKeyUpCommand = commandFactory.Create<(Key Key, ModifierKeys Modifiers)?>(ProcessKeyUp);
            DropNodeCommand = commandFactory.Create<(double X, double Y, IDesignerNode Item)>(DropNode, CanDropNode);
            StartConnectionCommand = commandFactory.Create<(double HorizontalOffset, double VerticalOffset, double X, double Y, double Width, double Height, object Item)>(StartConnection);
            MoveConnectionCommand = commandFactory.Create<(double HorizontalChange, double VerticalChange, double X, double Y, double Width, double Height, object Item)>(MoveConnection);
            StopConnectionCommand = commandFactory.Create<(double HorizontalChange, double VerticalChange, double X, double Y, double Width, double Height, object Item)>(StopConnection);
            MoveNodeCommand = commandFactory.Create<(double HorizontalChange, double VerticalChange, double X, double Y, double Width, double Height, object Item)>(MoveSelection);
            ClearSelectionCommand = commandFactory.Create<object>(ProcessClearSelection, CanProcessClearSelection);
            NodeLinkPreview = _linkPreviewFactory(typeof(INodeLinkPreview));
            Add(NodeLinkPreview);
        }

        private static bool CanDropNode((double X, double Y, IDesignerNode Item) dragComplete)
            => dragComplete.X > 0 && dragComplete.Y > 0 && dragComplete.Item != null;

        private void DropNode((double X, double Y, IDesignerNode Item) dragComplete)
        {
            var node = _nodeFactory(dragComplete.Item.NodeType);

            node.X = dragComplete.X;
            node.Y = dragComplete.Y;

            Add(node);
        }

        public void AddNode(Type nodeType)
        {
            var node = _nodeFactory(nodeType);

            node.X = HorizontalOffset + 10;
            node.Y = VerticalOffset + 10;

            Add(node);
        }

        public void DeleteSelectedItems()
            => Items
                .Where(item => item.IsChecked && ((item as IBaseNode)?.IsRemovable ?? true))
                .ToArray()
                .ForEach(Remove);

        private static bool CanProcessClearSelection(object parameter)
            => parameter is IWorkspace;

        private void ProcessClearSelection(object parameter)
            => ClearSelection(null);

        public void ClearSelection(IWorkflowItem ignoreItem)
        {
            if (_isProcessIsCheckedDisabled)
                return;
            _isProcessIsCheckedDisabled = true;
            GetSelectedItems<IWorkflowItem>()
                .Where(selected => selected != ignoreItem)
                .ToList()
                .ForEach(item => item.IsChecked = false);
            _isProcessIsCheckedDisabled = false;
        }

        private bool CanProcessIsCheckedChanged(IWorkflowItem item)
        {
            return !_isProcessIsCheckedDisabled;
        }

        private void ProcessIsCheckedChanged(IWorkflowItem item)
        {
            if (!_isMultiSelectMode && item.IsChecked)
            {
                ClearSelection(item);
            }
        }

        private void ProcessKeyDown((Key Key, ModifierKeys Modifiers)? parameter)
        {
            if (IsKey(parameter, Key.LeftCtrl, Key.RightCtrl)
                || IsModifier(parameter, ModifierKeys.Control))
            {
                _isMultiSelectMode = true;
            }
        }

        private void ProcessKeyUp((Key Key, ModifierKeys Modifiers)? parameter)
        {
            var key = parameter?.Key ?? Key.None;
            if (IsKey(parameter, Key.LeftCtrl, Key.RightCtrl)
                || IsModifier(parameter, ModifierKeys.Control))
            {
                _isMultiSelectMode = false;
            }

            switch (key)
            {
                case Key.Delete:
                    DeleteSelectedItems();
                    break;
                case Key.Escape:
                    ClearSelection(null);
                    break;
            }
        }

        private static bool IsKey((Key Key, ModifierKeys Modifiers)? parameter, params Key[] expectedKeys)
        {
            var key = parameter?.Key ?? Key.None;
            return expectedKeys.Any(k => k == key);
        }

        private static bool IsModifier((Key Key, ModifierKeys Modifiers)? parameter, ModifierKeys expectedModifiers)
        {
            var modifiers = parameter?.Modifiers ?? ModifierKeys.None;
            return (modifiers & expectedModifiers) == expectedModifiers;
        }

        private void StartConnection((double HorizontalOffset, double VerticalOffset, double X, double Y, double Width, double Height, object Item) connectStart)
        {
            var startX = connectStart.X + connectStart.Width;
            var startY = connectStart.Y + (connectStart.Height / 2);

            if (!IsLinkAllowed(startX, startY, node => node.HasOutput))
                return;

            NodeLinkPreview.LinkPreviewStartX = startX;
            NodeLinkPreview.LinkPreviewStartY = startY;
            NodeLinkPreview.LinkPreviewEndX = NodeLinkPreview.LinkPreviewStartX;
            NodeLinkPreview.LinkPreviewEndY = NodeLinkPreview.LinkPreviewStartY;
        }

        private void MoveConnection((double HorizontalChange, double VerticalChange, double X, double Y, double Width, double Height, object Item) connectDelta)
        {
            if (!IsLinkAllowed(NodeLinkPreview.LinkPreviewStartX, NodeLinkPreview.LinkPreviewStartY, node => node.HasOutput))
                return;

            NodeLinkPreview.LinkPreviewEndX = connectDelta.X + connectDelta.Width + connectDelta.HorizontalChange;
            NodeLinkPreview.LinkPreviewEndY = connectDelta.Y + (connectDelta.Height / 2) + connectDelta.VerticalChange;
        }

        private void StopConnection((double HorizontalChange, double VerticalChange, double X, double Y, double Width, double Height, object Item) connectComplete)
        {
            var startNode = FindNode(NodeLinkPreview.LinkPreviewStartX - 5, NodeLinkPreview.LinkPreviewStartY);
            var endNode = FindNode(NodeLinkPreview.LinkPreviewEndX, NodeLinkPreview.LinkPreviewEndY);

            if (startNode != null
                && endNode != null
                && startNode != endNode
                && startNode.HasOutput
                && endNode.HasInput)
            {
                AddLink(startNode, endNode);
            }

            ClearSelection(null);

            NodeLinkPreview.LinkPreviewStartX = default;
            NodeLinkPreview.LinkPreviewStartY = default;
            NodeLinkPreview.LinkPreviewEndX = default;
            NodeLinkPreview.LinkPreviewEndY = default;
        }

        private void MoveSelection((double HorizontalChange, double VerticalChange, double X, double Y, double Width, double Height, object Item) moveDelta)
        {
            var mainItem = (IBaseNode) moveDelta.Item;
            var oldRectangle = GetSelectionRectangle(mainItem);
            var newRectangle = CalculateTargetRectangle(oldRectangle, moveDelta.HorizontalChange, moveDelta.VerticalChange);
            GetSelectedItems<IBaseNode>(mainItem)
                .ForEach(node =>
                {
                    node.X += newRectangle.X - oldRectangle.X;
                    node.Y += newRectangle.Y - oldRectangle.Y;
                });
            HorizontalOffset = CalculateOffset(mainItem.X, mainItem.X + mainItem.Width, HorizontalOffset, ViewWidth);
            VerticalOffset = CalculateOffset(mainItem.Y, mainItem.Y + mainItem.Height, VerticalOffset, ViewHeight);
        }

        private static double CalculateOffset(double itemMin, double itemMax, double offset, double viewSize)
        {
            if (itemMax > offset + viewSize)
            {
                return itemMax - viewSize;
            }

            return itemMin < offset 
                ? itemMin 
                : offset;
        }

        private Rect CalculateTargetRectangle(Rect rectangle, double horizontalChange, double verticalChange)
            => new Rect(
                Math.Min(Math.Max(rectangle.Left + horizontalChange, 0), Width - rectangle.Width),
                Math.Min(Math.Max(rectangle.Top + verticalChange, 0), Height - rectangle.Height),
                rectangle.Width,
                rectangle.Height
            );

        private Rect GetSelectionRectangle(IWorkflowItem mainSelection = null)
        {
            var selection = GetSelectedItems<IBaseNode>(mainSelection).ToList();
            var rectangle = selection
                .Select(node => (Left: node.X, Top: node.Y, Right: node.X + node.Width, Bottom: node.Y + node.Height))
                .Aggregate((Left: (double)Width, Top: (double)Height, Right: 0.0, Bottom: 0.0),
                    (seed, current) => (Left: Math.Min(seed.Left, current.Left), Top: Math.Min(seed.Top, current.Top),
                        Right: Math.Max(seed.Right, current.Right), Bottom: Math.Max(seed.Bottom, current.Bottom)));
            return new Rect(rectangle.Left, rectangle.Top, rectangle.Right - rectangle.Left, rectangle.Bottom - rectangle.Top);
        }

        private IEnumerable<TItem> GetSelectedItems<TItem>(IWorkflowItem mainSelection = null) where TItem : IWorkflowItem
            => _items
                .OfType<TItem>()
                .Where(node => Equals(node, mainSelection) || node.IsChecked);

        private bool IsLinkAllowed(double? startX, double? startY, Func<IBaseNode, bool> isAllowed)
        {
            var node = FindNode(startX, startY);
            return isAllowed(node);
        }

        private IBaseNode FindNode(double? x, double? y)
            => Items.OfType<IBaseNode>().FirstOrDefault(item => IsNodeHit(item, x, y));

        private static bool IsNodeHit(IBaseNode node, double? x, double? y)
            => node.X <= x
               && x <= node.X + node.Width
               && node.Y <= y
               && y <= node.Y + node.Height;

        private void Remove(IWorkflowItem item)
        {
            _items.Remove(item);
            PostProcessRemove((dynamic)item);
        }

        private static void PostProcessRemove(INodeLink link)
        {
            link.StartNode.OutputNodes.Remove(link.EndNode.ItemId);
        }

        private void PostProcessRemove(IBaseNode node)
        {
            Items
                .OfType<INodeLink>()
                .Where(link => link.StartNode == node || link.EndNode == node)
                .ToArray()
                .ForEach(Remove);
        }

        private void AddOutputLinksFromNode(IBaseNode node)
            => node.OutputNodes.ForEach(nodeId => AddLink(node.ItemId, nodeId));

        private void AddLink(Guid startNodeId, Guid endNodeId)
        {
            var startNode = FindNodeById(startNodeId);
            var endNode = FindNodeById(endNodeId);
            AddLink(startNode, endNode);
        }

        private void AddLink(IBaseNode startNode, IBaseNode endNode)
        {
            if (startNode.OutputType == NodeOutputType.Single)
            {
                RemoveLinks(startNode);
            }

            var link = CreateLink(startNode, endNode);
            Add(link);
        }

        private void RemoveLinks(IBaseNode startNode)
        {
            var links = _items.OfType<INodeLink>().Where(link => link.StartNode == startNode).ToList();
            links.ForEach(link => _items.Remove(link));
        }

        private IBaseNode FindNodeById(Guid endNodeId)
            => Items
                .OfType<IBaseNode>()
                .FirstOrDefault(node => node.ItemId == endNodeId);

        private INodeLink CreateLink(IBaseNode startNode, IBaseNode endNode)
        {
            var link = _linkFactory(typeof(INodeLink));
            link.StartNode = startNode;
            link.EndNode = endNode;
            return link;
        }

        private void Add(IWorkflowItem item)
            => _items.Add(item);
    }
}
