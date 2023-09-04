using UnityEngine;

namespace EasyAssetsLocalize
{
	/// <summary>
	/// Class for displaying the type in the inspector or editor window.
	/// </summary>
	internal class TypeView : IEditorView
	{
		public GUIContent Content { get; set; }

		/// <summary>
		/// Method to display in an inspector or editor window.
		/// </summary>
		/// <param name="position"><see cref="Rect"/></param>
		public void OnGUI(Rect position)
		{
			var rect = new Rect(0f, 0f, 128f, 128f);
			rect.center = position.center;
			rect.y -= 25f;
			GUI.DrawTexture(rect, Content.image, ScaleMode.ScaleToFit);

			position.y += 55f;
			var style = new GUIStyle("AM MixerHeader");
			style.alignment = TextAnchor.MiddleCenter;
			style.fontSize = 32;
			GUI.Label(position, Content.text, style);
		}
	}
}
