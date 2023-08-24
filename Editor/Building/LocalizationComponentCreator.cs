namespace EasyAssetsLocalize
{
    public class LocalizationComponentPrototype
    {
        private System.Type type;

        public LocalizationComponentPrototype(System.Type type)
        {
            this.type = type ?? throw new System.ArgumentNullException(nameof(type));
        }

        public string Code
        {
            get => $@"
using UnityEngine.Events;

namespace {GetType().Namespace}
{{
    public class {type.Name}Localization : LocalizationComponent
    {{
        [System.Serializable] public class Handler : UnityEvent<{type.FullName}> {{ }}
        [UnityEngine.SerializeField, UnityEngine.HideInInspector] private Handler handler;

        public override System.Type Type => typeof({type.FullName});

        public override void SetData(object data) => handler?.Invoke(({type.FullName})data);
    }}
}}";
        }
    }
}
