using System.Collections;
using System.IO;
using System.Linq;
using UnityExtended;

namespace ResourceLocalization
{
    public class TypeLocalizationProvider : ITypeLocalizationProvider
    {
        public void AddType(TypeLocalization newType)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable GetList()
        {
            var path = ExtendedEditor.GetDirectory($"{this.GetType().Name}.cs").Replace("/Core/Editor", "/Components");
            return (from a in Directory.GetFiles(path).Where(n => n.EndsWith(".cs")) select Path.GetFileNameWithoutExtension(a)).ToList();
        }

        public TypeLocalization[] GetTypes()
        {
            throw new System.NotImplementedException();
        }

        private void CreateComponent(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName)) { throw new System.ArgumentNullException(nameof(typeName)); }
            var path = ExtendedEditor.GetDirectory($"{this.GetType().Name}.cs").Replace("/Core/Editor", "/Components");
            if (!Directory.Exists($"{path}Editor/"))
            {
                Directory.CreateDirectory($"{path}Editor/");
            }

            ExtendedEditor.CreateClass(typeName + "Localization", path, GetComponentCode(typeName));
            ExtendedEditor.CreateClass(typeName + "LocalizationEditor", path + "Editor/", GetComponentEditorCode(typeName));
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

        private string GetComponentEditorCode(string typeName)
        {
            return
$@"
using UnityEditor;

namespace ResourceLocalization
{{
    /// <summary>
    /// Class for displaying localization fields.
    /// </summary>
    [CustomEditor(typeof({typeName}Localization))]
    public class {typeName}LocalizationEditor : LocalizationComponentEditor {{ }}
}}";
        }
    }
}
