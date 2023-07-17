﻿using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityExtended;

namespace ResourceLocalization
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class TypeMetadata : System.Attribute
    {
        public System.Type Type { get; private set; }
        public Texture Texture { get; private set; }
        public TypeMetadata(System.Type type, string ediorIcon)
        {
            this.Type = type;
            this.Texture = EditorGUIUtility.IconContent(ediorIcon).image;
        }
    }

    public class TypesMetaProvider
    {
        public void AddType(string typeName, string iconName = "cs Script Icon")
        {
            CreateComponent(typeName, iconName);
        }

        public TypeMetadata[] GetTypesMeta()
        {
            var baseType = typeof(LocalizationComponentEditor);
            Assembly assembly = baseType.Assembly;

            var path = ExtendedEditor.GetDirectory($"{this.GetType().Name}.cs").Replace("Core", "Components");
            var types = (from p in Directory.GetFiles(path).Where(n => n.EndsWith(".cs"))
                         select assembly.GetType($"{baseType.Namespace}.{Path.GetFileNameWithoutExtension(p)}")).ToArray();

            return (from t in types.Where(n => n?.GetCustomAttribute<TypeMetadata>() != null)
                    select t.GetCustomAttribute<TypeMetadata>()).ToArray();
        }

        private void CreateComponent(string typeName, string iconName = "cs Script Icon")
        {
            if (string.IsNullOrWhiteSpace(typeName)) { throw new System.ArgumentNullException(nameof(typeName)); }
            var path = ExtendedEditor.GetDirectory($"{this.GetType().Name}.cs").Replace("/Core/Editor", "/Components");
            if (!Directory.Exists($"{path}Editor/"))
            {
                Directory.CreateDirectory($"{path}Editor/");
            }

            ExtendedEditor.CreateClass(typeName + "Localization", path, GetComponentCode(typeName));
            ExtendedEditor.CreateClass(typeName + "LocalizationEditor", path + "Editor/", GetComponentEditorCode(typeName, iconName));
        }

        private string GetComponentCode(string typeName)
        {
            return
$@"
using UnityEngine;
using UnityEngine.Events;

namespace ResourceLocalization
{{
    public class {typeName}Localization : LocalizationComponent
    {{
        [System.Serializable] public class Handler : UnityEvent<{typeName}> {{ }}
        [SerializeField, HideInInspector] private Handler handler;

        public override System.Type Type => typeof({typeName});

        public override void SetLocalizationData(object data) => handler?.Invoke(({typeName})data);
    }}
}}";
        }

        private string GetComponentEditorCode(string typeName, string iconName)
        {
            return
$@"
using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{{
    /// <summary>
    /// Class for displaying localization fields.
    /// </summary>
    [CustomEditor(typeof({typeName}Localization)), TypeMetadata(typeof({typeName}), ""{iconName}"")]
    public class {typeName}LocalizationEditor : LocalizationComponentEditor {{ }}
}}";
        }
    }
}