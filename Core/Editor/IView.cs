using UnityEngine;

namespace SimpleLocalization
{
	/// <summary>
	/// Interface for displaying the View in the inspector window
	/// </summary>
	public interface IView
	{
		/// <summary>
		/// Draw the View in the inspector window.
		/// </summary>
		/// <param name="position"></param>
		void OnGUI(Rect position);
	}
}