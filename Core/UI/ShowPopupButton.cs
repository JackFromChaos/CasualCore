using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShowPopupButton : MonoBehaviour
{
    public PopUpType type;
    public ShowPopUpMsg showPopUp;
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        showPopUp.PopUpType = type;
        Transmitter.Send(showPopUp);
    }
}
