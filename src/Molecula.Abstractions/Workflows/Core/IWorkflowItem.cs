using System;
using System.ComponentModel;

namespace Molecula.Abstractions.Workflows.Core
{
    public interface IWorkflowItem : INotifyPropertyChanged
    {
        Guid ItemId { get; set; }
        bool IsChecked { get; set; }
        string ItemType { get; }
    }
}