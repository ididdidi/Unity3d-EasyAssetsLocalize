using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    public abstract class LocalizationTagEditor : Editor
    {
        private LocalizationTag receiver;
        private LocalizationStorage storage;
        private int storageVersion;

        private Dictionary<string, Resource> dictionary = new Dictionary<string, Resource>();
        public bool foldout;

        public virtual void OnEnable()
        {
            receiver = target as LocalizationTag;
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
            if(storageVersion != storage?.Version) { UpdateDictionary(); }

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

        private void UpdateDictionary()
        {
            dictionary.Clear();
            var localization = storage?.GetLocalization(receiver.ID);
            for (int i = 0; i < storage.Languages.Length; i++)
            {
                dictionary.Add(storage.Languages[i].Name, localization.Resources[i]);
            }
        }
    }
}