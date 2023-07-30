﻿using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace ResourceLocalization
{
	/// <summary>
	/// Display localization storage data in a separate inspector window.
	/// </summary>
	public class LocalizationStorageWindow : EditorWindow
	{
		public const float MIN_WIDTH = 720f;
		public const float MIN_HIGHT = 320f;

		private readonly Color LightSkin = new Color(0.77f, 0.77f, 0.77f);
		private readonly Color DarkSkin = new Color(0.22f, 0.22f, 0.22f);

		private Color Background => EditorGUIUtility.isProSkin ? DarkSkin : LightSkin;

		// Data renderer in a editor window
		private IView curentView;
		private NoticeView noticeView;
		private TypePreview typePreview;
		private SearchTreeView searchView;
		private LocalizationView localizationView;
		private LocalizationSettingsView settingsView;
		private int storageVersion;
		private static LocalizationStorage LocalizationStorage { get => LocalizationManager.Storage; }

		private void OnEnable()
		{
			typePreview = new TypePreview();
			noticeView = new NoticeView(this);
			localizationView = new LocalizationView(LocalizationStorage, noticeView);
			settingsView = new LocalizationSettingsView(this);
			var provider = new LocalizationSearchProvider(LocalizationStorage, OnSelectEntry, OnFocusEntry);
			searchView = new SearchTreeView(this, provider);
		}

		/// <summary>
		/// Creation of initialization and display of a window on the monitor screen.
		/// </summary>
		public static LocalizationStorageWindow Show(float minWidth = MIN_WIDTH, float minHight = MIN_HIGHT)
		{
			var instance = GetWindow<LocalizationStorageWindow>(LocalizationStorage.name);

			instance.minSize = new Vector2(minWidth, minHight);

			return instance;
		}

		/// <summary>
		/// Method for rendering window content
		/// </summary>
		internal void OnGUI()
		{
			if(searchView != null && storageVersion != LocalizationStorage.Version)
			{
				searchView.IsChanged = true;
				storageVersion = LocalizationStorage.Version;
			}

			var rect = new Rect(0, 0, 320, position.height);
			EditorGUI.DrawRect(rect, Background);
			if (LocalizationManager.Languages.Length > 0) searchView?.OnGUI(rect);

			if (string.IsNullOrEmpty(searchView?.SearchKeyword) && SettingsButton(new Rect(302, 8, 20, 20)))
			{
				curentView = settingsView;
			}

			rect.x = 319;
			rect.width = position.width - 320;
			EditorGUI.DrawRect(rect, Background);
			GUI.Label(rect, GUIContent.none, "grey_border");

			curentView?.OnGUI(rect);
			noticeView.OnGUI();
		}

		private bool SettingsButton(Rect rect)
		{
			return GUI.Button(rect, EditorGUIUtility.IconContent("_Popup"), GUIStyle.none);
		}

		private bool OnSelectEntry(object data)
		{
			if (data is LocalizationTag localization)
			{
				localizationView.Data = localization;
				curentView = localizationView;
			}
			return false;
		}

		private void OnFocusEntry(object data)
		{
			if (data is GUIContent content)
			{
				typePreview.Content = content;
				curentView = typePreview;
			}
			else if (data is LocalizationTag localization)
			{
				localizationView.Data = localization;
				curentView = localizationView;
			}
		}
	}
}