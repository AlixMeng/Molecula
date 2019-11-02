using Molecula.Abstractions.Workflows.Core;

namespace Molecula.Workflows.Designer.Core
{
    public class NodeLinkPreview : WorkflowItem, INodeLinkPreview
    {
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

    }
}