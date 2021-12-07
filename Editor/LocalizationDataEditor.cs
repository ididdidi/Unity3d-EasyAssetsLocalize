using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ru.mofrison.Unity3d
{

	[CustomEditor(typeof(LocalizationData))]
	public class LocalizationDataEditor : Editor
	{
		[System.Serializable]
		private class Tag
		{
			private string name;
			private Dictionary<string, string> localizations;

			public string Name { get => name; set => name = value; }
			public Dictionary<string, string> Localizations { get => localizations; }

			public Tag(string name)
			{
				this.name = name;
				localizations = new Dictionary<string, string>();
			}
		}

		private LocalizationData localization;
		private ReorderableList tagsList;
		float widthTag = 100f;
		float widthPlus = 40f;

		void OnEnable()
		{
			localization = this.target as LocalizationData;

			tagsList = new ReorderableList(ExtractionData(), typeof(Tag), true, true, true, true);

			tagsList.drawHeaderCallback = DrawLanguageNames;

			tagsList.drawElementCallback = DrawTag;

			tagsList.onAddCallback = AddNewTag;

			tagsList.onRemoveCallback = RemoveTag;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			tagsList.DoLayoutList();
		}

		private List<Tag> ExtractionData()
		{
			var tags = new List<Tag>();
			foreach (var language in localization.Languages)
			{
				foreach (var local in language.Resources)
				{
					bool tagExists = false;
					for (int i=0; i < tags.Count; i++)
					{
						if (tags[i].Name.Equals(local.Tag))
						{
							tags[i].Localizations.Add(language.Name, local.StringData);
							tagExists = true;
							break;
						}
					}

					if (!tagExists)
					{
						var newTag = new Tag(local.Tag);
						newTag.Localizations.Add(language.Name, local.StringData);
						tags.Add(newTag);
					}
				}
			}
			return tags;
		}

		private void DrawLanguageNames(Rect rect)
		{
			float averageWidth = (rect.width - (widthTag + widthPlus + localization.LanguageNames.Count*20f + 16f)) / localization.LanguageNames.Count;
			float widthLanguage = (averageWidth > 130f) ? averageWidth : 130f;
			
			GUIStyle boldStyle = new GUIStyle(GUI.skin.GetStyle("label"))
			{
				alignment = TextAnchor.LowerCenter,
				fontStyle = FontStyle.Bold
			};
			float dX = rect.x;
			GUI.Label(new Rect(new Vector2(rect.x, rect.y), new Vector2(widthTag + dX, rect.height)), "Tags", boldStyle);
			dX += widthTag + 16f;

			for (int i = 0; i < localization.Languages.Count; i++)
			{
				localization.Languages[i].Name = GUI.TextField(new Rect(new Vector2(dX, rect.y), new Vector2(widthLanguage, rect.height)), localization.Languages[i].Name, "TextField");
				dX += widthLanguage;
				GUIContent iconButton = EditorGUIUtility.TrIconContent("Toolbar Minus", "Delete language");
				if (GUI.Button(new Rect(new Vector2(dX, rect.y), new Vector2(18, rect.height)), iconButton, "SearchCancelButton"))
				{
					localization.Languages.RemoveAt(i--);
				}
				dX += 20f;
			}

			GUIContent icon = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add language");
			if (GUI.Button(new Rect(new Vector2(dX, rect.y), new Vector2(18, rect.height)), icon, "RL FooterButton"))
			{
				localization.Languages.Add(new LanguageData("Language " + (localization.Languages.Count + 1)));
			}

			if (GUI.changed)
			{
				tagsList.list = ExtractionData();
			}
		}

		private void DrawTag(Rect rect, int index, bool isActive, bool isFocused)
		{
			float averageWidth = (rect.width - (widthTag + widthPlus)) / localization.LanguageNames.Count;
			float widthLanguage = (averageWidth > 150f) ? averageWidth : 150f;

			var tag = (Tag)tagsList.list[index];
			if (isActive)
			{
				tag.Name = GUI.TextField(new Rect(new Vector2(rect.x, rect.y - 6), new Vector2(widthTag, rect.height)), tag.Name, "PR TextField");
				float dX = rect.x + widthTag;

				string temp;
				foreach (var language in localization.Languages)
				{
					if (!tag.Localizations.TryGetValue(language.Name, out temp)) { temp = ""; }
					temp = GUI.TextField(new Rect(new Vector2(dX, rect.y - 6), new Vector2(widthLanguage, rect.height)), temp, "PR TextField");
					if (!string.IsNullOrWhiteSpace(temp))
					{
						if (!tag.Localizations.ContainsKey(language.Name))
						{
							tag.Localizations.Add(language.Name, temp);
						}
						else
						{
							tag.Localizations[language.Name] = temp;
						}
					}
					else if (tag.Localizations.ContainsKey(language.Name))
					{
						tag.Localizations.Remove(language.Name);
					}
					dX += widthLanguage;

					if (GUI.changed)
					{
						SetChanges();
					}
				}
			}
			else
			{
				GUI.Label(new Rect(new Vector2(rect.x, rect.y), new Vector2(widthTag, rect.height)), tag.Name);
				float dX = rect.x + widthTag;
				foreach (var language in localization.Languages)
				{
					if (tag.Localizations.ContainsKey(language.Name))
					{
						GUI.Label(new Rect(new Vector2(dX, rect.y), new Vector2(widthLanguage, rect.height)), tag.Localizations[language.Name]);
					}
					dX += widthLanguage;
				}
			}
		}

		private void AddNewTag(ReorderableList allList)
		{
			allList.list.Add(new Tag("Tag " + (allList.list.Count + 1)));
			allList.index = allList.list.Count - 1;
		}

		private void RemoveTag(ReorderableList reorderable)
		{
			var tag = (Tag)reorderable.list[reorderable.index];
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
			SetChanges();
		}

		private void SetChanges()
		{
			foreach (var language in localization.Languages)
			{
				language.Resources.Clear();
				var tags = (List<Tag>)tagsList.list;
				for (int i=0; i < tags.Count; i++)
				{
					if (tags[i].Localizations.ContainsKey(language.Name))
					{
						language.Resources.Add(new LocalizationResource(tags[i].Name, tags[i].Localizations[language.Name]));
					}
				}
			}
			EditorUtility.SetDirty(localization);
			serializedObject.ApplyModifiedProperties();
		}

		private void OnDisable()
		{
			foreach (var language in localization.Languages)
			{
				var tags = (List<Tag>)tagsList.list;
				for (int i = 0; i < tags.Count; i++)
				{
					if (!tags[i].Localizations.ContainsKey(language.Name))
					{
						Debug.LogWarning(string.Format("The {0} doesn't have a value for the {1}.", language.Name, tags[i].Name));
					}
				}
			}
		}
	}
}
