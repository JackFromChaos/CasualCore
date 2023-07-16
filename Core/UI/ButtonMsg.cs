using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonMsg : MonoBehaviour
{
    [SerializeReference]
    public IMsg message;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Transmitter.SendByObject(message);
    }
}