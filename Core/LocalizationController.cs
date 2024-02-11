using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EasyAssetsLocalize
{
    [ExecuteAlways, AddComponentMenu("Localize/Localization Controller")]
    public class LocalizationController : MonoBehaviour
    {
        [SerializeField] private LocalizationStorage localStorage;

        [System.Serializable] public class Handler : UnityEvent<string> { }
        [SerializeField] private Handler OnChangingLanguage;

        private void Reset() => localStorage = LocalizationManager.Storage as LocalizationStorage;

        private void OnValidate() => LocalizationManager.SetStorage(localStorage ?? LocalizationManager.Storage);

        // Set language value at start
        private void Start()
        {
            LocalizationManager.OnLanguageChange += OnLanguageChange;
            if (localStorage) { LocalizationManager.SetStorage(localStorage); }
        }

        private void OnLanguageChange(Language language) => OnChangingLanguage?.Invoke(language.ToString());

        /// <summary>
        /// Changes the language to the next one in the localization list.
        /// </summary>
        public void SetNextLanguage() => ChangeLocalzation(Direction.Next);

        /// <summary>
        /// Changes the language to the previous one in the list of localizations.
        /// </summary>
        public void SetPrevLanguage() => ChangeLocalzation(Direction.Back);

        private enum Direction { Next = 1, Back = -1 }
        /// <summary>
        /// Method for changing localization language. Switching is carried out in a circle.
        /// </summary>
        /// <param name="direction">Indicates which language to select: the previous one in the list or the next one</param>
        private static void ChangeLocalzation(Direction direction)
        {
            var languages = new List<Language>(LocalizationManager.Storage.Languages);
            if (languages.Count < 2) { return; }

            int index = languages.IndexOf(LocalizationManager.Language);
            int newIndex = (index + (int)direction) % languages.Count;
            if (newIndex >= 0) { index = newIndex; }
            else { index = languages.Count + newIndex; }
            LocalizationManager.Language = languages[index];
        }

        private void OnDisable() => LocalizationManager.OnLanguageChange -= OnLanguageChange;

        private void OnDestroy() => LocalizationManager.ResetStorage();
    }
}