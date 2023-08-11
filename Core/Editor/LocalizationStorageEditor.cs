using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
	/// <summary>
	/// Responsible for presenting the Localization Storage in the inspector.
	/// </summary>
	[CustomEditor(typeof(LocalizationStorage))]
	public partial class LocalizationStorageEditor : Editor, IDisplay
	{
		private NoticeView noticeView;
		private LocalizationStorage storage;
		private SearchEditorView searchView;
		private LocalizationView localizationView;
		private LocalizationPropertiesView1 propertiesView;
		private IEditorView currentView;

		private float width;

		/// Animated view change
		private IEditorView animatedView;
		private long lastTime;
		private float currentAnimation, targetAnimation = 0f;

		/// <summary>
		/// Storage link caching.
		/// </summary>
		public void OnEnable()
		{
			storage = this.target as LocalizationStorage;

			noticeView = new NoticeView(this);
			localizationView = new LocalizationView(storage, noticeView, () => StartAnimationView(localizationView, 0f, 1f));

			propertiesView = new LocalizationPropertiesView1(() => { StartAnimationView(propertiesView, 0f, 1f); });

			searchView = new SearchEditorView(this, new LocalizationSearchProvider(storage, (data) => { localizationView.Data = data;  StartAnimationView(localizationView, 1f, 0f); return false; }), false);
			searchView.OptionButton = ShowPropertiesButton;
			currentView = searchView;
		}

		/// <summary>
		/// Drawing LocalizationStorage in the inspector window.
		/// </summary>
		public override void OnInspectorGUI()
		{
			var height = (animatedView != null)? Mathf.Max(currentView.HeightInGUI, animatedView.HeightInGUI) : currentView.HeightInGUI;
			

			var rect = GUILayoutUtility.GetRect(width, height);
			if(rect.width>1) { width = rect.width + 22; }

			var position = new Rect(0, 0, width, height);
			currentView.OnGUI(position);
			if (animatedView != null) PlayAnimation(position);
			noticeView.OnGUI();
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
			if (currentAnimation != targetAnimation)
			{
				currentAnimation = Mathf.MoveTowards(currentAnimation, targetAnimation, deltaTime * 4);
				rect.x = rect.width * currentAnimation;
				animatedView.OnGUI(rect);
			}
			else
			{
				if (targetAnimation == 0)
				{
					currentView = animatedView;
					currentView.OnGUI(rect);
				}
				animatedView = null;
			}
			this.Repaint();
		}
	}
}