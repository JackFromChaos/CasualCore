using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

[AutoBind]
public class DI : MonoInstaller<DI>
{
    public static DI instance { get; set;}
    public GenericDictionary<string, MonoBehaviour> bindingViewer = new GenericDictionary<string, MonoBehaviour>();

    private void Awake()
    {
        if (instance==null)
            instance = this;
        DontDestroyOnLoad(this);
    }

    public override void InstallBindings()
    {
        if (instance == null)
            instance = this;
        AutoRegisterChildren(gameObject);
    }

    public static new GameObject Instantiate(UnityEngine.Object prefab, Transform parent = null)
    {
        if (instance == null)
        {
            Debug.LogError("DO not Use DI installer before Initialization");
            return GameObject.Instantiate(prefab, parent) as GameObject;
        }
        if(parent==null)
            return instance.Container.InstantiatePrefab(prefab);
        if (instance == null)
            Debug.LogError("instance == null");
        if (instance.Container == null)
            Debug.LogError("instance.Containe == null");
        return instance.Container.InstantiatePrefab(prefab, parent);
    }
  
    public static bool IsInitialized()
    {
        return instance != null;
    }

    public static void Inject(object o)
    {
        instance.Container.Inject(o);
    }

    public  void AutoRegisterChildren(GameObject obj)
    {
            if (Container == null)
        {
            Debug.LogError(" Container == null");
            return;
        }

        MonoBehaviour[] components = obj.GetComponentsInChildren<MonoBehaviour>();
        foreach (var comp in components)
        {
            if (comp == null)
            {
                Debug.LogError("Missing Conponent in obj.name = " + obj.name);
                continue;
            }
            var atr = (AutoBind)Attribute.GetCustomAttribute(comp.GetType(), typeof(AutoBind));
            if ( atr!= null)
            {
                Type bindedType = (atr.bindWithType != null) ? atr.bindWithType : comp.GetType();
                if (!instance.bindingViewer.ContainsKey(bindedType.Name.ToString()))
                {
                    instance.Container.Bind(bindedType).FromInstance(comp);
                    instance.bindingViewer.Add(bindedType.Name.ToString(), comp);
                    bindingViewer = instance.bindingViewer;
                }
                else {
                    Debug.LogError("Attempt Double building bindedType = " + bindedType);
                }
            }
            else
            {
                if( comp.GetType()!= typeof(SceneContext) && comp.GetType() != typeof(ProjectContext) &&
                    !comp.GetType().FullName.StartsWith("UnityEngine.") &&
                    !comp.GetType().FullName.StartsWith("TMPro.") &&
                    !comp.GetType().FullName.StartsWith("Offers.") &&
                    !comp.GetType().FullName.StartsWith("ButtonFX") &&
                    !comp.GetType().FullName.StartsWith("Ads.AdsController") &&
                    comp.GetType() != typeof(AutoChildRegistrator) &&
                    comp.GetType()!= typeof(DigitalRuby.SoundManagerNamespace.SoundManager))
                    Debug.LogError("Type comp.GetType() = " + comp.GetType() + " Is not marked as AutoBind- it look like error ("+comp.GetType().FullName);
            }
        }
    }
  
}