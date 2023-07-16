using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TransmitterMonitorWindow : EditorWindow
{
    [MenuItem("Tools/TransmitterMonitorWindow")]
    static void MenuShow()
    {
        var window = EditorWindow.GetWindow(typeof(TransmitterMonitorWindow));
        window.titleContent = new GUIContent("TransmitterMonitorWindow");
        window.Show();
    }
    List<EvMonitorElement> logs=new List<EvMonitorElement>();
    void OnEnable()
    {
        Transmitter.OnMessageSend -= OnMessageSend;
        Transmitter.OnMessageReceive -= OnMessageReceive;
        Transmitter.OnMessageSend += OnMessageSend;
        Transmitter.OnMessageReceive += OnMessageReceive;
    }

    void OnDestroy()
    {
        Transmitter.OnMessageSend -= OnMessageSend;
        Transmitter.OnMessageReceive -= OnMessageReceive;

    }
    Vector2 scrollPos;
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear"))
        {
            logs.Clear();
        }
        GUILayout.EndHorizontal();
        if(logs==null)
            return;
        scrollPos=GUILayout.BeginScrollView(scrollPos);
        GUILayout.BeginVertical();
        foreach (var log in logs)
        {
            if (log.recive&&log.reciver==null)
                continue;
            if(log.ev==null)
                continue;

            GUI.backgroundColor = ColorObject(log.ev);
            GUILayout.BeginHorizontal(GUI.skin.box);
            GUI.backgroundColor = Color.white;
            if (log.recive)
            {
                GUILayout.Space(100);
                GUILayout.Label(log.ev.GetTypeName(),GUILayout.Width(200));
                if (GUILayout.Button("Select",GUILayout.Width(50)))
                {
                    Selection.activeObject = log.reciver;
                }
                if (log.reciver!=null&&GUILayout.Button(log.reciver.GetTypeName()))
                {
                    EditorGUIUtility.systemCopyBuffer = log.reciver.GetTypeName();
                }
            }
            else
            {
                
                if (GUILayout.Button(log.ev.GetTypeName(), GUILayout.Width(200)))
                {
                    EditorGUIUtility.systemCopyBuffer = log.ev.GetTypeName();
                }
                if (GUILayout.Button(log.method))
                {
                    EditorGUIUtility.systemCopyBuffer = log.method;
                }
                if (GUILayout.Button(log.sender))
                {
                    EditorGUIUtility.systemCopyBuffer = log.sender;
                }
            }




            GUILayout.EndHorizontal();
            

        }


        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
    private void OnMessageReceive(object message, MonoBehaviour reciver)
    {
        logs.Add(new EvMonitorElement(message,reciver));
        Repaint();
    }

    void OnMessageSend(object message)
    {
        logs.Add(new EvMonitorElement(message,null) );
        Debug.LogWarning($"Send {message.GetTypeName()}");
        Repaint();
    }
    public Color baseActionColor = new Color32(197,112,180,255);
    private Color ColorForType(int hash)
    {
        float H, S, V;
        Color.RGBToHSV(baseActionColor, out H, out S, out V);
        H = ((float)hash) / (float)int.MaxValue / 2;
        H = H < 0 ? -H : H + .5f;
        Color c = Color.HSVToRGB(H, S, V);
        c.a = baseActionColor.a;
        return c;
    }

    public Color ColorObject(object o)
    {
        int hash = 0;
        if (o == null)
        {
            return baseActionColor;
        }

        if (o is Type)
        {
            hash = (o as Type).Name.GetHashCode();
        }
        else if (o is string)
        {
            hash = o.GetHashCode();
        }
        else
        {
            hash=o.GetType().Name.GetHashCode();
        }

        return ColorForType(hash);
    }
    public class EvMonitorElement
    {

        public object ev;
        public MonoBehaviour reciver;
        public string stackTrace;
        public string sender;
        public string method;
        public bool recive;

        public EvMonitorElement(object ev, MonoBehaviour reciver)
        {
            this.ev = ev;
            this.reciver = reciver;
            recive = reciver != null;
            stackTrace = Environment.StackTrace;
            var arr= stackTrace.Split('\n');
            sender = arr[4];
            var arr2=sender.Split(" ");
            method = arr2[3];
            arr =sender.Split("\\");
            sender = arr[arr.Length - 1];
 
        }

    }

}