using System.IO;
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
        public TypeMetadata(System.Type type)
        {
            Type = type;
            if (typeof(string).IsAssignableFrom(type)) { Texture = EditorGUIUtility.IconContent("Text Icon").image; }
            else if (typeof(ScriptableObject).IsAssignableFrom(type)) { Texture = EditorGUIUtility.IconContent("ScriptableObject Icon").image; }
            else { Texture = EditorGUIUtility.ObjectContent(null, type).image; }
        }
    }

    public static class TypesMetaProvider
    {
        public static void AddType(System.Type type)
        {
            CreateComponent(type);
        }

        public static void RemoveType(TypeMetadata metadata)
        {
            var fileName = $"{metadata.Type.Name}LocalizationEditor.cs";

            var path = ExtendedEditor.GetDirectory(fileName);
            if (!string.IsNullOrEmpty(path))
            {
                File.Delete($"{path}{fileName}");
                File.Delete($"{path.Replace("/Editor", "")}{fileName.Replace("Editor", "")}");
                AssetDatabase.Refresh();
            }
            else throw new System.ArgumentNullException(fileName);
        }

        public static TypeMetadata[] GetTypesMeta()
        {
            var baseType = typeof(LocalizationComponentEditor);
            Assembly assembly = baseType.Assembly;

            var path = ExtendedEditor.GetDirectory($"{baseType.Name}.cs").Replace("Core", "Components");
            var types = (from p in Directory.GetFiles(path).Where(n => n.EndsWith(".cs"))
                         select assembly.GetType($"{baseType.Namespace}.{Path.GetFileNameWithoutExtension(p)}")).ToArray();

            return (from t in types.Where(n => n?.GetCustomAttribute<TypeMetadata>() != null)
                    select t.GetCustomAttribute<TypeMetadata>()).ToArray();
        }

        private static void CreateComponent(System.Type type)
        {
            if (type == null) { throw new System.ArgumentNullException(nameof(type)); }
            var path = ExtendedEditor.GetDirectory($"{typeof(LocalizationComponentEditor).Name}.cs").Replace("/Core/Editor", "/Components");
            if (!Directory.Exists($"{path}Editor/"))
            {
                Directory.CreateDirectory($"{path}Editor/");
            }

            ExtendedEditor.CreateClass(type.Name + "Localization", path, GetComponentCode(type));
            ExtendedEditor.CreateClass(type.Name + "LocalizationEditor", path + "Editor/", GetComponentEditorCode(type));
        }

        private static string GetComponentCode(System.Type type)
        {
            return (type.Namespace.Equals("UnityEngine") ? "" : $"using {type.Namespace};") + $@"
using UnityEngine;
using UnityEngine.Events;

namespace ResourceLocalization
{{
    public class {type.Name}Localization : LocalizationComponent
    {{
        [System.Serializable] public class Handler : UnityEvent<{type.Name}> {{ }}
        [SerializeField, HideInInspector] private Handler handler;

        public override System.Type Type => typeof({type.Name});

        public override void SetLocalizationData(object data) => handler?.Invoke(({type.Name})data);
    }}
}}";
        }

        private static string GetComponentEditorCode(System.Type type)
        {
            return (type.Namespace.Equals("UnityEngine") ? "" : $"using {type.Namespace};") + $@"
using UnityEngine;
using UnityEditor;

namespace ResourceLocalization
{{
    /// <summary>
    /// Class for displaying localization fields.
    /// </summary>
    [CustomEditor(typeof({type.Name}Localization)), TypeMetadata(typeof({type.Name}))]
    public class {type.Name}LocalizationEditor : LocalizationComponentEditor {{ }}
}}";
        }
    }
}
