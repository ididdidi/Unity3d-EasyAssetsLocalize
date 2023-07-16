using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[AttributeUsage(AttributeTargets.Class)]
public class TypeMetadata : Attribute
{
    public Type Type { get; private set; }
    public Texture Texture { get; private set; }
    public TypeMetadata(Type type, string ediorIcon)
    {
        this.Type = type;
        this.Texture = EditorGUIUtility.IconContent(ediorIcon).image;
    }
}

public static class MetaContentProvider<T>
{
    private static Type type;
    private static Texture icon;
    public static GUIContent Content => new GUIContent(type.Name, icon);
}

// for example, this can be the type that you wanna associate the "string" type with
[TypeMetadata(typeof(string), "cs Script Icon")]
public class MyClass : MonoBehaviour { }

public class TestMetaData
{
    private static void InitializeMetadata()
    {
        Type genericType = typeof(MetaContentProvider<>);
        List<Tuple<Type, Type, Texture>> typeAndMetadataPairs = Assembly
            .GetAssembly(typeof(MyClass))
            .GetTypes()
            .Where(t => t.GetCustomAttribute<TypeMetadata>() != null)
            .Select(t => new Tuple<Type, Type, Texture>(t, t.GetCustomAttribute<TypeMetadata>().Type, t.GetCustomAttribute<TypeMetadata>().Texture))
            .ToList();


        foreach (var t in typeAndMetadataPairs)
        {
            Type constructGenericType = genericType.MakeGenericType(t.Item1);
            FieldInfo f = constructGenericType.GetField("type", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            f.SetValue(null, t.Item2);
            f = constructGenericType.GetField("icon", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            f.SetValue(null, t.Item3);
        }
    }

    [InitializeOnLoadMethod]
    public static void Test()
    {
        // call this at whatever you consider "startup" in your application/program
        InitializeMetadata();

        // test to see if it works
        Debug.Log($"Type : {typeof(MyClass)} - MetadataType : {MetaContentProvider<MyClass>.Content}");
    }
}