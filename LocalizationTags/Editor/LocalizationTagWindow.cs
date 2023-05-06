using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	public class LocalizationTagWindow : EditorWindow
	{
		private Vector2 scrollPosition = Vector2.zero;
		private bool foldout = true;
		public LocalizationTag LocalizationReceiver { 
			set 
			{
				if (editor) { foldout = editor.localizationfoldout; }
				if (value) { editor = (LocalizationTagEditor)Editor.CreateEditor(value); editor.localizationfoldout = foldout; } 
			} 
		}
		private LocalizationTagEditor editor;

		public void OnEnable()
		{
			minSize = new Vector2(240f, 320f);
		}

		void OnGUI()
		{
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			editor?.OnInspectorGUI();
			EditorGUILayout.EndScrollView();
		}
	}
}