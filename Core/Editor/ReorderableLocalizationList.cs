using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityExtended;

namespace ResourceLocalization
{
	/// <summary>
	/// Display localization storage data in a reorderable list.
	/// </summary>
	public class ReorderableLocalizationList : ReorderableList
	{
		private readonly Vector2 padding = new Vector2(1f, 1f);
		private readonly float fieldWidth = 150f;
		private readonly float fieldHeight = 18f;

		private LocalizationSearch search;
		private int storageVersion;

		private LocalizationStorage LocalizationStorage { get; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="localizationStorage">LocalizationStorage</param>
		public ReorderableLocalizationList(LocalizationStorage localizationStorage) : base(localizationStorage.Localizations, typeof(Localization), true, true, true, true)
		{
			this.LocalizationStorage = localizationStorage;

			storageVersion = localizationStorage.Version;

			elementHeight = fieldHeight + padding.y * 2f;

			drawHeaderCallback = DrawHeader;

			drawElementCallback = DrawResources;
		
			onReorderCallbackWithDetails = ReorderList;
			
			onAddCallback = AddLocalisatrion;
			
			onRemoveCallback = RemoveLocalisatrion;

			search = new LocalizationSearch(LocalizationStorage);
		}

		/// <summary>
		/// Calculates the required window size.
		/// </summary>
		/// <returns>Minimum window dimensions</returns>
		public Vector2 GetSize()
		{
			return new Vector2((LocalizationStorage.Languages.Length + 1) * (fieldWidth + 2f) + 12f, 320f);
		}

		/// <summary>
		/// Method for displaying the contents of a spike in the Inspector window.
		/// </summary>
		public new void DoLayoutList()
		{
			if(storageVersion != LocalizationStorage.Version)
			{
				list = search.GetResult();
				storageVersion = LocalizationStorage.Version;
			}
			base.DoLayoutList();
		}
		#region Header
		/// <summary>
		/// Display the title in which the search string is located and the list of supported languages.
		/// </summary>
		/// <param name="rect"><see cref="Rect"/></param>
		private void DrawHeader(Rect rect)
		{
			float dX = DrawSearchField(rect, -4f);
			dX = DrowLanguages(rect, dX);

			AddNewLanguageButton(rect, dX);
		}

		/// <summary>
		/// Display a field for searching by tags.
		/// </summary>
		/// <param name="rect"><see cref="Rect"/></param>
		/// <param name="dX">Offset X</param>
		/// <returns>New offset X</returns>
		private float DrawSearchField(Rect rect, float dX)
		{
			var width = fieldWidth - 6f;
			if (search.SearchFieldChanged(ExtendedEditorGUI.GetNewRect(rect, new Vector2(width, rect.height), padding, dX)))
			{
				list = search.GetResult();
			}
			return dX + width;
		}

		/// <summary>
		/// Display Language fields.
		/// </summary>
		/// <param name="rect"><see cref="Rect"/></param>
		/// <param name="dX">Offset X</param>
		/// <returns>New offset X</returns>
		private float DrowLanguages(Rect rect, float dX)
		{
			var languages = LocalizationStorage.Languages;
			for (int i = 0; i < languages.Length; i++)
			{
				var width = fieldWidth - 20f;
				languages[i].Name = GUI.TextField(ExtendedEditorGUI.GetNewRect(rect, new Vector2(width, rect.height), padding, dX), languages[i].Name, "TextField");
				dX += width;

				// Delete language button. Inactive if there is only one language.
				EditorGUI.BeginDisabledGroup(languages.Length < 2);
				if (ExtendedEditorGUI.CancelButton(ExtendedEditorGUI.GetNewRect(rect, new Vector2(fieldHeight, rect.height), padding, dX), "Delete language"))
				{
					if (EditorUtility.DisplayDialog("Delete this language?",
		"Are you sure that this language is not used anywhere and you want to delete it?", "Yes Delete", "Do Not Delete"))
					{
						LocalizationStorage.RemoveLanguage(languages[i]);
						break;
					}
				}
				EditorGUI.EndDisabledGroup();
				dX += fieldHeight + padding.y * 2;
			}
			return dX;
		}

		/// <summary>
		/// Display a button to add a new language.
		/// </summary>
		/// <param name="rect"><see cref="Rect"/></param>
		/// <param name="dX">Offset X</param>
		private void AddNewLanguageButton(Rect rect, float dX)
		{
			GUIContent icon = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add language");
			if (GUI.Button(ExtendedEditorGUI.GetNewRect(rect, new Vector2(fieldHeight, rect.height), padding, dX), icon, "RL FooterButton"))
			{
				LocalizationStorage.AddLanguage(new Language("Language " + (LocalizationStorage.Languages.Length + 1)));
			}
		}
		#endregion

		/// <summary>
		/// Display a tag of lits associated resources.
		/// </summary>
		/// <param name="rect"><see cref="Rect"/></param>
		/// <param name="index">The index of the tag in the list</param>
		private void DrawResources(Rect rect, int index, bool isActive, bool isFocused)
		{
			var localsation = list[index] as Localization;
			var width = fieldWidth - 24;
			GUI.Label(ExtendedEditorGUI.GetNewRect(rect, new Vector2(width, rect.height), padding), localsation.Name);
			float dX = width;

			for (int i = 0; i < localsation.Resources.Count; i++)
			{
		//		if (typeof(string).IsAssignableFrom(localsation.Resources[i].Type))
		//		{
		//			localsation.Resources[i].Data = GUI.TextField(ExtendedEditorGUI.GetNewRect(rect, new Vector2(fieldWidth - 8f, rect.height), padding, dX),
		//				(string)localsation.Resources[i].Data, "PR TextField");
		//		}
		//		else
		//		{
		//			localsation.Resources[i].Data = EditorGUI.ObjectField(ExtendedEditorGUI.GetNewRect(rect, new Vector2(fieldWidth - 8f, rect.height), padding, dX),
		//				(Object)localsation.Resources[i].Data, localsation.Resources[i].Type, false);
		//		}
				dX += fieldWidth;
			}
		}

		/// <summary>
		/// Changes the order of elements in a list.
		/// </summary>
		/// <param name="reorderable">Mutable list</param>
		/// <param name="oldIndex">Old list item index</param>
		/// <param name="newIndex">New list item index</param>
		private void ReorderList(ReorderableList reorderable, int oldIndex, int newIndex)
		{
			var localization = reorderable.list[index] as Localization;
			LocalizationStorage.RemoveLocalization(oldIndex);
			LocalizationStorage.InsertLocalization(newIndex, localization);
		}

		/// <summary>
		/// Add localization button.
		/// </summary>
		/// <param name="reorderable"></param>
		private void AddLocalisatrion(ReorderableList reorderable)
		{
			var window = (LocalizationCreateWindow)EditorWindow.GetWindow(typeof(LocalizationCreateWindow), false, "Create Localization");
			window.LocalizationStorage = LocalizationStorage;
		}

		/// <summary>
		/// Delete localization button.
		/// </summary>
		/// <param name="reorderable"></param>
		private void RemoveLocalisatrion(ReorderableList reorderable)
		{
			if (EditorUtility.DisplayDialog("Delete this localization?",
	"Are you sure that this localization is not used anywhere and you want to delete it?", "Yes Delete", "Do Not Delete"))
			{
				LocalizationStorage.RemoveLocalization(index);
			}
		}
	}
}