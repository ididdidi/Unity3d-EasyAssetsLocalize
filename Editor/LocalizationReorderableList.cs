using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ResourceLocalization
{
	public class LocalizationReorderableList : ReorderableList
	{
		private class Element
		{
			public string Tag { get; set; }
			public Dictionary<string, object> Localizations { get; }

			public Element(string name)
			{
				Tag = name;
				Localizations = new Dictionary<string, object>();
			}

			public void DrawOnGUI(Rect rect)
			{
				this.Tag = GUI.TextField(new Rect(new Vector2(rect.x, rect.y), new Vector2(86, rect.height)), this.Tag, "PR TextField");
				float dX = 86f;

				string[] languages = new string[Localizations.Count]; 
				Localizations.Keys.CopyTo(languages,0);

				foreach (var language in languages)
				{
					var value = Localizations[language];
					if (value.GetType().IsAssignableFrom(typeof(string)))
					{
						value = GUI.TextField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(150, rect.height)), (string)value, "PR TextField");
					}
					else
					{
						value = EditorGUI.ObjectField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(150, rect.height)), (Object)value, value.GetType(), false);
					}
					dX += 150f;
					Localizations[language] = value;
				}
			}
		}

		private LocalizationStorage LocalizationStorage { get; }

		public LocalizationReorderableList(LocalizationStorage localizationStorage) : base(ExtractElements(localizationStorage), typeof(Element), true, true, true, true)
		{
			this.LocalizationStorage = localizationStorage;

			elementHeight = 18;

			drawHeaderCallback = DrawLanguageNames;

			drawElementCallback = DrawElement;

			onAddCallback = AddNewElement;

			onRemoveCallback = RemoveElement;

			onReorderCallbackWithDetails = ReorderList;
		}

		private static List<Element> ExtractElements(LocalizationStorage localizationStorage)
		{
			var resources = new List<Element>();
			foreach (var localization in localizationStorage.Localizations)
			{
				foreach (var local in localization.Dictionary)
				{
					bool resourceExists = false;
					foreach (var resource in resources)
					{
						if (resource.Tag.Equals(local.Key))
						{
							resource.Localizations.Add(localization.Language, local.Value);
							resourceExists = true;
							break;
						}
					}

					if (!resourceExists)
					{
						var newTag = new Element(local.Key);
						newTag.Localizations.Add(localization.Language, local.Value);
						resources.Add(newTag);
					}
				}
			}
			return resources;
		}

		private void DrawLanguageNames(Rect rect)
		{
			float dX = 40f;
			GUI.Label(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(rect.width, rect.height)), "Tags");
			dX = 100f;

			var count = 0;
			foreach (var localization in LocalizationStorage.Localizations)
			{
				localization.Language = GUI.TextField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(130, rect.height)), localization.Language, "TextField");
				dX += 130f;
				GUIContent iconButton = EditorGUIUtility.TrIconContent("Toolbar Minus", "Delete language");
				if (GUI.Button(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(18, rect.height)), iconButton, "SearchCancelButton"))
				{
					LocalizationStorage.RemoveLocalization(localization.Language);
					break;
				}
				dX += 20f;
				count++;
			}

			GUIContent icon = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add language");
			if (GUI.Button(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(18, rect.height)), icon, "RL FooterButton"))
			{
				LocalizationStorage.AddLocalization("Language " + (count + 1));
			}

			if (GUI.changed)
			{
				this.list = ExtractElements(LocalizationStorage);
			}
		}

		private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			((Element)list[index]).DrawOnGUI(rect);
			if (GUI.changed)
			{
				foreach (var localization in LocalizationStorage.Localizations)
				{
					foreach (var resource in (List<Element>)list)
					{
						localization.SetValue(resource.Tag, resource.Localizations[localization.Language]);
					}
				}
			}
		}

		private void ReorderList(ReorderableList list, int oldIndex, int newIndex)
		{
			foreach(var localization in LocalizationStorage.Localizations)
			{
				var tag = localization.GetTag(oldIndex);
				var value = localization.GetValue(oldIndex);
				localization.RemoveAt(oldIndex);
				localization.Insert(newIndex, tag, value);
			}
			this.list = ExtractElements(LocalizationStorage);
		}

		private void AddNewElement(ReorderableList reorderable)
		{
			foreach (var localization in LocalizationStorage.Localizations)
			{
				localization.SetValue("Tag " + (reorderable.list.Count + 1), "");
			}

			list = ExtractElements(LocalizationStorage);
		}

		private void RemoveElement(ReorderableList reorderable)
		{
			var resource = (Element)reorderable.list[reorderable.index];
			foreach (var localization in LocalizationStorage.Localizations)
			{
				localization.Remove(resource.Tag);
			}
			list = ExtractElements(LocalizationStorage);
		}
	}
}