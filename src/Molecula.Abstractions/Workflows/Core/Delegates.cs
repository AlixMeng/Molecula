using System;

namespace Molecula.Abstractions.Workflows.Core
{
    public delegate TInstance CreateWorkflowItem<out TInstance>(Type type) where TInstance : IWorkflowItem;
}
