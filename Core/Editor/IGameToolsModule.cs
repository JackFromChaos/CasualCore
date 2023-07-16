using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public interface IGameToolsModule
{
    void Init();
    bool IsEnable();
    void OnGUI();
}


public interface IGameToolsPage
{
    string Name { get; }
    void OnGUI();
}
public abstract class BaseButtonGameToolsModule : IGameToolsModule
{

    protected string buttonName;
    public virtual void Init()
    {
        buttonName = GetType().Name.Replace("GameToolsModule", "");
    }

    public virtual bool IsEnable()
    {
        return true;
    }

    public virtual void OnGUI()
    {
        Color32 c = GameToolsHelper.ColorObject(this);
        GUI.backgroundColor = c;
        if (GUILayout.Button(buttonName))
        {
            OnClick();
        }
        GUI.backgroundColor = Color.white;
    }
    protected abstract void OnClick();
}

public class ButtonHelperToolsModule : IGameToolsModule
{
    public void Init()
    {
        
    }

    public bool IsEnable()
    {
        return Selection.activeGameObject != null;
    }

    public void OnGUI()
    {
        GUILayout.BeginHorizontal();

        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            /*if (GUILayout.Button("TryFixButtonsView"))
            {
                var mainMenuButtonControllers = go.GetComponentsInChildren<IHasInitView>();
                foreach (var menuButtonController in mainMenuButtonControllers)
                {
                    menuButtonController.InitView();
                }
            }*/

            if (GUILayout.Button("Change to ButtonFX"))
            {
                Undo.RecordObject(go, "Change to ButtonFX");
                ESoundType sound = ESoundType.ButtonDefault;
                var bs = go.GetComponent<ButtonSound>();
                if (bs != null)
                {
                    sound = bs.sound;
                    GameObject.DestroyImmediate(bs);
                }
                var b = go.GetComponent<Button>();
                GameObject.DestroyImmediate(b);
                var b2 = go.AddComponent<ButtonFX>();
                b2.image = b.image;

                b2.transition = b.transition;
                b2.animationTriggers = b.animationTriggers;
                b2.interactable = b.interactable;
                b2.onClick = b.onClick;
                b2.targetGraphic = b.targetGraphic;
                b2.colors = b.colors;

                bs = b2.AddComponent<ButtonSound>();
                bs.sound = sound;
                EditorUtility.SetDirty(go);

            }

        }

        GUILayout.EndHorizontal();
    }
}

public class GameToolsPageWindow<T> : EditorWindow where T : IGameToolsPage, new()
{
    protected T page;

    public static void ShowWindow()
    {
        ShowWindow<GameToolsPageWindow<T>>();
    }
    public static void ShowWindow<T2>() where T2 : GameToolsPageWindow<T>
    {
        T page = new T();
        T2 window = (T2)EditorWindow.GetWindow(typeof(T2));
        window.titleContent = new GUIContent(page.Name);
        window.page = page;
        window.Show();


    }

    void OnGUI()
    {
        page.OnGUI();
    }
}
