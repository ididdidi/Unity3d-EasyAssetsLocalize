using UnityEngine;

namespace EasyAssetsLocalize
{
	public class TypeView : IEditorView
	{
		public GUIContent Content { get; set; }

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
