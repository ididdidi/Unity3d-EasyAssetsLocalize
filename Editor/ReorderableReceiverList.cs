using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;

namespace ResourceLocalization
{
    public class ReorderableReceiverList : ReorderableList
    {
        private ILocalizationRepository LocalizationStorege { get; }
        public ReorderableReceiverList(List<LocalizationReceiver> receivers, ILocalizationRepository localizationStorege) : base(receivers, typeof(LocalizationReceiver))
        {
            LocalizationStorege = localizationStorege;
            onAddCallback = AddRecever;
            drawElementCallback = DrowLocalizationReceiver;
        }

        private void DrowLocalizationReceiver(Rect rect, int index, bool isActive, bool isFocused)
        {
            var objectFieldRect = new Rect(new Vector2(rect.x, rect.y), new Vector2(150, rect.height));
            var receiver = (LocalizationReceiver)list[index];
            receiver = EditorGUI.ObjectField(objectFieldRect, receiver, typeof(LocalizationReceiver), true) as LocalizationReceiver;
            if (GUI.changed && receiver != (LocalizationReceiver)list[index])
            {
                if((LocalizationReceiver)list[index] != null)
                {
                    var tag = ((LocalizationReceiver)list[index]).LocalizationTag;
                    if (LocalizationStorege.Conteins(tag)) { LocalizationStorege.RemoveResource(tag); }
                }

                if(receiver != null && !LocalizationStorege.Conteins(receiver.LocalizationTag))
                {
                    LocalizationStorege.AddResource(receiver.LocalizationTag, receiver.Resource);
                }
                list[index] = receiver;
            }
        }

        private void AddRecever(ReorderableList reorderable)
        {
            reorderable.list.Add(null);
        }
    }
}
