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
			private Dictionary<string, Data> localizations;

			public string Name { get => name; set => name = value; }
			public Dictionary<string, Data> Localizations { get => localizations; }

			public Entity(string name)
			{
				this.name = name;
				localizations = new Dictionary<string, Data>();
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
			var tags = new List<Entity>();
			foreach (var language in localization.Languages)
			{
				foreach (var local in language.Resources)
				{
					bool tagExists = false;
					foreach (var tag in tags)
					{
						if (tag.Name.Equals(local.Tag))
						{
							tag.Localizations.Add(language.Name, local.Data);
							tagExists = true;
							break;
						}
					}

					if (!tagExists)
					{
						var newTag = new Entity(local.Tag);
						newTag.Localizations.Add(language.Name, local.Data);
						tags.Add(newTag);
					}
				}
			}
			return tags;
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
			var tag = (Entity)list[index];
			tag.Name = GUI.TextField(new Rect(new Vector2(rect.x, rect.y), new Vector2(86, rect.height)), tag.Name, "PR TextField");
			float dX = 86f;

			Data temp;
			foreach (var language in localization.Languages)
			{
				if (!tag.Localizations.TryGetValue(language.Name, out temp)) { temp = new Data(Data.Type.Audio); }
				if (temp.DataObject is string)
				{
					temp.DataObject = GUI.TextField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(150, rect.height)), (string)temp.DataObject, "PR TextField");
				}
				else
				{
					temp.DataObject = EditorGUI.ObjectField(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(150, rect.height)), (Object)temp.DataObject, temp.DataType, false);
				}

				if (!tag.Localizations.ContainsKey(language.Name))
				{
					tag.Localizations.Add(language.Name, temp);
				}
				else
				{
					tag.Localizations[language.Name] = temp;
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
				entity.Localizations.Add(language.Name, new Data(Data.Type.Audio));
			}
			allList.list.Add(entity);
			allList.index = allList.list.Count - 1;
			SetChanges();
		}

		private void RemoveTag(ReorderableList reorderable)
		{
			var tag = (Entity)reorderable.list[reorderable.index];
			foreach (var lang in localization.Languages)
			{
				if (lang.Resources != null)
				{
					for (int i = 0; i < lang.Resources.Count; i++)
					{
						if (lang.Resources[i].Tag.Equals(tag.Name))
						{
							lang.Resources.RemoveAt(i--);
						}
					}
				}
			}
			reorderable.list.Remove(tag);
		}

		private void SetChanges()
		{
			foreach (var language in localization.Languages)
			{
				language.Resources.Clear();
				foreach (var tag in (List<Entity>)list)
				{
					if (tag.Localizations.ContainsKey(language.Name))
					{
						//Debug.Log($"{tag.Name} - {tag.Localizations[language.Name]}");
						language.Resources.Add(new LocalizationResource(tag.Name, tag.Localizations[language.Name]));
					}
				}
			}
			EditorUtility.SetDirty(localization);
		}
	}
}