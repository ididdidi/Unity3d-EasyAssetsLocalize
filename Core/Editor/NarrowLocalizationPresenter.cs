using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
    public class NarrowLocalizationPresenter : LocalizationPresenter
	{
		/// Animated view change
		private IEditorView animatedView;
		private long lastTime;
		private float currentAnimation, targetAnimation = 0f;

		public NarrowLocalizationPresenter(IDisplay parent, SearchTreeView searchView, LocalizationView localizationView, LocalizationPropertiesView propertiesView)
			: base(parent, searchView, localizationView, propertiesView) {
			this.localizationView.OnBackButton = () => StartAnimationView(localizationView, 0f, 1f);
			this.propertiesView.OnBackButton = () => StartAnimationView(propertiesView, 0f, 1f);
			this.searchView.OptionButton = ShowPropertiesButton;
			this.searchView.OnSelectEntry = (data) => { localizationView.Data = data; StartAnimationView(localizationView, 1f, 0f); };
			currentView = searchView;
		}

		/// <summary>
		/// Drawing LocalizationStorage in the inspector window.
		/// </summary>
		public override void OnGUI(Rect position)
		{
			currentView.OnGUI(position);
			if (currentAnimation != targetAnimation) { PlayAnimation(position); }
		}

		/// <summary>
		/// Button to show properties
		/// </summary>
		/// <param name="rect">Position</param>
		private void ShowPropertiesButton(Rect rect)
		{
			if (GUI.Button(rect, EditorGUIUtility.IconContent("_Popup"), GUIStyle.none)) { StartAnimationView(propertiesView, 1f, 0f); }
		}

		private void StartAnimationView(IEditorView view, float currentAnimation, float targetAnimation)
		{
			animatedView = view;
			currentView = searchView;
			this.currentAnimation = currentAnimation;
			this.targetAnimation = targetAnimation;
			lastTime = System.DateTime.Now.Ticks;
		}

		private void PlayAnimation(Rect position)
		{
			long now = System.DateTime.Now.Ticks;
			float deltaTime = (now - lastTime) / (float)System.TimeSpan.TicksPerSecond;
			lastTime = now;

			var rect = new Rect(position);
			currentAnimation = Mathf.MoveTowards(currentAnimation, targetAnimation, deltaTime * 4);
			rect.x = rect.width * currentAnimation;
			EditorGUI.DrawRect(rect, Background);
			animatedView.OnGUI(rect);

			if (currentAnimation == targetAnimation && targetAnimation == 0)
			{
				currentView = animatedView;
				animatedView = null;
			}
			parent.Repaint();
		}
	}
}