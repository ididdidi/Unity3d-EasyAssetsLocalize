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
		private NoticeView noticeView;
		private SearchTreeView searchView;
		private LocalizationView localizationView;
		private LocalizationSettingsView settingsView;
		private int storageVersion;
		private static LocalizationStorage LocalizationStorage { get => LocalizationManager.LocalizationStorage; }
		public bool ShowSettings { get; set; }

		private void OnEnable()
		{
			noticeView = new NoticeView(this);
			localizationView = new LocalizationView(LocalizationStorage, noticeView);
			var provider = new LocalizationSearchProvider(LocalizationStorage, 
				(tag) => { ShowSettings = false; localizationView.Data = tag; return false; }, 
				(tag) => localizationView.Data = tag);
			searchView = new SearchTreeView(this, provider);
			settingsView = new LocalizationSettingsView(this);
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
				ShowSettings = true;
			}

			rect.x = 319;
			rect.width = position.width - 320;
			EditorGUI.DrawRect(rect, Background);
			if (!ShowSettings)
			{
				localizationView?.OnGUI(rect);
			}
			else
			{
				settingsView?.OnGUI(rect);
			}

			noticeView.OnGUI();
		}

		private bool SettingsButton(Rect rect)
		{
			return GUI.Button(rect, EditorGUIUtility.IconContent("_Popup"), GUIStyle.none);
		}
	}
}