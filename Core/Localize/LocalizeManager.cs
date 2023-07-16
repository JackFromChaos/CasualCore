using Sinbad;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

[AutoBind]
public class LocalizeManager : MonoBehaviour
{
    public static LocalizeManager Instance { get; private set; }
    public string currentLanguage="en";
    Dictionary<string,Dictionary<string,string>> languages = new Dictionary<string, Dictionary<string, string>>();
    public string locFolder="Locals";
    public bool autoLang = true;
    public List<TextAsset> files;
    [Button]
    public void SetLanguage(string language)
    {
        if (currentLanguage != language)
        {
            currentLanguage = language;
            Transmitter.Send(new ChangeLangMsg());
        }
    }

    void Awake()
    {
        Instance = this;
        LoadLocals();
        if (autoLang)
        {
            SystemLanguage systemLanguage = Application.systemLanguage;
            switch (systemLanguage)
            {
                case SystemLanguage.Russian:
                    SetLanguage("ru");
                    break;
                default:
                    SetLanguage("en");
                    break;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Dictionary<string, string> GetLang(string lang)
    {
        if (!languages.TryGetValue(lang, out var result))
        {
            result=new Dictionary<string, string>();
            languages[lang]=result;
        }
        return result;
    }
    public void LoadLocals()
    {
        if (files.Count > 0)
        {
            foreach (var asset in files)
            {
                var text = asset.text;
                CsvUtil.defaultSelector = ";";
                var elements = CsvUtil.LoadObjectsFromText<LocalizeElement>(text);
                foreach (var element in elements)
                {
                    GetLang("en")[element.key] = element.en;
                    GetLang("ru")[element.key] = element.ru;
                }
            }
        }
        else
        {
            string folderPath = Path.Combine(Application.streamingAssetsPath, locFolder);
            var txtFiles = Directory.GetFiles(folderPath, "*.csv", SearchOption.AllDirectories);
            foreach (var txtFile in txtFiles)
            {
                var text = File.ReadAllText(txtFile);
                CsvUtil.defaultSelector = ";";
                var elements = CsvUtil.LoadObjectsFromText<LocalizeElement>(text);
                foreach (var element in elements)
                {
                    GetLang("en")[element.key] = element.en;
                    GetLang("ru")[element.key] = element.ru;
                }

                Debug.Log(Path.GetFileNameWithoutExtension(txtFile));
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string Get(string key, string defaultText)
    {
        if (languages.TryGetValue(currentLanguage, out var lang))
        {
            if (lang.TryGetValue(key, out var text))
                return text;
            else
                Debug.LogWarning($"Key {key} not found in language {currentLanguage}");
        }
        else
        {
            Debug.LogWarning($"Language {currentLanguage} not found");
        }
        return defaultText;
    }

    public void Set(string elementKey, string elementRu, string elementEn)
    {
        var lang=GetLang("en");
        lang[elementKey] = elementEn;
        lang=GetLang("ru");
        lang[elementKey] = elementRu;
    }
}

public class ChangeLangMsg
{
    public LocalizeManager manager;
}
[System.Serializable]
public class LocalizeElement
{
    public string key;
    public string ru;
    public string en;
}