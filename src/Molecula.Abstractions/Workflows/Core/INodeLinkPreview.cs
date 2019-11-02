namespace Molecula.Abstractions.Workflows.Core
{
    public interface INodeLinkPreview : IWorkflowItem
    {
        double? LinkPreviewStartX { get; set; }
        double? LinkPreviewStartY { get; set; }
        double? LinkPreviewEndX { get; set; }
        double? LinkPreviewEndY { get; set; }
    }
}