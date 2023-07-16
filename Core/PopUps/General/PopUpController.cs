using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class PopUpController : MonoBehaviour
{
    [Header("External Dependencies")] 
    //[Inject] public UserManager UserManager;
    [Inject] public GameSoundManager GameSoundManager;
    
    [Header("Internal Dependencies")] 
    public Button buttonClose;
    public PopUpLayer preferedPopUpLayer;
    public ESoundType soundShow = ESoundType.PopupShow;
    //float hideInterval = 0.1;
  
    [Header("Current State")]
    public ShowPopUpMsg initialShowRequest;

    public void Initialize(ShowPopUpMsg request)
    {
        if (buttonClose != null)
            buttonClose.onClick.AddListener(OnClose);
        initialShowRequest = request;
        Init(request);
    }

    public virtual void StartShow()
    {
    }
    public virtual void StartHide()
    {
    }
    
    public virtual float GetHideInterval()
    {
        return 0.01f;
    }

    protected virtual void Init(ShowPopUpMsg request)
    {
        if(GameSoundManager!=null)
            GameSoundManager.PlaySound(soundShow);
    }

    public virtual void OnClose()
    {
        if (initialShowRequest.closeCallback != null)
            initialShowRequest.closeCallback(0);
        Transmitter.Send(new ClosePopUpMsg { request = initialShowRequest });
    }
}