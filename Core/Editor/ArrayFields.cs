using UnityEditor;
using UnityEngine;

public static class ArrayObjectFields
{
    public static T[] ArrayFields<T>(this T[] array, string label, ref bool open, bool resizable = true) where T : Object
    {
        open = EditorGUILayout.Foldout(open, label);
        int newSize = array.Length;

        if (open)
        {
            if (resizable)
            {
                newSize = EditorGUILayout.IntField("Size", newSize);
                newSize = newSize < 0 ? 0 : newSize;
            }

            if (newSize != array.Length)
            {
                array = ResizeArray(array,  newSize);
            }

            EditorGUI.indentLevel++;
            for (var i = 0; i < newSize; i++)
            {
                array[i] = EditorGUILayout.ObjectField(typeof(T).Name, array[i], typeof(T), true) as T;
            }
            EditorGUI.indentLevel--;
        }
        return array;
    }

    private static T[] ResizeArray<T>(T[] array, int size)
    {
        T[] newArray = new T[size];

        for (var i = 0; i < size; i++)
        {
            if (i < array.Length)
            {
                newArray[i] = array[i];
            }
        }
        return newArray;
    }
}