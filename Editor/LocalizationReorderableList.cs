﻿using System.Collections.Generic;
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

		public LocalizationData localization;

		public LocalizationReorderableList(LocalizationData localizationData) : base(LocalizationDataAdapter(localizationData), typeof(Entity), true, true, true, true)
		{
			localization = localizationData;

			elementHeight = 18;

			drawHeaderCallback = DrawLanguageNames;

			drawElementCallback = DrawTag;

			onAddCallback = AddNewTag;

			onRemoveCallback = RemoveTag;
		}

		private static List<Entity> LocalizationDataAdapter(LocalizationData localization)
		{
			var entities = new List<Entity>();
			foreach (var language in localization.Languages)
			{
				foreach (var local in language.Resources)
				{
					bool entityExists = false;
					foreach (var entity in entities)
					{
						if (entity.Name.Equals(local.Tag))
						{
							entity.Localizations.Add(language.Name, local.Data);
							entityExists = true;
							break;
						}
					}

					if (!entityExists)
					{
						var newTag = new Entity(local.Tag);
						newTag.Localizations.Add(language.Name, local.Data);
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
				localization.Languages[i].Name = GUI.TextField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(130, rect.height)), localization.Languages[i].Name, "TextField");
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
				localization.Languages.Add(new LanguageData("Language " + (localization.Languages.Count + 1)));
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
				if (entity.Localizations.ContainsKey(language.Name)) { temp = entity.Localizations[language.Name]; }
				
				if (temp.GetType().IsAssignableFrom(typeof(string)))
				{
					temp = GUI.TextField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(150, rect.height)), (string)temp, "PR TextField");
				}
				else
				{
					temp = EditorGUI.ObjectField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(150, rect.height)), (Object)temp, temp.GetType(), false);
				}

				if (!entity.Localizations.ContainsKey(language.Name))
				{
					entity.Localizations.Add(language.Name, temp);
				}
				else
				{
					entity.Localizations[language.Name] = temp;
				}
				dX += 150f;

				if (GUI.changed)
				{
					SetChanges();
				}
			}
		}

		private void AddNewTag(ReorderableList allList)
		{
			var entity = new Entity("Tag " + (allList.list.Count + 1));
			foreach (var language in localization.Languages)
			{
				entity.Localizations.Add(language.Name, "");
			}
			allList.list.Add(entity);
			allList.index = allList.list.Count - 1;
			SetChanges();
		}

		private void RemoveTag(ReorderableList reorderable)
		{
			var entity = (Entity)reorderable.list[reorderable.index];
			foreach (var lang in localization.Languages)
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
			foreach (var language in localization.Languages)
			{
				language.Resources.Clear();
				foreach (var entity in (List<Entity>)list)
				{
					if (entity.Localizations.ContainsKey(language.Name))
					{
						language.Resources.Add(new Resource(entity.Name, entity.Localizations[language.Name]));
					}
				}
			}
			EditorUtility.SetDirty(localization);
		}
	}
}