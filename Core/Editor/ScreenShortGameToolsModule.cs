using UnityEngine;

public class ScreenShortGameToolsModule : BaseButtonGameToolsModule
{
    protected override void OnClick()
    {
        string fileName = GetFileName();
        UnityEngine.ScreenCapture.CaptureScreenshot(fileName);
        Debug.Log($"ScreenShortGameToolsModule: {fileName}");
    }
    private string GetFileName()
    {
        string path = Application.dataPath + "/../../Temp/ScreenShorts/";
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }
        for (int i = 0; i < 99999; i++)
        {
            string fileName = $"{path}screen_{i}.png";
            if (!System.IO.File.Exists(fileName))
            {
                return fileName;
            }
        }
        return Application.dataPath + "{path}screenshot.png";
    }

}