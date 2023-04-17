using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;
using static ResourceLocalization.LocalizationReceiver;

namespace ResourceLocalization
{
    public class ReorderableReceiverList : ReorderableList
    {
        private IEnumerable<Localization> Localizations { get; }
        public ReorderableReceiverList(List<LocalizationReceiver> receivers, IEnumerable<Localization> localizations) : base(receivers, typeof(LocalizationReceiver))
        {
            Localizations = localizations;
            onAddCallback = AddRecever;
            drawElementCallback = DrowLocalizationReceiver;
        }

        private void DrowLocalizationReceiver(Rect rect, int index, bool isActive, bool isFocused)
        {
            var resiver = (LocalizationReceiver)list[index];
            var objectFieldRect = new Rect(new Vector2(rect.x, rect.y), new Vector2(150, rect.height));
            ChecDragAndDrops(objectFieldRect, typeof(IReceiver));
            resiver.Object = GetReceiver(objectFieldRect, resiver.Object);
            if (GUI.changed && resiver.Object != null)
            {
             //   foreach (var localization in Localizations)
             //   {
             //       if (!localization.Conteins(resiver.ID))
             //       {
             //           localization.Add(resiver.ID, resiver.Object.ResourceType);
             //       }
             //       else if (!localization.GetDataType(resiver.ID).Equals(resiver.Object.ResourceType))
             //       {
             //           localization.SetValue(resiver.ID, resiver.Object.Data);
             //       }
             //   }
            }
        }

        private IReceiver GetReceiver(Rect position, IReceiver receiver)
        {
            var @object = receiver as Object;
            // Start blocking change checks
            EditorGUI.BeginChangeCheck();
            // Display a ObjectField
            @object = EditorGUI.ObjectField(position, @object, typeof(Object), true);
            // If changes were made to the contents of the field and a GameObject was added to the field
            if (EditorGUI.EndChangeCheck() && @object is GameObject gameobject)
            {
                // Get component of the required type on the object and save a reference to it in a property
                receiver = gameobject.GetComponent<IReceiver>();
            }
            return receiver;
        }

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


        private void AddRecever(ReorderableList reorderable)
        {
            var receiver = new LocalizationReceiver();
            reorderable.list.Add(receiver);
        }
    }
}
