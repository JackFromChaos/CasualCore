using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class AutoChildRegistrator : MonoInstaller<DI>
{
    public override void InstallBindings()
    {
        if (DI.instance == null)
        {
            Debug.LogError(" Container == null");
            return;
        }
        DI.instance.AutoRegisterChildren(gameObject);
    }
}