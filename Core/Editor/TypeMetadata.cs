using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using EasyLocalization;

namespace EasyLocalization
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class TypeMetadata : System.Attribute
    {
        public System.Type Type { get; private set; }
        public Texture Icon { get; private set; }
        public TypeMetadata(System.Type type)
        {
            Type = type;
            if (typeof(string).IsAssignableFrom(type))
            {
                Icon = EditorGUIUtility.IconContent("Text Icon").image;
            }
            else
            {
                if (typeof(ScriptableObject).IsAssignableFrom(type)) { Icon = EditorGUIUtility.IconContent("ScriptableObject Icon").image; }
                else { Icon = EditorGUIUtility.ObjectContent(null, type).image; }
            }
        }

        public static TypeMetadata[] GetAllMetadata()
        {
            var baseType = typeof(LocalizationComponentEditor);
            Assembly assembly = baseType.Assembly;

            var path = ExtendedEditor.GetDirectory($"{baseType.Name}.cs").Replace("Core", "Components");
            var types = (from p in Directory.GetFiles(path).Where(n => n.EndsWith(".cs"))
                         select assembly.GetType($"{baseType.Namespace}.{Path.GetFileNameWithoutExtension(p)}")).ToArray();

            return (from t in types.Where(n => n?.GetCustomAttribute<TypeMetadata>() != null)
                    select t.GetCustomAttribute<TypeMetadata>()).ToArray();
        }
    }
}
