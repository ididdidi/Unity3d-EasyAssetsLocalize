using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;

namespace ResourceLocalization
{
    public class ReorderableReceiverList : ReorderableList
    {
        private LocalizationStorage LocalizationStorege { get; }
        public ReorderableReceiverList(List<LocalizationReceiver> receivers, LocalizationStorage localizationStorege) : base(receivers, typeof(LocalizationReceiver))
        {
            LocalizationStorege = localizationStorege;

            drawHeaderCallback = DrawHeader;

            if (CheckingLanguages())
            {
                onAddCallback = AddRecever;
                onRemoveCallback = RemoveRecever;
                drawElementCallback = DrowLocalizationReceiver;
            }
            else
            {
                receivers.Clear();
                drawNoneElementCallback = DrawLanguagesError;
            }
        }

        private bool CheckingLanguages()
        {
            return LocalizationStorege.Languages.Length > 0;
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Receivers");
        }

        private void DrawLanguagesError(Rect rect)
        {
            this.elementHeight = 40;
            this.displayAdd = false;
            this.displayRemove = false;

            GUIStyle style = new GUIStyle();
            style.richText = true;
            style.wordWrap = true;

            GUIContent label = new GUIContent("<color=red>Before adding objects, you must add at least one language</color>",
                EditorGUIUtility.Load("icons/console.erroricon.png") as Texture2D);

            EditorGUI.LabelField(rect, label, style);
        }

        private void DrowLocalizationReceiver(Rect rect, int index, bool isActive, bool isFocused)
        {
            var objectFieldRect = new Rect(new Vector2(rect.x, rect.y), new Vector2(150, rect.height));
            var receiver = (LocalizationReceiver)list[index];
            receiver = EditorGUI.ObjectField(objectFieldRect, receiver, typeof(LocalizationReceiver), true) as LocalizationReceiver;
            if (GUI.changed && receiver != (LocalizationReceiver)list[index])
            {
                if (list.Contains(receiver))
                {
                    throw new System.ArgumentException($"There is already a localizations with {receiver.LocalizationTag.Name}-{receiver.LocalizationTag.ID}");
                }

                if ((LocalizationReceiver)list[index] != null)
                {
                    var tag = ((LocalizationReceiver)list[index]).LocalizationTag;
                    if (LocalizationStorege.Conteins(tag)) { LocalizationStorege.RemoveResource(tag); }
                }

                if (receiver != null)
                {
                    receiver.LocalizationTag = new LocalizationTag(receiver.name);
                    LocalizationStorege.AddResource(receiver.LocalizationTag, receiver.Resource);
                }
                list[index] = receiver;
            }
        }

        private void AddRecever(ReorderableList reorderable)
        {
            reorderable.list.Add(null);
        }

        private void RemoveRecever(ReorderableList reorderable)
        {
            var receiver = (LocalizationReceiver)list[index];
            if (receiver != null && LocalizationStorege.Conteins(receiver.LocalizationTag)) { LocalizationStorege.RemoveResource(receiver.LocalizationTag); }
            reorderable.list.RemoveAt(index);
        }
    }
}
