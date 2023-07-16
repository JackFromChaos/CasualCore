using TMPro;
using UnityEngine;
using Zenject;

public class LocalizeText : MonoBehaviour,IListener<ChangeLangMsg>
{
    //[Inject]
    //public LocalizeManager manager;
    public TMP_Text text;
    public string key;
    public string defaultText;

    void Start()
    {
        if(text == null)
            text = GetComponent<TMP_Text>(); 
        defaultText=text.text;
        Localize();
    }

    public void Localize(string defaultText)
    {
        this.defaultText = defaultText;
        if(text != null)
            text.text = LocalizeManager.Instance.Get(key, defaultText);
    }
    public void Localize()
    {
        text.text = LocalizeManager.Instance.Get(key, defaultText);
    }
    public void Handle(ChangeLangMsg ev)
    {
        Localize();
    }
}