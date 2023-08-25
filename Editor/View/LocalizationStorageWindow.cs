﻿using UnityEditor;
using UnityEngine;
using EasyAssetsLocalize;

namespace EasyAssetsLocalize
{
	/// <summary>
	/// Display localization storage data in a separate inspector window.
	/// </summary>
	public class LocalizationStorageWindow : EditorWindow, IDisplay
	{
		public const float MIN_WIDTH = 240f;
		public const float MIN_HIGHT = 320f;

		// Data renderer in a editor window
		private NoticeView noticeView;
		private SearchTreeView searchView;
		private LocalizationView localizationView;
		private LocalizationPropertiesView propertiesView;
		private LocalizationPresenter localizationPresentor;
		private LocalizationStorage storage;
		private int storageVersion;
		private LocalizationStorage Storage { get => storage ?? LocalizationManager.Storage; }

		private void OnEnable()
		{
			noticeView = new NoticeView(this);
			localizationView = new LocalizationView(Storage, noticeView);
			propertiesView = new LocalizationPropertiesView(Storage);
			searchView = new SearchTreeView(this, new LocalizationSearchProvider(Storage));
			localizationPresentor = new LocalizationPresenter(this, searchView, localizationView, propertiesView);
		}

		/// <summary>
		/// Creation of initialization and display of a window on the monitor screen.
		/// </summary>
		public static LocalizationStorageWindow Show(LocalizationStorage storage, float minWidth = MIN_WIDTH, float minHight = MIN_HIGHT)
		{
			var instance = GetWindow<LocalizationStorageWindow>();
			instance.titleContent = new GUIContent("Simple Localization", EditorGUIUtility.IconContent("FilterByType@2x").image);
			instance.minSize = new Vector2(minWidth, minHight);
			instance.storage = storage;

			return instance;
		}

		/// <summary>
		/// Creation of initialization and display of a window on the monitor screen.
		/// </summary>
		[MenuItem("Window/Localization Storage")]
		public new static LocalizationStorageWindow Show() => Show(LocalizationManager.Storage);

		/// <summary>
		/// Method for rendering window content.
		/// </summary>
		internal void OnGUI()
		{
			if(searchView != null && storageVersion != Storage.Version)
			{
				searchView.IsChanged = true;
				storageVersion = Storage.Version;
			}

			localizationPresentor.OnGUI(new Rect(0, 0, position.width, position.height));

			noticeView.OnGUI();
		}
	}
}