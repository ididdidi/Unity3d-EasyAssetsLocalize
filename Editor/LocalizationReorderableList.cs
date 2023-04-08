using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Localization
{
	public class LocalizationReorderableList : ReorderableList
	{
		[System.Serializable]
		private class Entity
		{
			private string name;
			private Dictionary<string, object> localizations;

			public string Name { get => name; set => name = value; }
			public Dictionary<string, object> Localizations { get => localizations; }

			public Entity(string name)
			{
				this.name = name;
				localizations = new Dictionary<string, object>();
			}
		}

		public LocalizationStorage localization;

		public LocalizationReorderableList(LocalizationStorage localizationData) : base(LocalizationDataAdapter(localizationData), typeof(Entity), true, true, true, true)
		{
			localization = localizationData;

			elementHeight = 18;

			drawHeaderCallback = DrawLanguageNames;

			drawElementCallback = DrawTag;

			onAddCallback = AddNewTag;

			onRemoveCallback = RemoveTag;

			onReorderCallback = (list) => { SetChanges(); };
		}

		private static List<Entity> LocalizationDataAdapter(LocalizationStorage localization)
		{
			var entities = new List<Entity>();
			foreach (var language in localization.Languages)
			{
				foreach (var local in localization.GetLocalizationResources(language))
				{
					bool entityExists = false;
					foreach (var entity in entities)
					{
						if (entity.Name.Equals(local.Key))
						{
							entity.Localizations.Add(language, local.Value.Data);
							entityExists = true;
							break;
						}
					}

					if (!entityExists)
					{
						var newTag = new Entity(local.Key);
						newTag.Localizations.Add(language, local.Value.Data);
						entities.Add(newTag);
					}
				}
			}
			return entities;
		}

		private void DrawLanguageNames(Rect rect)
		{
			float dX = 40f;
			GUI.Label(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(rect.width, rect.height)), "Tags");
			dX = 100f;

			for (int i = 0; i < localization.Languages.Count; i++)
			{
				localization.Languages[i] = GUI.TextField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(130, rect.height)), localization.Languages[i], "TextField");
				dX += 130f;
				GUIContent iconButton = EditorGUIUtility.TrIconContent("Toolbar Minus", "Delete language");
				if (GUI.Button(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(18, rect.height)), iconButton, "SearchCancelButton"))
				{
					localization.Languages.RemoveAt(i--);
				}
				dX += 20f;
			}

			GUIContent icon = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add language");
			if (GUI.Button(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(18, rect.height)), icon, "RL FooterButton"))
			{
				localization.localizations.Add(new Localization("Language " + (localization.Languages.Count + 1)));
			}

			if (GUI.changed)
			{
				list = LocalizationDataAdapter(localization);
			}
		}

		private void DrawTag(Rect rect, int index, bool isActive, bool isFocused)
		{
			var entity = (Entity)list[index];
			entity.Name = GUI.TextField(new Rect(new Vector2(rect.x, rect.y), new Vector2(86, rect.height)), entity.Name, "PR TextField");
			float dX = 86f;

			object temp = null;
			foreach (var language in localization.Languages)
			{
				if (entity.Localizations.ContainsKey(language)) { temp = entity.Localizations[language]; }

				if (temp.GetType().IsAssignableFrom(typeof(string)))
				{
					temp = GUI.TextField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(150, rect.height)), (string)temp, "PR TextField");
				}
				else
				{
					temp = EditorGUI.ObjectField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(150, rect.height)), (Object)temp, temp.GetType(), false);
				}

				if (!entity.Localizations.ContainsKey(language))
				{
					entity.Localizations.Add(language, temp);
				}
				else
				{
					entity.Localizations[language] = temp;
				}
				dX += 150f;

				if (GUI.changed)
				{
					SetChanges();
				}
			}
		}

		private void AddNewTag(ReorderableList reorderable)
		{
			var entity = new Entity("Tag " + (reorderable.list.Count + 1));
			foreach (var language in localization.Languages)
			{
				entity.Localizations.Add(language, "");
			}
			reorderable.list.Add(entity);
			reorderable.index = reorderable.list.Count - 1;
			SetChanges();
		}

		private void RemoveTag(ReorderableList reorderable)
		{
			var entity = (Entity)reorderable.list[reorderable.index];
			foreach (var lang in localization.localizations)
			{
				if (lang.Resources != null)
				{
					for (int i = 0; i < lang.Resources.Count; i++)
					{
						if (lang.Resources[i].Tag.Equals(entity.Name))
						{
							lang.Resources.RemoveAt(i--);
						}
					}
				}
			}
			reorderable.list.Remove(entity);
		}

		private void SetChanges()
		{
			foreach (var language in localization.localizations)
			{
				language.Resources.Clear();
				foreach (var entity in (List<Entity>)list)
				{
					if (entity.Localizations.ContainsKey(language.Language))
					{
						language.Resources.Add(new Resource(entity.Name, entity.Localizations[language.Language]));
					}
				}
			}
			EditorUtility.SetDirty(localization);
		}
	}
}