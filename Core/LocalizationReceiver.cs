using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace ResourceLocalization
{
    /// <summary>
    /// Base class for storing references to localization objects.
    /// </summary>
    [System.Serializable]
    public abstract class LocalizationReceiver
    {
        [SerializeField] private string name;
        [SerializeField] private string id;

#if UNITY_EDITOR
        [System.NonSerialized] public bool open;
#endif
        /// <summary>
        /// Identifier of the localization tag in the repository
        /// </summary>
        public string ID { get => id; set => id = value; }
        public string Name { get => name; }
        protected LocalizationReceiver(LocalizationTag localizationTag)
        {
            this.name = localizationTag.Name;
            this.id = localizationTag.ID;
        }
        public abstract void SetLocalization(object resource);
    }

    [System.Serializable]
    public abstract class LocalizationReceiver<T> : LocalizationReceiver
    {
        [System.Serializable] public class Handlers : UnityEvent<T> { }
        [SerializeField] public Handlers handlers;

        protected LocalizationReceiver(LocalizationTag localizationTag) : base(localizationTag) { }

        public override void SetLocalization(object resource)
        {
            handlers?.Invoke((T)resource);
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LocalizationReceiver))]
    public class HumanPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            var heandlers = property.FindPropertyRelative("handlers");
            EditorGUI.PropertyField(rect, heandlers, label, true);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 150f;
        }
    }
#endif
}