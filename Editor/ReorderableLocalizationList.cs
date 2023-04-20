using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ResourceLocalization
{
	public class ReorderableLocalizationList : ReorderableList
	{
		private ILocalizationRepository LocalizationStorage { get; }

		public ReorderableLocalizationList(ILocalizationRepository localizationStorage) : base(localizationStorage.Localizations, typeof(Localization), true, true, true, true)
		{
			this.LocalizationStorage = localizationStorage;

			elementHeight = 18;

			drawHeaderCallback = DrawLanguageNames;

			drawElementCallback = DrawResources;

			onAddCallback = AddNewResources;
		
			onRemoveCallback = RemoveResources;
		
			onReorderCallbackWithDetails = ReorderList;
		}

		private void DrawLanguageNames(Rect rect)
		{
			float dX = 16f;
			GUI.Label(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(rect.width, rect.height)), "Resources");
			dX = 100f;

			var count = 0;
			foreach (var language in LocalizationStorage.Languages)
			{
				language.Name = GUI.TextField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(130, rect.height)), language.Name, "TextField");
				dX += 130f;
				GUIContent iconButton = EditorGUIUtility.TrIconContent("Toolbar Minus", "Delete language");
				if (GUI.Button(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(18, rect.height)), iconButton, "SearchCancelButton"))
				{
					LocalizationStorage.RemoveLanguage(language.Name);
					break;
				}
				dX += 20f;
				count++;
			}

			GUIContent icon = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add language");
			if (GUI.Button(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(18, rect.height)), icon, "RL FooterButton"))
			{
				LocalizationStorage.AddLanguage("Language " + (count + 1));
			}

			if (GUI.changed)
			{
				this.list = LocalizationStorage.Localizations;
			}
		}

		private void DrawResources(Rect rect, int index, bool isActive, bool isFocused)
		{
			var localsation = list[index] as Localization;
			GUI.Label(new Rect(new Vector2(rect.x, rect.y), new Vector2(86, rect.height)), localsation.Tag.Name);
			float dX = 86f;

			for (int i = 0; i < localsation.Resources.Count; i++)
			{
				if (typeof(string).IsAssignableFrom(localsation.Resources[i].Type))
				{
					localsation.Resources[i].Data = GUI.TextField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(150, rect.height)), (string)localsation.Resources[i].Data, "PR TextField");
				}
				else
				{
					localsation.Resources[i].Data = EditorGUI.ObjectField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(150, rect.height)), (Object)localsation.Resources[i].Data, localsation.Resources[i].Type, false);
				}
				dX += 150f;
			}
		}

		private void ReorderList(ReorderableList list, int oldIndex, int newIndex)
		{
			var localization = list.list[index] as Localization;
			LocalizationStorage.RemoveResource(oldIndex);
			LocalizationStorage.InsertResource(newIndex, localization);
		}
	
		private void AddNewResources(ReorderableList reorderable)
		{
			//LocalizationStorage.AddResource("Avayar", new ImageResource(Resources.Load<Texture2D>("avatar")));
			list = LocalizationStorage.Localizations;
		}
	
		private void RemoveResources(ReorderableList reorderable)
		{
			LocalizationStorage.RemoveResource(reorderable.index);
			list = LocalizationStorage.Localizations;
		}
	}
}