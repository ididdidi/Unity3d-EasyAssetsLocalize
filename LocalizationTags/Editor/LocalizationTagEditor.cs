using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    public abstract class LocalizationTagEditor : Editor
    {
        private LocalizationTag tag;
        private LocalizationStorage storage;
        private int storageVersion;

        private Dictionary<string, Resource> dictionary = new Dictionary<string, Resource>();
        public bool localizationfoldout;

        public virtual void OnEnable()
        {
            tag = target as LocalizationTag;
            storage = FindObjectOfType<LocalizationController>().LocalizationStorage;
            storageVersion = storage.Version;

            UpdateDictionary();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawResources();
        }

        private void DrawResources()
        {
            if (storageVersion != storage?.Version) { UpdateDictionary(); }

            if (dictionary == null)
            {
                EditorGUILayout.HelpBox(new GUIContent("LocalizationController"));
                return;
            }

            if (localizationfoldout = EditorGUILayout.Foldout(localizationfoldout, "Localizations"))
            {
                foreach (var resource in dictionary)
                {
                    LocalizationStorageEditor.DrawResource(resource.Key, resource.Value);
                }
            }
        }

        private void UpdateDictionary()
        {
            dictionary.Clear();
            try
            {
                var localization = storage?.GetLocalization(tag.ID);
                for (int i = 0; i < storage.Languages.Length; i++)
                {
                    dictionary.Add(storage.Languages[i].Name, localization.Resources[i]);
                }
            }
            catch { }
        }
    }
}