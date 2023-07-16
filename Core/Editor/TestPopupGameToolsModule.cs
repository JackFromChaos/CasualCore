using UnityEditor;
using UnityEngine;

public class TestPopupGameToolsModule : ExpandableGameToolsModule
{
    public override string Name => "TestPopup";
    protected PopUpManager popUpManager;
    public override void Init()
    {
        popUpManager=GetComponent<PopUpManager>();
    }

    public override bool IsEnable()
    {
        return Application.isPlaying;
    }
    PopUpType type=PopUpType.Credits;
    PopUpType type2 = PopUpType.Credits;
    private string groupName="test";
    protected override void RealOnGUI()
    {
        GUILayout.BeginHorizontal();
        type = (PopUpType)EditorGUILayout.EnumPopup(type);
        if (GUILayout.Button("ShowPopup"))
        {
            Transmitter.Send(new ShowPopUpMsg(type));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        groupName= EditorGUILayout.TextField(groupName);
        type2 = (PopUpType)EditorGUILayout.EnumPopup(type2);
        if (GUILayout.Button("ShowQueuePopup"))
        {
            Transmitter.Send(new ShowQueuePopup(groupName, type2));
        }
        GUILayout.EndHorizontal();
        if (popUpManager != null)
        {
            foreach (var msg in popUpManager.instatiatedPopUps)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(msg.Key.PopUpType.ToString(), msg.Value, typeof(GameObject), true);
                if (GUILayout.Button("Select",GUILayout.Width(60)))
                {
                    Selection.activeObject = msg.Value;
                }
                if (GUILayout.Button("Close", GUILayout.Width(60)))
                {
                    Transmitter.Send(new ClosePopUpMsg(){request = msg.Key});
                }
                
                GUILayout.EndHorizontal();
            }
        }
    }

    public static T GetComponent<T>() where T : Component
    {
        T[] allMonoBehavoir = UnityEngine.Object.FindObjectsOfType<T>();
        if (allMonoBehavoir.Length > 0)
            return allMonoBehavoir[0];
        return null;
    }
}