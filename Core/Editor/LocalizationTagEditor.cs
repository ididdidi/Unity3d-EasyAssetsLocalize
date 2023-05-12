using UnityEditor;

namespace ResourceLocalization
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class LocalizationTagEditor : Editor
    {
        private LocalizationResourceView resourceView;
        private bool foldout;

        public virtual void OnEnable()
        {
            var tag = target as LocalizationTag;
            var storage = FindObjectOfType<LocalizationController>().LocalizationStorage;
            resourceView = new LocalizationResourceView(storage, tag);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (foldout = EditorGUILayout.Foldout(foldout, "Resources"))
            {
                resourceView.DrawResources();
            }
        }
    }
}