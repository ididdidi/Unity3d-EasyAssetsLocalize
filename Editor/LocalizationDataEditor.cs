using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

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

		public Tag(string name) {
			this.name = name;
			localizations = new Dictionary<string, string>();
		}
	}

	private LocalizationData localization;
	private ReorderableList tagsList;

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
				foreach (var tag in tags)
				{
					if (tag.Name.Equals(local.Tag))
					{
						tag.Localizations.Add(language.Name, local.StringData);
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
			tagsList.list = ExtractionData();
		}
	}

	private void DrawTag(Rect rect, int index, bool isActive, bool isFocused)
	{
		var tag = (Tag)tagsList.list[index];
		if (isActive)
		{
			tag.Name = GUI.TextField(new Rect(new Vector2(rect.x, rect.y - 6), new Vector2(86, rect.height)), tag.Name, "PR TextField");
			float dX = 86f;

			string temp;
			foreach (var language in localization.Languages)
			{
				if (!tag.Localizations.TryGetValue(language.Name, out temp)) { temp = ""; }
				temp = GUI.TextField(new Rect(new Vector2(rect.x + dX, rect.y - 6), new Vector2(150, rect.height)), temp, "PR TextField");
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
				dX += 150f;

				if (GUI.changed)
				{
					SetChanges();
				}
			}
		}
		else
		{
			GUI.Label(new Rect(new Vector2(rect.x, rect.y), new Vector2(86, rect.height)), tag.Name);
			float dX = 86f;
			foreach (var language in localization.Languages)
			{
				if (tag.Localizations.ContainsKey(language.Name))
				{
					GUI.Label(new Rect(new Vector2(rect.x + dX, rect.y), new Vector2(150, rect.height)), tag.Localizations[language.Name]);
				}
				dX += 150f;
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
	}

	private void SetChanges()
	{
		foreach (var language in localization.Languages)
		{
			language.Resources.Clear();
			foreach (var tag in (List<Tag>)tagsList.list)
			{
				if (tag.Localizations.ContainsKey(language.Name))
				{
					language.Resources.Add(new LocalizationResource(tag.Name, tag.Localizations[language.Name]));
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
			foreach (var tag in (List<Tag>)tagsList.list)
			{
				if (!tag.Localizations.ContainsKey(language.Name))
				{
					Debug.LogWarning(string.Format("The {0} doesn't have a value for the {1}.", language.Name, tag.Name));
				}
			}
		}
	}
}
