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

        private void DrowLocalizationTag(Rect rect, int index, bool isActive, bool isFocused)
        {
            var tag = (LocalizationTag)list[index];
            var objectFieldRect = GetNewRect(rect, new Vector2(rect.width - 70, rect.height));

            ChecDragAndDrops(objectFieldRect, typeof(LocalizationTag));
            DrawTagField(objectFieldRect, ref tag);

            
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
            if (GUI.Button(rect, "Edit"))
            {
                LocalizationTagWindow window = (LocalizationTagWindow)EditorWindow.GetWindow(typeof(LocalizationTagWindow), false, tag.name);
                window.LocalizationReceiver = tag;
            }
        }

        private Rect GetNewRect(Rect rect, Vector2 size, float dX = 0f, float dY = 0f)
        {
            return new Rect(new Vector2(rect.x + dX + padding, rect.y + dY + padding), new Vector2(size.x - padding * 2f, size.y - padding * 2f));
        }


        /// <summary>
        /// Checks the validity of the dragged objects
        /// </summary>
        /// <param name="position"><see cref="Rect"/> fields to activate validation</param>
        /// <param name="requiredType">Required <see cref="System.Type"/> of reference being checked</param>
        private void ChecDragAndDrops(Rect position, System.Type requiredType)
        {
            // If the cursor is in the area of the rendered field
            if (position.Contains(Event.current.mousePosition))
            {
                // Iterate over all draggable references
                foreach (var @object in DragAndDrop.objectReferences)
                {
                    // If we do not find the required type
                    if (!IsValidObject(@object, requiredType))
                    {
                        // Disable drag and drop
                        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if an reference matches the required type
        /// </summary>
        /// <param name="object">Checked reference</param>
        /// <param name="requiredType">Required <see cref="System.Type"/> of reference being checked</param>
        /// <returns></returns>
        private bool IsValidObject(Object @object, System.Type requiredType)
        {
            // If the object is a GameObject
            if (@object is GameObject go)
            {
                // Check if it has a component of the required type and return result
                return go.GetComponent(requiredType) != null;
            }

            // Check the reference itself for compliance with the required type
            return requiredType.IsAssignableFrom(@object.GetType());
        }

        /// <summary>
        /// Display a field for adding a reference to an object
        /// </summary>
        /// <param name="position"><see cref="Rect"/> fields to activate validation</param>
        /// <param name="property">Serializedproperty the object</param>
        /// <param name="label">Displaу field label</param>
        private void DrawTagField(Rect position, ref LocalizationTag localizationTag)
        {
            // Start change checks
            EditorGUI.BeginChangeCheck();
            // Display a ObjectField
            var @object = EditorGUI.ObjectField(position, localizationTag, typeof(object), true);
            // If changes were made to the contents of the field and a GameObject was added to the field
            if (EditorGUI.EndChangeCheck() && @object is GameObject gameObject)
            {
                // Get component of the required type on the object and save a reference to it in a property
                foreach (var tag in gameObject.GetComponents(typeof(LocalizationTag)))
                {
                    if(!list.Contains(tag)) { @object = tag; break; }
                }
            }
            localizationTag = @object as LocalizationTag;
        }

    }
}
