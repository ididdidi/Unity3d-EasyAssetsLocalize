using UnityEngine;
using UnityEditor;

namespace ResourceLocalization
{
    public class AddTypeLocalizationWindow : EditorWindow
    {
		public static readonly Vector2 SIZE = new Vector2(320f, 240f);

		private string iconName = "FilterByType";
		private Object @object;

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
			@object = EditorGUILayout.ObjectField("Object", @object, typeof(object), false );
			iconName = EditorGUILayout.TextField("Name of icon", iconName);

			EditorGUI.BeginDisabledGroup(!@object);
			if (GUILayout.Button("Add"))
			{
				TypesMetaProvider.AddType(@object.GetType(), iconName);
				Close();
				AssetDatabase.Refresh();
			}
			EditorGUI.EndDisabledGroup();
		}
	}
}
