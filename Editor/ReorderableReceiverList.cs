using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;

namespace ResourceLocalization
{
    public class ReorderableReceiverList : ReorderableList
    {
        private readonly float padding = 1f;
        private LocalizationStorage LocalizationStorage { get; }
        public ReorderableReceiverList(List<LocalizationReceiver> receivers, LocalizationStorage localizationStorege) : base(receivers, typeof(LocalizationReceiver))
        {
            LocalizationStorage = localizationStorege;

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
            return LocalizationStorage.Languages.Length > 0;
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
            var receiver = (LocalizationReceiver)list[index];
            var objectFieldRect = GetNewRect(rect, new Vector2(rect.width - 70, rect.height));
            
            receiver = EditorGUI.ObjectField(objectFieldRect, receiver, typeof(LocalizationReceiver), true) as LocalizationReceiver;
            
            if (GUI.changed && receiver != (LocalizationReceiver)list[index]) { SetReceiver(receiver); }
            if (receiver != null) { EditResourcesButton(GetNewRect(rect, new Vector2(56f, rect.height), rect.width - 60f), receiver); }
        }

        private void AddRecever(ReorderableList reorderable)
        {
            reorderable.list.Add(null);
        }

        private void RemoveRecever(ReorderableList reorderable)
        {
            var receiver = (LocalizationReceiver)list[index];
            if (receiver != null && LocalizationStorage.Conteins(receiver.LocalizationTag)) { LocalizationStorage.RemoveResource(receiver.LocalizationTag); }
            reorderable.list.RemoveAt(index);
        }

        private void SetReceiver(LocalizationReceiver receiver)
        {
            if (list.Contains(receiver))
            {
                throw new System.ArgumentException($"There is already a localizations with {receiver.LocalizationTag.Name}-{receiver.LocalizationTag.ID}");
            }

            if ((LocalizationReceiver)list[index] != null)
            {
                var tag = ((LocalizationReceiver)list[index]).LocalizationTag;
                if (LocalizationStorage.Conteins(tag)) { LocalizationStorage.RemoveResource(tag); }
            }

            if (receiver != null)
            {
                receiver.LocalizationTag = new LocalizationTag(receiver.name);
                LocalizationStorage.AddResource(receiver.LocalizationTag, receiver.Resource);
            }
            list[index] = receiver;
        }

        private void EditResourcesButton(Rect rect, LocalizationReceiver receiver)
        {
            if (GUI.Button(rect, "Edit"))
            {
                LocalizationReceiverWindow window = (LocalizationReceiverWindow)EditorWindow.GetWindow(typeof(LocalizationReceiverWindow));
                window.LocalizationReceiver = receiver;
            }
        }

        private Rect GetNewRect(Rect rect, Vector2 size, float dX = 0f, float dY = 0f)
        {
            return new Rect(new Vector2(rect.x + dX + padding, rect.y + dY + padding), new Vector2(size.x - padding * 2f, size.y - padding * 2f));
        }
    }
}
