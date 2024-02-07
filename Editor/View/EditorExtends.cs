using UnityEditor;
using UnityEngine;

namespace EasyAssetsLocalize
{
    /// <summary>
    /// Class that complements the capabilities of the editor GUI
    /// </summary>
    internal static class EditorExtends
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
        /// Content getter for type.
        /// </summary>
        /// <param name="type"><see cref="System.Type"/></param>
        /// <returns><see cref="GUIContent"/> for this type</returns>
        public static GUIContent GetContent(this System.Type type)
        {
            GUIContent content;
            if (typeof(string).IsAssignableFrom(type))
            {
                content = new GUIContent(type.Name, EditorGUIUtility.IconContent("Text Icon").image, type.ToString());
            }
            else
            {
                Texture icon;
                if (typeof(ScriptableObject).IsAssignableFrom(type)) { icon = EditorGUIUtility.IconContent("ScriptableObject Icon").image; }
                else { icon = EditorGUIUtility.ObjectContent(null, type).image; }
                content = new GUIContent(type.Name, icon, type.ToString());
            }
            return content;
        }

        /// <summary>
        /// Static method to display message in Unity3d inspector
        /// </summary>
        /// <param name="message">Message text as <see cref="string"/></param>
        /// <param name="messageType">Message type determines the icon before the message(None, Information, Warning, Error)</param>
        public static void DisplayMessage(string message, MessageType messageType = MessageType.None)
        {
            // Create content from the icon and text of the message
            GUIContent label = new GUIContent(message);
            switch (messageType)
            {
                case MessageType.Info: { label.image = EditorGUIUtility.Load("icons/console.infoicon.png") as Texture2D; break; }
                case MessageType.Warning: { label.image = EditorGUIUtility.Load("icons/console.warnicon.png") as Texture2D; break; }
                case MessageType.Error: { label.image = EditorGUIUtility.Load("icons/console.erroricon.png") as Texture2D; break; }
            }

            // Define the message display style
            var style = new GUIStyle(EditorStyles.helpBox);
            style.wordWrap = true;
            style.normal.textColor = GUI.skin.label.normal.textColor;

            // Display message
            EditorGUILayout.LabelField(label, style);
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