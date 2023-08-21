using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
    /// <summary>
    /// Class for visualizing the list of used languages.
    /// </summary>
    public class LanguagesListView : ReorderableList
    {
        private LocalizationStorage storage;
        private int lastIndex =-1;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="storage">Localization store instance</param>
        public LanguagesListView(LocalizationStorage storage) : base(storage.Languages, typeof(Language), true, true, true, true)
        {
            this.storage = storage;
            drawHeaderCallback = DrawHeader;
            drawElementCallback = DrawLanguage;
            onAddCallback = AddLanguage;
            onRemoveCallback = RemoveLanguage;
            onReorderCallback = ReorderLanguages;
        }

        /// <summary>
        /// Draws the title of the list of languages.
        /// </summary>
        /// <param name="position"><see cref="Rect"/> provided for drawing the header</param>
        private void DrawHeader(Rect position)
        {
            EditorGUI.LabelField(position, "Languages");

            if (list.Count > 1) { displayRemove = true; }
            else { displayRemove = false; }
        }

        /// <summary>
        /// Draws <see cref="Language"/> properties from a list
        /// </summary>
        /// <param name="position"><see cref="Rect"/>provided for rendering the language properties</param>
        /// <param name="index"><see cref="Language"/> index in the list</param>
        /// <param name="isActive">Is the list item active?</param>
        /// <param name="isFocused">Is the list item on focused?</param>
        private void DrawLanguage(Rect position, int index, bool isActive, bool isFocused)
        {
            var rect = new Rect(position);
            rect.width = 156f;
            rect.x -= 2f;
            rect.y += 1f;
            var language = (Language)list[index];
            
            EditorGUI.BeginChangeCheck();
            var systemLanguage = EditorGUI.EnumPopup(rect, language.SystemLanguage);
            if (EditorGUI.EndChangeCheck()) 
            {
                Undo.RecordObject(storage, $"Rename {language.SystemLanguage} to {systemLanguage}");
                language.SystemLanguage = (SystemLanguage)systemLanguage;
            }
            
            if (isFocused || lastIndex != index) { lastIndex = index; }
        }

        /// <summary>
        /// Adding a new language to the list.
        /// </summary>
        /// <param name="list">This <see cref="ReorderableList"/></param>
        private void AddLanguage(ReorderableList list)
        {
            Undo.RecordObject(storage, "Add new language");
            storage.AddLanguage(new Language(SystemLanguage.Unknown));
            list.index = list.count - 1;
            EditorUtility.SetDirty(storage);
        }

        /// <summary>
        /// Remove the selected <see cref="Language"/> from the list.
        /// </summary>
        /// <param name="list">This <see cref="ReorderableList"/></param>
        private void RemoveLanguage(ReorderableList list)
        {
            var language = list.list[list.index] as Language;
            if (ExtendedEditor.DeleteConfirmation(language.SystemLanguage.ToString()))
            {
                Undo.RecordObject(storage, $"Remove {language.SystemLanguage}");
                storage.RemoveLanguage(list.index--);
                EditorGUI.FocusTextInControl(null);
                EditorUtility.SetDirty(storage);
            }
        }

        /// <summary>
        /// Change the order of Languages in the list.
        /// </summary>
        /// <param name="list">This <see cref="ReorderableList"/></param>
        private void ReorderLanguages(ReorderableList list)
        {
            Undo.RecordObject(storage, "Reorder languages");
            storage.ReorderLocalizations(lastIndex, list.index);
            EditorUtility.SetDirty(storage);
        }
    }
}
