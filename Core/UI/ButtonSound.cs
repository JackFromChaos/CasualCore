using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    [Header("Internal Dependencies")] 
    public ESoundType sound = ESoundType.ButtonDefault;
    public Button button;
    
    //[Header("Current State")]

    void Awake()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Transmitter.Send(new PlaySoundMsg(sound));

    }
}
