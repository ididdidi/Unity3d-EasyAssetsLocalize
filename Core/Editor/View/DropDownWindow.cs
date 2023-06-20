using UnityEngine;
using UnityEditor;

public interface IEditorView
{
    void OnGUI();
}

public abstract class DropDownWindow<C> : EditorWindow
{
    // Constants
    private const float defaultWidth = 240f;
    private const float defaultHeight = 320f;

    // Member variables
    protected IEditorView view;

    public static void Show<T>(Vector2 screenPosition, Vector2 size, C content) where T : DropDownWindow<C>
    {
        float width = System.Math.Max(size.x, defaultWidth);
        float height = System.Math.Max(size.y, defaultHeight);
        Rect buttonRect = new Rect(screenPosition.x - width / 2, screenPosition.y - EditorGUIUtility.singleLineHeight, width, 1);
        
        var instance = (T)CreateInstance(typeof(T));
        instance.hideFlags = HideFlags.HideAndDontSave;
        instance.Init(content);
        
        instance.ShowAsDropDown(buttonRect, new Vector2(buttonRect.width, height));

        instance.Focus();

        instance.wantsMouseMove = true;
    }

    public abstract void Init(C content);

    internal void OnGUI()
    {
        view?.OnGUI();
    }
}
