using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    [CustomEditor(typeof(LocalizationReceiver))]
    public abstract class LocalizationReceiverEditor : Editor
    {
        private LocalizationStorage LocalizationStorage { get; set; }
        private Dictionary<string, Resource> dictionary = new Dictionary<string, Resource>();
        public bool foldout;

        public virtual void OnEnable()
        {
            var receiver = target as LocalizationReceiver;
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
                    if (typeof(string).IsAssignableFrom(resource.Value.Type))
                    {
                        EditorGUILayout.LabelField(resource.Key);
                        resource.Value.Data = EditorGUILayout.TextArea((string)resource.Value.Data, GUILayout.Height(50f));
                    }
                    else
                    {
                        resource.Value.Data = EditorGUILayout.ObjectField(resource.Key, (Object)resource.Value.Data, resource.Value.Type, false);
                    }
                }
            }
        }
    }
}