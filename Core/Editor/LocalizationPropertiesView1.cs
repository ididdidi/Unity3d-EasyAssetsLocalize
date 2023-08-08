using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
    public class LocalizationPropertiesView1 : IEditorView
    {
        private readonly Color LightSkin = new Color(0.77f, 0.77f, 0.77f);
        private readonly Color DarkSkin = new Color(0.22f, 0.22f, 0.22f);

        private Color Background => EditorGUIUtility.isProSkin ? DarkSkin : LightSkin;

        public void OnGUI(Rect position)
        {
            EditorGUI.DrawRect(position, Background);
        }
    }
}
