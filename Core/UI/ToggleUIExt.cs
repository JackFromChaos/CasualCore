using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Toggle))]
public class ToggleUIExt : MonoBehaviour
{
    [Header("External Dependencies")] 
    [Inject] public GameSoundManager GameSoundManager;
    
    [Header("Current State")]
    public Toggle toggle;
    public GameObject enableGraphics;
    public GameObject disableGraphics;
    public ESoundType sound = ESoundType.ButtonDefault;
    
    //[Header("Internal Dependencies")] 

    void Start()
    {
        if (toggle == null)
        {
            toggle = GetComponent<Toggle>();
        }
        enableGraphics?.SetActive(toggle.isOn);
        disableGraphics?.SetActive(!toggle.isOn);
        toggle.onValueChanged.AddListener(OnChanged);
    }
    private void OnChanged(bool a)
    {
        enableGraphics?.SetActive(a);
        disableGraphics?.SetActive(!a);
        GameSoundManager.PlaySound(sound);
    }

}
