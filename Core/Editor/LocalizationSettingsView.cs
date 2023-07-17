﻿using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationSettingsView
    {
        private readonly Color background = new Color(0.22f, 0.22f, 0.22f);
        private LocalizationStorageWindow window;
        private LanguagesListView languagesList;
        private TypesListView typesList;
        private Vector2 scrollPosition = default;

        public LocalizationSettingsView(LocalizationStorageWindow window)
        {
            this.window = window ?? throw new System.ArgumentNullException(nameof(window));
            languagesList = new LanguagesListView(LocalizationManager.LocalizationStorage);
            typesList = new TypesListView(new TypesMetaProvider());
        }

        public void OnGUI(Rect position)
        {
            EditorGUI.DrawRect(position, background);
            GUI.Label(position, GUIContent.none, "grey_border");

            GUILayout.BeginArea(position);
            GUILayout.BeginHorizontal(EditorStyles.inspectorDefaultMargins);
            if (GUILayout.Button(EditorGUIUtility.IconContent("tab_prev@2x"), GUIStyle.none, GUILayout.Height(20f))) {
                Close();
            }
            GUILayout.FlexibleSpace();
            GUILayout.Label("Settings", "AM MixerHeader");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, EditorStyles.inspectorDefaultMargins);
            languagesList.DoLayoutList();
            GUILayout.Space(4);
            typesList.DoLayoutList();
            GUILayout.EndScrollView();
            GUILayout.EndArea();

            var curentEvent = Event.current;
            if (curentEvent.type == EventType.KeyDown && curentEvent.keyCode == KeyCode.Escape)
            {
                Close();
                curentEvent.Use();
            }
        }

        public void Close()
        {
            languagesList.index = typesList.index  = -1;
            window.ShowSettings = false;
            EditorGUI.FocusTextInControl(null);
        }
    }
}