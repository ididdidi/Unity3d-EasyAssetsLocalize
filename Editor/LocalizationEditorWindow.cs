using UnityEditor;
public class LocalizationEditorWindow : EditorWindow
{
	public LocalizationData localization;
	private LocalizationReorderableList localizationsList;

    void OnGUI()
    {
		if(localizationsList == null && localization) { localizationsList = new LocalizationReorderableList(localization); }
		localizationsList?.DoLayoutList();
	}
}