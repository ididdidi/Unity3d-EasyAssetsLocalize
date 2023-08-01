using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SimpleLocalization
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
            onReorderCallback = ReorderLanguages;
        }

        private void DrawHeader(Rect position)
        {
            EditorGUI.LabelField(position, "Languages");

            if (list.Count > 1) { displayRemove = true; }
            else { displayRemove = false; }
        }

        private void DrawLanguage(Rect position, int index, bool isActive, bool isFocused)
        {
            var rect = new Rect(position);
            rect.width = 156f;
            rect.x -= 2f;
            rect.y += 1f;
            var language = (Language)list[index];
            language.SystemLanguage = (SystemLanguage)EditorGUI.EnumPopup(rect, language.SystemLanguage);

            if (isFocused || lastIndex != index) { lastIndex = index; }
        }

        private void AddLanguage(ReorderableList reorderable)
        {
            storage.AddLanguage(new Language(SystemLanguage.Unknown));
            reorderable.index = reorderable.count - 1;
            EditorUtility.SetDirty(storage);
        }

        private void RemoveLanguage(ReorderableList reorderable)
        {
            storage.RemoveLanguage(reorderable.index--);
            EditorGUI.FocusTextInControl(null);
            EditorUtility.SetDirty(storage);
        }

        private void ReorderLanguages(ReorderableList list)
        {
            storage.ReorderLocalizations(lastIndex, list.index);
            EditorUtility.SetDirty(storage);
        }
    }
}
