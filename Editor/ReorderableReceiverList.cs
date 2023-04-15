using System;
using System.Collections;
using UnityEditorInternal;

namespace ResourceLocalization
{
    public class ReorderableReceiverList : ReorderableList
    {
        public ReorderableReceiverList(IList elements, Type elementType) : base(elements, elementType)
        {
        }
    }
}
