using System.Collections.Generic;
using System.Linq;
using Dio.MiniJSON;
using Sirenix.OdinInspector.Editor.GettingStarted;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameToolsWindow : EditorWindow
{
    public bool inited = false;
    //public List<IGameToolsPage> Pages = new List<IGameToolsPage>();
    public List<IGameToolsModule> Modules = new List<IGameToolsModule>();


    [MenuItem("Tools/GameToolsWindow")]
    static void MenuShow()
    {
        var window = EditorWindow.GetWindow(typeof(GameToolsWindow));
        window.titleContent = new GUIContent("GameToolsWindow");
        window.Show();
    }
    private void Init()
    {
        Modules.Clear();
        var list=TypesExt.GetDerivedClasses<IGameToolsModule>();
        foreach (var type in list)
        {
            var module=type.Instantiate() as IGameToolsModule;
            AddModule(module);
        }
        //AddModule(new ScreenShortGameToolsModule());
        
    }

    private void AddModule(IGameToolsModule module)
    {
        Modules.Add(module);
        module.Init();
    }

    void OnGUI()
    {
        if (!inited||Modules.Count==0 )
        {
            Init();
            inited = true;
        }

        foreach (var module in Modules)
        {
            if(module.IsEnable())
                module.OnGUI();
        }

        //var editor = UnityEditor.Editor.CreateEditor(this);
        //editor.OnInspectorGUI();
    }

}

public static class GameToolsHelper
{
    public static Color baseActionColor = new Color32(97, 12, 80, 255);
    private static Color ColorForType(int hash)
    {
        float H, S, V;
        Color.RGBToHSV(baseActionColor, out H, out S, out V);
        H = ((float)hash) / (float)int.MaxValue / 2;
        H = H < 0 ? -H : H + .5f;
        Color c = Color.HSVToRGB(H, S, V);
        c.a = baseActionColor.a;
        return c;
    }

    public static Color ColorObject(object o)
    {
        int hash = 0;
        if (o == null)
        {
            return baseActionColor;
        }

        if (o is System.Type)
        {
            hash = (o as System.Type).Name.GetHashCode();
        }
        else if (o is string)
        {
            hash = o.GetHashCode();
        }
        else
        {
            hash = o.GetType().Name.GetHashCode();
        }

        return ColorForType(hash);
    }

}