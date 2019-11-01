using System;
using System.Collections.ObjectModel;
using Molecula.Abstractions.Workflows.Core;

namespace Molecula.Abstractions.Workflows.Nodes
{
    public interface IBaseNode : IWorkflowItem
    {
        string IconId { get; set; }
        string Text { get; set; }
        bool HasInput { get; }
        bool IsRemovable { get; }
        NodeOutputType OutputType { get; }
        bool HasOutput { get; }
        ObservableCollection<Guid> OutputNodes { get; }
        double X { get; set; }
        double Y { get; set; }
        double Width { get; set; }
        double Height { get; set; }
    }
}