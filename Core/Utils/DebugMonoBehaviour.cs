using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMonoBehaviour : MonoBehaviour
{
    void Awake()
    {
        Debug.Log($"{name} Awake");
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"{name} Start");
    }

    void OnEnable()
    {
        Debug.Log($"{name} OnEnable");
    }

    void OnDisable()
    {
        Debug.Log($"{name} OnDisable");
    }

    void OnDestroy()
    {
        Debug.Log($"{name} OnDestroy");
    }

}
