using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    public abstract class LocalizationTagEditor : Editor
    {
        private LocalizationStorage LocalizationStorage { get; set; }
        private Dictionary<string, Resource> dictionary = new Dictionary<string, Resource>();
        public bool foldout;

        public virtual void OnEnable()
        {
            var receiver = target as LocalizationTag;
            LocalizationStorage = FindObjectOfType<LocalizationController>().LocalizationStorage;
            var localization = LocalizationStorage?.GetLocalization(receiver.ID);

            for (int i=0; i < LocalizationStorage.Languages.Length; i++)
            {
                dictionary.Add(LocalizationStorage.Languages[i].Name, localization.Resources[i]);
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawResources();
        }

        private void DrawResources()
        {
            if(dictionary == null)
            {
                EditorGUILayout.HelpBox(new GUIContent("LocalizationController"));
                return;
            }

            if (foldout = EditorGUILayout.Foldout(foldout, "Localizations"))
            {
                foreach (var resource in dictionary)
                {
                    LocalizationStorageEditor.DrawResource(resource.Key, resource.Value);
                }
            }
        }
    }
}