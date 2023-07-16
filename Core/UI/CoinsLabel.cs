using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsLabel : MonoBehaviour, IListener<SyncCoinsMsg>
{
    public TextMeshProUGUI text;

    void Start()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        Transmitter.Ask<SyncCoinsMsg>(this);
    }

    public void Handle(SyncCoinsMsg ev)
    {
        text.text = ev.newValue.ToString();
    }
}
