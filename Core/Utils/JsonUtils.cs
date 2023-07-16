using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonUtils 
{
    public static T JsonClone<T>(T source) where T : class
    {
        string json = JsonUtility.ToJson(source);
        return JsonUtility.FromJson<T>(json);
    }

}
