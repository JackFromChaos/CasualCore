using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class UnityExt 
{
    static private MonoBehaviour _monoBehaviour;
    public static void DelayedCall(float delaySeconds, System.Action callback)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            DelyaCallInternal(delaySeconds, callback);
        }
        else
        {
            EditorApplication.CallbackFunction delayedCall = null;
            delayedCall = () =>
            {
                if (EditorApplication.timeSinceStartup >= delaySeconds)
                {
                    if (callback != null)
                    {
                        callback();
                    }
                    EditorApplication.update -= delayedCall;
                }
            };
            EditorApplication.update += delayedCall;

        }
#else
            DelyaCallInternal(delaySeconds, callback);
#endif
    }



    private static void DelyaCallInternal(float delaySeconds, System.Action callback)
    {
        if (_monoBehaviour == null|| _monoBehaviour.gameObject==null)
        {
            _monoBehaviour = new GameObject("DelayUtility").AddComponent<MonoBehaviour>();
            GameObject.DontDestroyOnLoad(_monoBehaviour.gameObject);
        }
        _monoBehaviour.StartCoroutine(DelayedCallCoroutine(delaySeconds, callback));
    }
    private static IEnumerator DelayedCallCoroutine(float delaySeconds, System.Action callback)
    {
        yield return new WaitForSeconds(delaySeconds);
        if (callback != null)
        {
            callback();
        }
    }
}
