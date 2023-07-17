using UnityEngine;
using UnityEditor;

namespace ResourceLocalization
{
    public class AddTypeLocalizationWindow : EditorWindow
    {
		public static readonly Vector2 SIZE = new Vector2(320f, 240f);

		private string typeName;
		private string iconName;
		private System.Action<string> action;

		/// <summary>
		/// Creation of initialization and display of a window on the monitor screen.
		/// </summary>
		public static AddTypeLocalizationWindow Show(System.Action<string> action)
		{
			var instance = GetWindow<AddTypeLocalizationWindow>(true, "Add new type", true);

			instance.minSize = SIZE;
			instance.maxSize = SIZE;
			instance.action = action;
			return instance;
		}

		public void OnGUI()
		{
			typeName = EditorGUILayout.TextField("", typeName);
			iconName = EditorGUILayout.TextField("", iconName);

			if (GUILayout.Button("Add"))
			{
				new TypesMetaProvider().AddType(typeName);
				
				//action?.Invoke(typeName);
				Close();
			}
		}
	}
}
