using System;
using Molecula.Abstractions.Workflows.Core;
using Pamucuk.Mvvm.Observables;

namespace Molecula.Workflows.Designer.Core
{
    public class WorkflowItem : ObservableObject, IWorkflowItem
    {
        private Guid _itemId;
        public Guid ItemId
        {
            get => _itemId;
            set => Set(ref _itemId, value);
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set => Set(ref _isChecked, value);
        }

        private string _itemType;
        public string ItemType => _itemType ??= $"{GetType().Namespace}.{GetType().Name}";
        
        protected WorkflowItem()
        {
            ItemId = Guid.NewGuid();
        }
    }
}