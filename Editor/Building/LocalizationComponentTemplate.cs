namespace EasyAssetsLocalize
{
    /// <summary>
    /// A class that contains a code template for creating a component.
    /// </summary>
    internal class LocalizationComponentTemplate
    {
        private System.Type type;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">resource type</param>
        public LocalizationComponentTemplate(System.Type type)
        {
            this.type = type ?? throw new System.ArgumentNullException(nameof(type));
        }
        
        /// <summary>
        /// Code to create class file.
        /// </summary>
        public string Code
        {
            get => $@"
using UnityEngine.Events;

namespace {GetType().Namespace}
{{
    [UnityEngine.AddComponentMenu(""Localize/{type.Name} Localization"")]
    public class {type.Name}Localization : LocalizationComponent
    {{
        [System.Serializable] public class Handler : UnityEvent<{type.FullName}> {{ }}
        [UnityEngine.SerializeField, UnityEngine.HideInInspector] private Handler handler;

        public override System.Type Type => typeof({type.FullName});

        protected override void SetData(object data) => handler?.Invoke(({type.FullName})data);
    }}
}}";
        }
    }
}
