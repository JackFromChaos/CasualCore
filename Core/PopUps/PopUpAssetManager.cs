using System;
using Sirenix.OdinInspector;
using UnityEngine;
[AutoBind]
public class PopUpAssetManager : MonoBehaviour, IPopUpAssetManager
{
    public PopUpTypeGameObjectMap popupPrefabMap;

    public bool TryGetPopUp(int key, out GameObject asset)
    {
        PopUpType realKey = (PopUpType)key;
        return popupPrefabMap.TryGetValue(realKey, out asset);
    }
    [Button]
    public void ShowPopup(PopUpType type)
    {
        Transmitter.Send(new ShowPopUpMsg(type) );
    }
    public void ShowPopup()
    {
        Transmitter.Send(new ShowPopUpMsg(PopUpType.Settings));
    }
}
[Serializable] public class PopUpTypeGameObjectMap : SerializableDictionary<PopUpType, GameObject> { }

public partial class ShowPopUpMsg
{
    public PopUpType PopUpType
    {
        get=>(PopUpType)popUpType;
         set
         {
             popUpType = (int)value;
         }
    }

    public ShowPopUpMsg()
    {

    }
    public ShowPopUpMsg(PopUpType type)
    {
        popUpType = (int)type;
    }
}