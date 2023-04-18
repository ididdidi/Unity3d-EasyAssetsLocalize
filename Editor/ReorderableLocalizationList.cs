using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ResourceLocalization
{
	public class ReorderableLocalizationList : ReorderableList
	{
		private class LocalizedResources
		{
			public string Name { get; set; }
			public System.Type Type { get; }
			public Dictionary<string, Resource> Localizations { get; }

			public LocalizedResources(string name, System.Type type)
			{
				Name = name;
				Type = type;
				Localizations = new Dictionary<string, Resource>();
			}

			public void DrawOnGUI(Rect rect)
			{
				this.Name = GUI.TextField(new Rect(new Vector2(rect.x, rect.y), new Vector2(86, rect.height)), this.Name, "PR TextField");
				float dX = 86f;

				string[] languages = new string[Localizations.Count]; 
				Localizations.Keys.CopyTo(languages,0);

				foreach (var language in languages)
				{
					var resource = Localizations[language];
					if (typeof(string).IsAssignableFrom(resource.Type))
					{
						resource.Data = GUI.TextField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(150, rect.height)), (string)resource.Data, "PR TextField");
					}
					else
					{
						resource.Data = EditorGUI.ObjectField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(150, rect.height)), (Object)resource.Data, resource.Type, false);
					}
					dX += 150f;
					Localizations[language] = resource;
				}
			}
		}

		private LocalizationStorage LocalizationStorage { get; }

		public ReorderableLocalizationList(LocalizationStorage localizationStorage) : base(ExtractResourcess(localizationStorage), typeof(LocalizedResources), true, true, true, true)
		{
			this.LocalizationStorage = localizationStorage;

			elementHeight = 18;

			drawHeaderCallback = DrawLanguageNames;

			drawElementCallback = DrawResources;

			onAddCallback = AddNewResources;

			onRemoveCallback = RemoveResources;

			onReorderCallbackWithDetails = ReorderList;
		}

		private static List<LocalizedResources> ExtractResourcess(LocalizationStorage localizationStorage)
		{
			var resources = new List<LocalizedResources>();
			foreach (var localization in localizationStorage.Localizations)
			{
				foreach (var local in localization.Dictionary)
				{
					bool resourceExists = false;
					foreach (var resource in resources)
					{
						if (resource.Name.Equals(local.Key))
						{
							resource.Localizations.Add(localization.Language, local.Value);
							resourceExists = true;
							break;
						}
					}

					if (!resourceExists)
					{
						var newResource = new LocalizedResources(local.Key, local.Value.Type);
						newResource.Localizations.Add(localization.Language, local.Value);
						resources.Add(newResource);
					}
				}
			}
			return resources;
		}

		private void DrawLanguageNames(Rect rect)
		{
			float dX = 40f;
			GUI.Label(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(rect.width, rect.height)), "Resources");
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
				this.list = ExtractResourcess(LocalizationStorage);
			}
		}

		private void DrawResources(Rect rect, int index, bool isActive, bool isFocused)
		{
			((LocalizedResources)list[index]).DrawOnGUI(rect);
			if (GUI.changed)
			{
				foreach (var localization in LocalizationStorage.Localizations)
				{
					foreach (var resource in (List<LocalizedResources>)list)
					{
						localization.SetValue(resource.Localizations[localization.Language]);
					}
				}
			}
		}

		private void ReorderList(ReorderableList list, int oldIndex, int newIndex)
		{
			foreach(var localization in LocalizationStorage.Localizations)
			{
				var resource = localization.GetResource(oldIndex);
				localization.RemoveAt(oldIndex);
				localization.Insert(newIndex, resource);
			}
			this.list = ExtractResourcess(LocalizationStorage);
		}

		private void AddNewResources(ReorderableList reorderable)
		{
			var resource = new ImageResource("Name " + (reorderable.list.Count + 1), UnityEngine.Resources.Load<Texture2D>("avatar"));
			foreach (var localization in LocalizationStorage.Localizations)
			{
				localization.SetValue(resource.Clone());
			}

			list = ExtractResourcess(LocalizationStorage);
		}

		private void RemoveResources(ReorderableList reorderable)
		{
			var resource = (LocalizedResources)reorderable.list[reorderable.index];
			foreach (var localization in LocalizationStorage.Localizations)
			{
				localization.Remove(resource.Name);
			}
			list = ExtractResourcess(LocalizationStorage);
		}
	}
}