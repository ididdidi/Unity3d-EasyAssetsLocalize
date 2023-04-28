using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	public class LocalizationReceiverWindow : EditorWindow
	{
		private Vector2 scrollPosition = Vector2.zero;
		private bool foldout = true;
		public LocalizationTag LocalizationReceiver { 
			set 
			{
				if (editor) { foldout = editor.foldout; }
				if (value) { editor = (LocalizationReceiverEditor)Editor.CreateEditor(value); editor.foldout = foldout; } 
			} 
		}
		private LocalizationReceiverEditor editor;

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