using System.Collections;
using UnityEditorInternal;

namespace ResourceLocalization
{
    public class TypesListView : ReorderableList
    {
        public TypesListView(IList list) : base(list, typeof(string), true, true, true, true)
        {

        }
    }
}