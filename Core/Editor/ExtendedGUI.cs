using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EasyLocalization
{
    /// <summary>
    /// Class that complements the capabilities of the editor GUI
    /// </summary>
    public partial class ExtendedEditor
    {
        /// <summary>
        /// Field to store GUI.changed parameter
        /// </summary>
        private static System.Collections.Generic.Stack<bool> isGUIChanged = new System.Collections.Generic.Stack<bool>();

        /// <summary>
        /// Method for which allows GUI changes to be ignored.
        /// </summary>
        public static void BeginIgnoreChanges()
        {
            if (isGUIChanged.Count > 10) { throw new System.Exception("Too many nested BeginIgnoreChanges() calls"); }
            isGUIChanged.Push(GUI.changed);
            GUI.changed = false;
        }

        /// <summary>
        /// The method returns before the GUI changes are triggered.
        /// </summary>
        public static void EndIgnoreChanges()
        {
            if (isGUIChanged.Count > 0) {  GUI.changed = isGUIChanged.Pop(); }
            else { throw new System.Exception("EndIgnoreChanges() was caused more often than BeginIgnoreChanges()"); }
        }

        /// <summary>
        /// Draws a line of the given color and thickness in the inspector.
        /// </summary>
        /// <param name="color">line color</param>
        /// <param name="height">line thickness</param>
        public static void DrawLine(Color color, float height = 1f)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            rect.height = height;
            EditorGUI.DrawRect(rect, color);

        }

        /// <summary>
        /// Drawing the button in the center of the layout
        /// </summary>
        /// <param name="label">Button label</param>
        /// <param name="width">Button width</param>
        /// <param name="height">Button height</param>
        /// <returns>Was the button pressed</returns>
        public static bool CenterButton(string label, float width = 240f, float height = 24f)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var result = GUILayout.Button(label, GUILayout.Width(width), GUILayout.Height(height));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return result;
        }

        /// <summary>
        /// Simple button with a cross
        /// </summary>
        /// <param name="rect"><see cref="Rect"/></param>
        /// <param name="tooltip">Tooltip on hover</param>
        /// <returns>Button is pressed</returns>
        public static bool CancelButton(Rect rect, string tooltip = null)
        {
            GUIContent iconButton = EditorGUIUtility.TrIconContent("Toolbar Minus", tooltip);
            if (GUI.Button(rect, iconButton, "SearchCancelButton")) { return true; }
            return false;
        }

        /// <summary>
        /// Displays the search bar
        /// </summary>
        /// <param name="position">Field position</param>
        /// <param name="keyword">Search keyword</param>
        /// <param name="controlID">Field control ID</param>
        /// <returns>New search keyword</returns>
        public static string SearchField(Rect position, string keyword, out int controlID)
        {
            var @event = Event.current;
            var buttonSize = EditorGUIUtility.singleLineHeight;
            Rect rect = position;
            rect.width -= buttonSize / 2;

            keyword = EditorGUI.TextField(rect, GUIContent.none, keyword, EditorStyles.toolbarSearchField);
            controlID = GetLastControlId();

            rect.x += rect.width;
            rect.width = buttonSize;
            // Clear search keyword
            if (keyword != string.Empty && CancelButton(rect) || (@event.type == EventType.KeyDown && @event.keyCode == KeyCode.Escape))
            {
                keyword = string.Empty;
                GUIUtility.keyboardControl = 0;
            }
            // Unfocus search field
            else
            {
                UnfocusOnClick(controlID, @event);
            }
            return keyword;
        }

        /// <summary>
        /// Removes focus from the field when clicking elsewhere in the editor window.
        /// </summary>
        /// <param name="controlID">Field control id</param>
        /// <param name="event">Current event</param>
        public static void UnfocusOnClick(int controlID, Event @event = null)
        {
            var current = @event ?? Event.current;
            if (controlID != 0 && current.type == EventType.MouseUp)
            {
                if (controlID != GUIUtility.hotControl && controlID == GUIUtility.keyboardControl)
                {
                    GUIUtility.keyboardControl = 0;
                    EditorGUIUtility.editingTextField = false;
                    current.Use();
                }
            }
        }

        /// <summary>
        /// Return last control ID setted in GUI
        /// </summary>
        /// <returns>Last control ID setted</returns>
        public static int GetLastControlId()
        {
            FieldInfo getLastControlId = typeof(EditorGUIUtility).GetField("s_LastControlID", BindingFlags.Static | BindingFlags.NonPublic);
            if (getLastControlId != null)
                return (int)getLastControlId.GetValue(null);
            return 0;
        }

        /// <summary>
        /// Simple deletion confirmation dialog.
        /// </summary>
        /// <param name="name">Name of the object to be deleted</param>
        /// <returns>Yes or No as bool value</returns>
        public static bool DeleteConfirmation(string name) 
            => EditorUtility.DisplayDialog($"Delete {name}", $"Are you sure want to delete {name}?", "Yes", "No");
    }
}