using System.Runtime.InteropServices.Expando;
using UnityEditor;
using UnityEngine;

public abstract class InlineGameToolsModule : IGameToolsModule
{
    public GUISkin skin;
    public abstract string Name { get; }

    public virtual void Init()
    {

    }

    public abstract bool IsEnable();

    public void OnGUI()
    {
        if (skin == null)
        {
            skin = Resources.Load<GUISkin>("GameToolsSkin");
        }
        //Color32 c = new Color32(76, 75, 73, 255);
        Color32 c = GameToolsHelper.ColorObject(this);
        GUI.color = c;
        GUILayout.BeginHorizontal(skin.GetStyle("region"));
        GUI.color = Color.white;
        GUILayout.Label(Name);
        InlineOnGUI();
        if (GUILayout.Button("C", GUILayout.Width(20)))
        {
            GUIUtility.systemCopyBuffer = GetType().GetClassName();
        }
        GUILayout.EndHorizontal();
    }

    public abstract void InlineOnGUI();
}
public abstract class ExpandableGameToolsModule : IGameToolsModule
{
    public GUISkin skin;
    public abstract string Name { get; }
    public abstract void Init();

    public abstract bool IsEnable();
    public bool IsExpand = false;
    public void OnGUI()
    {
        if (skin == null)
        {
            skin=Resources.Load<GUISkin>("GameToolsSkin");
        }
        //var saveSkin=GUI.skin;
        //GUI.skin = skin;
        //GUI.color=Color.gray;
        //GUILayout.BeginVertical(skin.box);
        //Color32 c = new Color32(76, 75, 73, 255);
        Color32 c = GameToolsHelper.ColorObject(this);

        GUI.color = c;
        GUILayout.BeginVertical(skin.GetStyle("region"));
        GUI.color=Color.white;
        GUILayout.BeginHorizontal();
        if (IsExpand)
        {
            if (GUILayout.Button("", skin.GetStyle("arrow_down")))
            {
                IsExpand = false;
            }

        }
        else
        {
            if (GUILayout.Button("", skin.GetStyle("arrow_right")))
            {
                IsExpand=true;
            }
        }
        GUILayout.Label(Name);
        InlineOnGUI();
        if (GUILayout.Button("C", GUILayout.Width(20)))
        {
            GUIUtility.systemCopyBuffer = GetType().GetClassName();
        }
        GUILayout.EndHorizontal();
        if(IsExpand)
            RealOnGUI();
        GUILayout.EndVertical();
        
    }

    public virtual void InlineOnGUI()
    {
    }
    protected abstract void RealOnGUI();
}