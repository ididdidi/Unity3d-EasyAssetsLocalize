using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UnityExtended
{
	/// <summary>
	/// Class for displaying a notification in the editor window.
	/// </summary>
	public class NoticeView
    {
        private IDisplay display;
        private GUIContent noticeContent;
		private Rect position;
        private long lastTime;
        private bool showNotice;
        private float fillAmount;
        private int taskID;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="window"><see cref="EditorWindow"/></param>
		public NoticeView(IDisplay display) => this.display = display ?? throw new System.ArgumentNullException(nameof(display));

		/// <summary>
		/// Method for rendering an animated notification. It is necessary to call the window method of the same name.
		/// </summary>
		public async void OnGUI()
		{
			if(!showNotice) { return; }

			long now = System.DateTime.Now.Ticks;
			float deltaTime = (now - lastTime) / (float)System.TimeSpan.TicksPerSecond;
			lastTime = now;

			// Animate notification
			if (fillAmount != 1f)
			{
				fillAmount = Mathf.MoveTowards(fillAmount, 1f, deltaTime * 2);
				position.y = position.height * (fillAmount - 0.8f);
			}

			// Draw notification
			GUILayout.BeginArea(position);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label(noticeContent, "NotificationBackground");
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();

			// Wait notification
			if (fillAmount == 1f)
			{
				var waitID = taskID;
				await Task.Delay(1000);
				// Reset
				if (waitID == taskID)
				{
					fillAmount = 0f;
					showNotice = false;
				}
			}

			display.Repaint();
		}

		/// <summary>
		/// Method to start animated notification display.
		/// </summary>
		/// <param name="position"><see cref="Rect"/> position</param>
		/// <param name="content"><see cref="GUIContent"/> displayed in notification</param>
		public void Show(Rect position, GUIContent content)
		{
			this.position = position;
			noticeContent = content;
			taskID++;
			showNotice = true;
			fillAmount = 0f;
			lastTime = System.DateTime.Now.Ticks;
		}
	}
}