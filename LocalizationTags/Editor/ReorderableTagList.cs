using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;

namespace ResourceLocalization
{
    public class ReorderableTagList : ReorderableList
    {
        private readonly float padding = 1f;
        private LocalizationStorage LocalizationStorage { get; }
        public ReorderableTagList(List<LocalizationTag> receivers, LocalizationStorage localizationStorege) : base(receivers, typeof(LocalizationTag))
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
            var receiver = (LocalizationTag)list[index];
            var objectFieldRect = GetNewRect(rect, new Vector2(rect.width - 70, rect.height));
            
            receiver = EditorGUI.ObjectField(objectFieldRect, receiver, typeof(LocalizationTag), true) as LocalizationTag;
            
            if (GUI.changed && receiver != (LocalizationTag)list[index]) { SetReceiver(receiver, index); }
            if (receiver != null) { EditResourcesButton(GetNewRect(rect, new Vector2(56f, rect.height), rect.width - 60f), receiver); }
        }

        private void AddRecever(ReorderableList reorderable)
        {
            reorderable.list.Add(null);
        }

        private void RemoveRecever(ReorderableList reorderable)
        {
            var receiver = (LocalizationTag)list[index];
            if (receiver != null && LocalizationStorage.ConteinsLocalization(receiver.ID)) { LocalizationStorage.RemoveLocalization(receiver.ID); }
            reorderable.list.RemoveAt(index);
        }

        private void SetReceiver(LocalizationTag receiver, int index)
        {
            if (list.Contains(receiver))
            {
                throw new System.ArgumentException($"There is already a localizations with {receiver.name}-{receiver.ID}");
            }

            if ((LocalizationTag)list[index] != null)
            {
                var id = ((LocalizationTag)list[index]).ID;
                if (LocalizationStorage.ConteinsLocalization(id)) { LocalizationStorage.RemoveLocalization(id); }
            }

            if (receiver != null)
            {
                receiver.ID = LocalizationStorage.AddLocalization(receiver.name, receiver.Resource);
            }
            list[index] = receiver;
        }

        private void EditResourcesButton(Rect rect, LocalizationTag receiver)
        {
            if (GUI.Button(rect, "Edit"))
            {
                LocalizationTagWindow window = (LocalizationTagWindow)EditorWindow.GetWindow(typeof(LocalizationTagWindow));
                window.LocalizationReceiver = receiver;
            }
        }

        private Rect GetNewRect(Rect rect, Vector2 size, float dX = 0f, float dY = 0f)
        {
            return new Rect(new Vector2(rect.x + dX + padding, rect.y + dY + padding), new Vector2(size.x - padding * 2f, size.y - padding * 2f));
        }
    }
}
