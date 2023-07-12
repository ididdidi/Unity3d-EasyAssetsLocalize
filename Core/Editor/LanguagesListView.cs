using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ResourceLocalization
{
    public class LanguagesListView : ReorderableList
    {
        private LocalizationStorage storage;
        private int lastIndex =-1;

        public LanguagesListView(LocalizationStorage storage) : base(storage.Languages, typeof(Language), true, true, true, true)
        {
            this.storage = storage;
            drawHeaderCallback = DrawHeader;
            drawElementCallback = DrawLanguage;
            onAddCallback = AddLanguage;
            onRemoveCallback = RemoveLanguage;
        }

        private void DrawHeader(Rect position)
        {
            EditorGUI.LabelField(position, "Languages");

            if (list.Count > 1) { displayRemove = true; }
            else { displayRemove = false; }
        }

        private void DrawLanguage(Rect position, int index, bool isActive, bool isFocused)
        {
            position.width = 160f;
            var language = (Language)list[index];
            if (isActive)
            {
                GUI.SetNextControlName("LanguageField");
                language.Name = EditorGUI.TextField(position, language.Name ?? $"Language-{index + 1}");
                if (isFocused || lastIndex != index)
                {
                    EditorGUI.FocusTextInControl("LanguageField");
                    lastIndex = index;
                }
            }
            else
            {
                position.x += 2f;
                position.y -= 1f;
                EditorGUI.LabelField(position, language.Name);
            }
        }

        private void AddLanguage(ReorderableList reorderable)
        {
            storage.AddLanguage(new Language($"Language-{reorderable.count}"));
            reorderable.index = reorderable.count - 1;

            EditorUtility.SetDirty(storage);
        }

        private void RemoveLanguage(ReorderableList reorderable)
        {
            storage.RemoveLanguage(reorderable.index);
            EditorGUI.FocusTextInControl(null);
            EditorUtility.SetDirty(storage);
        }
    }
}
