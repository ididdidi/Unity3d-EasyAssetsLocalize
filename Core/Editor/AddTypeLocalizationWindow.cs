using UnityEngine;
using UnityEditor;

namespace ResourceLocalization
{
    public class AddTypeLocalizationWindow : EditorWindow
    {
		public static readonly Vector2 SIZE = new Vector2(320f, 240f);

		private string typeName;
		private string iconName = "BuildSettings.Editor";

		/// <summary>
		/// Creation of initialization and display of a window on the monitor screen.
		/// </summary>
		public new static AddTypeLocalizationWindow Show()
		{
			var instance = GetWindow<AddTypeLocalizationWindow>(true, "Add new type", true);

			instance.minSize = SIZE;
			instance.maxSize = SIZE;
			return instance;
		}

		public void OnGUI()
		{
			typeName = EditorGUILayout.TextField("Name of type", typeName);
			iconName = EditorGUILayout.TextField("Name of icon", iconName);

			if (GUILayout.Button("Add"))
			{
				TypesMetaProvider.AddType(typeName);
				Close();
			}
		}
	}
}
