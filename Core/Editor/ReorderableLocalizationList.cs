using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ResourceLocalization
{
	public class ReorderableLocalizationList : ReorderableList
	{
		private readonly float padding = 1f;
		private readonly float fieldWidth = 150f;
		private readonly float fieldHeight = 18f;

		private LocalizationSearch search;
		private int storageVersion;

		private LocalizationStorage LocalizationStorage { get; }

		public ReorderableLocalizationList(LocalizationStorage localizationStorage) : base(localizationStorage.Localizations, typeof(Localization), true, true, false, false)
		{
			this.LocalizationStorage = localizationStorage;

			storageVersion = localizationStorage.Version;

			elementHeight = fieldHeight + padding * 2f;

			drawHeaderCallback = DrawLanguageNames;

			drawElementCallback = DrawResources;
		
			onReorderCallbackWithDetails = ReorderList;

			search = new LocalizationSearch(LocalizationStorage);
		}

		public Vector2 GetSize()
		{
			return new Vector2((LocalizationStorage.Languages.Length + 1) * (fieldWidth + 2f) + 12f, 320f);
		}

		public new void DoLayoutList()
		{
			if(storageVersion != LocalizationStorage.Version)
			{
				list = search.GetResult();
				storageVersion = LocalizationStorage.Version;
			}
			base.DoLayoutList();
		}

		private void DrawLanguageNames(Rect rect)
		{
			float dX = -4f;
			var width = fieldWidth - 6f;
			if(search.SearchFieldChanged(GetNewRect(rect, new Vector2(width, rect.height), dX)))
			{
				list = search.GetResult();
			}
			dX += width;

			var languages = LocalizationStorage.Languages;
			for (int i=0; i < languages.Length; i++)
			{
				width = fieldWidth - 20f;
				languages[i].Name = GUI.TextField(GetNewRect(rect, new Vector2(width, rect.height), dX), languages[i].Name, "TextField");
				dX += width;
				if (CancelButton(GetNewRect(rect, new Vector2(fieldHeight, rect.height), dX), "Delete language"))
				{
					LocalizationStorage.RemoveLanguage(languages[i]);
					break;
				}
				dX += fieldHeight + padding * 2;
			}

			GUIContent icon = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add language");
			if (GUI.Button(GetNewRect(rect, new Vector2(fieldHeight, rect.height), dX), icon, "RL FooterButton"))
			{
				LocalizationStorage.AddLanguage(new Language("Language " + (languages.Length + 1)));
			}
		}

		private void DrawResources(Rect rect, int index, bool isActive, bool isFocused)
		{
			var localsation = list[index] as Localization;
			var width = fieldWidth - 24;
			GUI.Label(GetNewRect(rect, new Vector2(width, rect.height)), localsation.Name);
			float dX = width;

			for (int i = 0; i < localsation.Resources.Count; i++)
			{
				if (typeof(string).IsAssignableFrom(localsation.Resources[i].Type))
				{
					localsation.Resources[i].Data = GUI.TextField(GetNewRect(rect, new Vector2(fieldWidth - 8f, rect.height), dX), (string)localsation.Resources[i].Data, "PR TextField");
				}
				else
				{
					localsation.Resources[i].Data = EditorGUI.ObjectField(GetNewRect(rect, new Vector2(fieldWidth - 8f, rect.height), dX), (Object)localsation.Resources[i].Data, localsation.Resources[i].Type, false);
				}
				dX += fieldWidth;
			}

			if (CancelButton(GetNewRect(rect, new Vector2(fieldHeight, rect.height), dX), "Delete localization"))
			{
				if(EditorUtility.DisplayDialog("Delete this localization?",
					"Are you sure that this localization is not used anywhere and you want to delete it?", "Yes Delete", "Do Not Delete"))
				{
					LocalizationStorage.RemoveLocalization(index);
				}
			}
		}

		private void ReorderList(ReorderableList list, int oldIndex, int newIndex)
		{
			var localization = list.list[index] as Localization;
			LocalizationStorage.RemoveLocalization(oldIndex);
			LocalizationStorage.InsertLocalization(newIndex, localization);
		}

		private Rect GetNewRect(Rect rect, Vector2 size, float dX = 0f, float dY = 0f)
		{
			return new Rect(new Vector2(rect.x + dX + padding, rect.y + dY + padding), new Vector2(size.x - padding * 2f, size.y - padding * 2f));
		}

		private bool CancelButton(Rect rect, string label = null)
		{
			GUIContent iconButton = EditorGUIUtility.TrIconContent("Toolbar Minus", label);
			if (GUI.Button(rect, iconButton, "SearchCancelButton")) { return true; }
			return false;
		}
	}
}