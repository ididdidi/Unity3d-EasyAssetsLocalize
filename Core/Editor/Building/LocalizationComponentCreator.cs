
public class LocalizationComponentCreator : ClassCreator
{
    private string path;
    private System.Type type;

    public LocalizationComponentCreator(string path, System.Type type)
    {
        this.path = path ?? throw new System.ArgumentNullException(nameof(path)); ;
        this.type = type ?? throw new System.ArgumentNullException(nameof(type)); ;
    }

    public override void CreateClass()
    {
        var code = (type.Namespace.Equals("UnityEngine") ? "" : $"using {type.Namespace};") + $@"
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

        CreateClass(type.Name + "Localization", path, code);
    }
}
