using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	public class LocalizationReceiverWindow : EditorWindow
	{
		public LocalizationReceiver LocalizationReceiver { set { if (value) { editor = Editor.CreateEditor(value); } } }
		private Editor editor;

		void OnGUI()
		{
			editor?.OnInspectorGUI();
		}
	}
}