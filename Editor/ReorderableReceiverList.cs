using System.Collections.Generic;
using UnityEditorInternal;

namespace ResourceLocalization
{
    public class ReorderableReceiverList : ReorderableList
    {
        public ReorderableReceiverList(List<LocalizationReceiver> receivers) : base(receivers, typeof(LocalizationReceiver))
        {
        }   
    }
}
