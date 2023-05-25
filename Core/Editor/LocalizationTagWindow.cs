using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	/// <summary>
	/// Display localization tag  data in a separate inspector window.
	/// </summary>
	public class LocalizationTagWindow : EditorWindow
	{
		private Vector2 scrollPosition = Vector2.zero;

		public LocalizationResourceView ResourceView { get; set; }

		public void OnEnable()
		{
			minSize = new Vector2(240f, 320f);
		}

		void OnGUI()
		{
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			ResourceView?.Show();
			EditorGUILayout.EndScrollView();
		}
	}
}