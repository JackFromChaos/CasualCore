using System;
using UnityEngine;

public class GUIScaler : IDisposable
{
    private Matrix4x4 guiMatrix;

    public GUIScaler(float screenWidthBase = 1024f)
    {
        guiMatrix = GUI.matrix;
        /*float originAspect = screenWidthBase / screenHeightBase;
        float currentAspect = (float) Screen.width / Screen.height;
        float relation = currentAspect / originAspect;*/
        /*float relation = (float)Screen.width / (float)screenWidthBase;
        Vector3 guiScale = new Vector3(Screen.width / screenWidthBase / relation, Screen.height / screenHeightBase,
            1f);*/
        Vector3 guiScale = new Vector3(Screen.width / screenWidthBase , Screen.width / screenWidthBase,
            1f);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
            guiScale);
    }

    public void Dispose()
    {
        GUI.matrix = guiMatrix;
    }
}
