using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public class TypeLocalization
    {
        private const string defoultIcon = "cs Script Icon";

        [SerializeField] private string typeName;
        [SerializeField] private Texture icon;

        public TypeLocalization(string typeName, Texture icon)
        {
            if (!string.IsNullOrEmpty(typeName)) { this.typeName = typeName; }
            else { throw new System.ArgumentNullException(nameof(typeName)); }

            if (icon != null) this.icon = icon;
            else this.icon = EditorGUIUtility.IconContent(defoultIcon).image;
        }

        public TypeLocalization(string typeName, string iconName = defoultIcon) => 
            new TypeLocalization(typeName, EditorGUIUtility.IconContent(iconName).image);

        public GUIContent Content => new GUIContent(typeName, icon);
    }
}
