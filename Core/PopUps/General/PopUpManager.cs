using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IPopUpAssetManager
{
    public bool TryGetPopUp(int key, out GameObject asset);
}

[AutoBind]
public class PopUpManager : MonoBehaviour, IListener<ShowPopUpMsg>, IListener<ClosePopUpMsg>
{
    [Header("External Dependencies")] 
    [Inject] public PopUpAssetManager AssetManager;
   
    [Header("Internal Dependencies")] 
    public PopUpLayerTransformMap layersMap;

    [Header("Current State")]
    private bool needCheckQueue = false;
    public List<ShowPopUpMsg> popUpRequestQueue;
    public ShowPopUpMsgGameObjectMap instatiatedPopUps;
    private float defaultHideInterval = 2f;

    public void Handle(ShowPopUpMsg msg)
    {
        if (!msg.ignoreQueue && DoesExitPopUpOnSameTypeOnSameLayer(msg))
        {
            Debug.Log("[BREAD] Add to queue ShowPopUp msg = " + msg.popUpType );
            popUpRequestQueue.Add(msg);
            return;
        }
        ShowPopUp(msg);
    }

    public void ShowPopUp(ShowPopUpMsg msg)
    {
        /*Debug.Log("[BREAD] ShowPopUp msg = " + msg.popUpType);
        if (!AssetManager.popupPrefabMap.ContainsKey(msg.popUpType))
        {
            Debug.LogError("Pop up Prefab not found when ShowPopUp msg = " + msg.popUpType);
        }
        GameObject prefab = AssetManager.popupPrefabMap[msg.popUpType];*/
        if (!AssetManager.TryGetPopUp(msg.popUpType, out var prefab))
        {
            Debug.LogError("Pop up Prefab not found when ShowPopUp msg = " + msg);
            return;
        }
        if(prefab==null)
        {
            Debug.LogError("Missing prefab for popUp type = " + msg.popUpType + " Please adjust prefab in Assets/Resources/ProjectContext.asset in sub object PopUpManager in component PopUpAssetManager");
            return;
        }
        if (msg.popUpLayer == PopUpLayer.Default)
        {
            PopUpController pc = prefab.GetComponent<PopUpController>();
            msg.popUpLayer = pc.preferedPopUpLayer;
        }
        Transform target = layersMap[msg.popUpLayer];
        GameObject obj = DI.Instantiate(prefab, target);
        instatiatedPopUps.Add(msg, obj);
        PopUpController popUpController = obj.GetComponent<PopUpController>();
        popUpController.Initialize(msg);
        popUpController.StartShow();
    }

    bool DoesExitPopUpOnSameTypeOnSameLayer(ShowPopUpMsg msg)
    {
        foreach (var k in instatiatedPopUps.Keys)
        {
            if (msg.popUpLayer == k.popUpLayer && msg.popUpType == k.popUpType)
                return true;
        }

        return false;
    }

    public virtual void Handle(ClosePopUpMsg msg)
    {
        Debug.Log("[BREAD] ClosePopUpMsg msg = " +  msg.request.popUpType);
        msg.request.isClosed = true;
        if (!instatiatedPopUps.Contains(msg.request))
            return;
        GameObject obj = instatiatedPopUps[msg.request];
        instatiatedPopUps.Remove(msg.request);
        PopUpController popUpController = obj.GetComponent<PopUpController>();
        popUpController.StartHide();
        float hideInterval = popUpController.GetHideInterval();
        if (hideInterval <= 0)
            hideInterval = defaultHideInterval;
        GameObject.Destroy(obj,hideInterval+0.001f);
        Resources.UnloadUnusedAssets();
        needCheckQueue = true;
    }

    private void Update()
    {
        if (needCheckQueue)
        {
            List<ShowPopUpMsg> toDel = new List<ShowPopUpMsg>();
            foreach (var msg in popUpRequestQueue)
            {
                if (DoesExitPopUpOnSameTypeOnSameLayer(msg))
                    continue;
                ShowPopUp(msg);
                toDel.Add(msg);
                break;
            }
            foreach (var msg in toDel)
                popUpRequestQueue.Remove(msg);
        }
    }
}
[Serializable] public class PopUpLayerTransformMap : SerializableDictionary<PopUpLayer, Transform> { }
[Serializable] public class ShowPopUpMsgGameObjectMap : SerializableDictionary<ShowPopUpMsg, GameObject> { }
