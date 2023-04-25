using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    [CustomEditor(typeof(LocalizationReceiver))]
    public class LocalizationReceiverEditor : Editor
    {
        private LocalizationStorage LocalizationStorage { get; set; }
        private Dictionary<string, Resource> resources;
        private bool foldout;

        public virtual void OnEnable()
        {
            var receiver = target as LocalizationReceiver;
            LocalizationStorage = FindObjectOfType<LocalizationController>().LocalizationStorage;
            resources = LocalizationStorage?.GetResources(receiver.LocalizationTag);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawResources();
        }

        private void DrawResources()
        {
            if(resources == null)
            {
                EditorGUILayout.HelpBox(new GUIContent("LocalizationController"));
                return;
            }

            if (foldout = EditorGUILayout.Foldout(foldout, "Localizations"))
            {
                foreach (var resource in resources)
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