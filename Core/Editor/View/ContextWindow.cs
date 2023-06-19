using UnityEngine;
using UnityEditor;

public class ContextWindow : EditorWindow
{
    [System.Serializable]
    public struct Context
    {
        public Vector2 Position { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        public Context(Vector2 screenPosition, float width = 0.0f, float height = 0.0f)
        {
            Position = screenPosition;
            Width = width;
            Height = height;
        }
    }

    // Constants
    private const float defaultWidth = 240f;
    private const float defaultHeight = 320f;

    // Static variables
    private static ContextWindow instance = null;
    private static long lastClosedTime;

    // Member variables
    private IEditorView view;

    void OnEnable()
    {
        instance = this;
    }

    void OnDisable()
    {
        lastClosedTime = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond;
        instance = null;
    }

    public static bool Open(Context context, IEditorView view)
    {
        // If the window is already open, close it instead.
        Object[] wins = Resources.FindObjectsOfTypeAll(typeof(ContextWindow));
        if (wins.Length > 0)
        {
            try
            {
                ((EditorWindow)wins[0]).Close();
                return false;
            }
            catch (System.Exception)
            {
                instance = null;
            }
        }

        long nowMilliSeconds = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond;
        bool justClosed = nowMilliSeconds < lastClosedTime + 50;
        if (!justClosed)
        {
            if (instance == null)
            {
                instance = CreateInstance<ContextWindow>();
                instance.hideFlags = HideFlags.HideAndDontSave;
            }
            instance.Init(context, view);
            return true;
        }
        return false;
    }


    void Init(Context context, IEditorView view)
    {
        this.view = view;

        float width = System.Math.Max(context.Width, defaultWidth);
        float height = System.Math.Max(context.Height, defaultHeight);

        Rect buttonRect = new Rect(context.Position.x - width / 2, context.Position.y - EditorGUIUtility.singleLineHeight, width, 1);

        ShowAsDropDown(buttonRect, new Vector2(buttonRect.width, height));

        Focus();

        wantsMouseMove = true;
    }

    public void OnGUI()
    {
        view?.OnInspectorGUI();
    }
}
