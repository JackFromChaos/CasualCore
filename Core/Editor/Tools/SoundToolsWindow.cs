using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class SoundToolsWindow : EditorWindow
{
    [MenuItem("Tools/SoundToolsWindow")]
    static void Init()
    {
        var window = EditorWindow.GetWindow(typeof(SoundToolsWindow));
        window.titleContent = new GUIContent("Sound Tools Window");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (Selection.activeObject is AudioClip clip)
        {
            GUILayout.Label(clip.name);
            if (GUILayout.Button("Play"))
            {
                StopAllClips();
                PlayClip(clip);
            }
            if (GUILayout.Button("PlayLoop"))
            {
                StopAllClips();
                PlayClip(clip,0,true);
            }
        }

        if (GUILayout.Button("Stop"))
        {
            StopAllClips();
        }

        GUILayout.EndHorizontal();
    }
    void Awake()
    {

    }

    void OnDestroy()
    {

    }

    void OnSelectionChange()
    {
        Debug.LogError("SelectionChanged "+Selection.activeObject?.GetType());
        if (Selection.activeObject is AudioClip clip)
        {
            StopAllClips();
            PlayClip(clip);
        }
    }

    public static void PlayClip(AudioClip clip,int startSample=0,bool loop=false)
    {
        //UnityEditor.AudioUtil.StopAllPreviewClips();
        //UnityEditor.AudioUtil.PlayPreviewClip(clip,);
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "PlayPreviewClip",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new System.Type[] {
                typeof(AudioClip),typeof(int),typeof(bool)
            },
            null
        );
        method.Invoke(
            null,
            new object[] {
                clip,startSample,loop
            }
        );
    }
    public static void StopAllClips()
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "StopAllPreviewClips",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new System.Type[] { },
            null
        );
        method.Invoke(
            null,
            new object[] { }
        );
    }

}
