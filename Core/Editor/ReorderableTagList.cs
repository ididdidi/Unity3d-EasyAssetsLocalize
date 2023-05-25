using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;
using UnityExtended;

namespace ResourceLocalization
{
    /// <summary>
    /// Display localization tags in a reorderable list.
    /// </summary>
    public class ReorderableTagList : ReorderableList
    {
        private readonly Vector2 padding = new Vector2(1f, 1f);
        private LocalizationStorage LocalizationStorage { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tags"><see cref="LocalizationTag"/></param>
        /// <param name="localizationStorege"><see cref="ResourceLocalization.LocalizationStorage"/></param>
        public ReorderableTagList(List<LocalizationTag> tags, LocalizationStorage localizationStorege) : base(tags, typeof(LocalizationTag))
        {
            LocalizationStorage = localizationStorege;

            drawHeaderCallback = DrawHeader;

            if (LocalizationStorage.Languages.Length > 0)
            {
                onAddCallback = AddTag;
                onRemoveCallback = RemoveTag;
                drawElementCallback = DrowLocalizationTag;
            }
            else
            {
                ExtendedEditorGUI.DisplayMessage("Before adding objects, you must add at least one language", MessageType.Error);
            }
        }

        /// <summary>
        /// Display the header of the list.
        /// </summary>
        /// <param name="rect"><see cref="Rect"/></param>
        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Localization tags");
        }

        /// <summary>
        /// Display a localization tag field.
        /// </summary>
        /// <param name="rect"><see cref="Rect"/></param>
        /// <param name="index">The index of the tag in the list</param>
        private void DrowLocalizationTag(Rect rect, int index, bool isActive, bool isFocused)
        {
            var tag = (LocalizationTag)list[index];
            var objectFieldRect = ExtendedEditorGUI.GetNewRect(rect, new Vector2(rect.width - 70, rect.height), padding);

            tag = ExtendedEditorGUI.ObjectField(objectFieldRect, tag, typeof(LocalizationTag), null, (item) => { return !list.Contains(item); }) as LocalizationTag;

            
            if (GUI.changed && tag != (LocalizationTag)list[index]) { SetLocalizationTag(tag, index); }
            if (tag != null) { EditResourcesButton(ExtendedEditorGUI.GetNewRect(rect, new Vector2(56f, rect.height), padding, rect.width - 60f), tag); }
        }

        /// <summary>
        /// Adds/Remove a localization tag to the list and to the <see cref="ResourceLocalization.LocalizationStorage"/>.
        /// </summary>
        /// <param name="tag"><see cref="LocalizationTag"/></param>
        /// <param name="index">The index of the tag in the list</param>
        private void SetLocalizationTag(LocalizationTag tag, int index)
        {
            if ((LocalizationTag)list[index] != null)
            {
                var id = ((LocalizationTag)list[index]).ID;
                if (LocalizationStorage.ConteinsLocalization(id)) { LocalizationStorage.RemoveLocalization(id); }
            }

            if (tag != null)
            {
                tag.ID = LocalizationStorage.AddLocalization(tag.name, tag.Resource);
            }
            list[index] = tag;
        }

        /// <summary>
        /// Adds a new tag field to the list.
        /// </summary>
        /// <param name="reorderable"><see cref="ReorderableList"/></param>
        private void AddTag(ReorderableList reorderable)
        {
            reorderable.list.Add(null);
        }

        /// <summary>
        /// Remove a new tag field to the list.
        /// </summary>
        /// <param name="reorderable"><see cref="ReorderableList"/></param>
        private void RemoveTag(ReorderableList reorderable)
        {
            var receiver = (LocalizationTag)list[index];
            if (receiver != null && LocalizationStorage.ConteinsLocalization(receiver.ID)) { LocalizationStorage.RemoveLocalization(receiver.ID); }
            reorderable.list.RemoveAt(index);
        }

        /// <summary>
        /// Button to display tag properties in a separate window.
        /// </summary>
        /// <param name="rect"><see cref="Rect"/></param>
        /// <param name="tag"><see cref="LocalizationTag"/></param>
        private void EditResourcesButton(Rect rect, LocalizationTag tag)
        {
            if (GUI.Button(rect, "Edit")) { DisplayTagWindow(tag); }
        }

        /// <summary>
        /// Opens the properties window for the selected tag.
        /// </summary>
        /// <param name="tag"><see cref="LocalizationTag"/></param>
        private void DisplayTagWindow(LocalizationTag tag)
        {
            LocalizationTagWindow window = (LocalizationTagWindow)EditorWindow.GetWindow(typeof(LocalizationTagWindow), false, tag.name);

            window.ResourceView = new LocalizationResourceView(LocalizationStorage, tag);
        }
    }
}
