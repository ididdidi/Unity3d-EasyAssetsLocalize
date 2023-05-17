using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;
using UnityExtended;

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
                drawElementCallback = DrowLocalizationTag;
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
            EditorGUI.LabelField(rect, "Localization tags");
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

        private void DrowLocalizationTag(Rect rect, int index, bool isActive, bool isFocused)
        {
            var tag = (LocalizationTag)list[index];
            var objectFieldRect = GetNewRect(rect, new Vector2(rect.width - 70, rect.height));

            tag = ExtendedEditorGUI.ObjectField(objectFieldRect, tag, typeof(LocalizationTag), null, (item) => { return !list.Contains(item); }) as LocalizationTag;

            
            if (GUI.changed && tag != (LocalizationTag)list[index]) { SetReceiver(tag, index); }
            if (tag != null) { EditResourcesButton(GetNewRect(rect, new Vector2(56f, rect.height), rect.width - 60f), tag); }
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

        private void EditResourcesButton(Rect rect, LocalizationTag tag)
        {
            if (GUI.Button(rect, "Edit")) { DisplayTagWindow(tag); }
        }

        private void DisplayTagWindow(LocalizationTag tag)
        {
            LocalizationTagWindow window = (LocalizationTagWindow)EditorWindow.GetWindow(typeof(LocalizationTagWindow), false, tag.name);

            window.ResourceView = new LocalizationResourceView(LocalizationStorage, tag);
        }

        private Rect GetNewRect(Rect rect, Vector2 size, float dX = 0f, float dY = 0f)
        {
            return new Rect(new Vector2(rect.x + dX + padding, rect.y + dY + padding), new Vector2(size.x - padding * 2f, size.y - padding * 2f));
        }
    }
}
