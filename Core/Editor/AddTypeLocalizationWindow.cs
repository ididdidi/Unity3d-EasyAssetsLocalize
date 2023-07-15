using UnityEngine;
using UnityEditor;

namespace ResourceLocalization
{
    public class AddTypeLocalizationWindow : EditorWindow
    {
		public static readonly Vector2 SIZE = new Vector2(320f, 240f);

		/// <summary>
		/// Creation of initialization and display of a window on the monitor screen.
		/// </summary>
		public static AddTypeLocalizationWindow Show(System.Action action)
		{
			var instance = GetWindow<AddTypeLocalizationWindow>(true, "Add new type", true);

			instance.minSize = SIZE;
			instance.maxSize = SIZE;

			return instance;
		}
	}
}
