using System;
using System.Collections;
using UnityEngine;

public static class GameObjectExt
{

    public static void DelayCall(this MonoBehaviour o, Action action, float delay)
    {
        o.StartCoroutine(DelayCallC(action, delay));
    }
    public static void DelayCallFrame(this MonoBehaviour o, Action action)
    {
        o.StartCoroutine(DelayCallFrameC(action));
    }

    private static IEnumerator DelayCallC(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    private static IEnumerator DelayCallFrameC(Action action)
    {
        yield return new WaitForEndOfFrame();
        action();
    }

    public static T GetOrCreateComponent<T>(this GameObject o) where T : Component
    {
        T result=o.GetComponent<T>();
        if (result == null)
        {
            result = o.AddComponent<T>();
        }
        if(DI.IsInitialized())
            DI.Inject(result);

        return result;
    }

    public static T FindChildComponent<T>(this Transform go, string thisName) where T : Component
    {
        Transform result = FindChildByName(go, thisName);
        if (result != null)
        {
            return result.GetComponent<T>();
        }
        return null;
    }
    public static Transform FindChildByName(this Transform go, string thisName)
    {
        Transform result = null;

        if (go.name != thisName)
        {
            foreach (Transform child in go)
            {
                result = FindChildByName(child, thisName);
                if (result != null)
                {
                    break;
                }
            }
        }
        else
        {
            result = go.transform;
        }
        return result;
    }
    public static Vector3 GetRotation(this Vector3 forward, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        Vector3 result = new Vector3(forward.x * Mathf.Cos(rad) + forward.z * Mathf.Sin(rad), 0,
            forward.z * Mathf.Cos(rad) - forward.x * Mathf.Sin(rad));
        return result;
    }
}