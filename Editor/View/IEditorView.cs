using UnityEngine;

namespace EasyAssetsLocalize
{
    /// <summary>
    /// Interface for displaying a View in an inspector or editor window.
    /// </summary>
    public interface IEditorView
    {
        /// <summary>
        /// Method to display in an inspector or editor window.
        /// </summary>
        /// <param name="position"><see cref="Rect"/></param>
        void OnGUI(Rect position);
    }
}