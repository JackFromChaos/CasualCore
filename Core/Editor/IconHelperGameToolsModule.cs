using UnityEditor;
using UnityEngine;

public class IconHelperGameToolsModule : InlineGameToolsModule
{

    public Texture2D texture;
    public override string Name => "Icon helper";

    public override bool IsEnable()
    {
        return !Application.isPlaying;
    }

    public override void InlineOnGUI()
    {
        texture = EditorGUILayout.ObjectField("Texture", texture, typeof(Texture2D), false, GUILayout.Height(16)) as Texture2D;
        if (texture != null && GUILayout.Button("Resize"))
        {
            SaveIcons();
        }
    }

    private readonly int[] iconSizes = new int[] { 432, 324, 216, 162, 108, 81, 192, 144, 96, 72, 48, 36 };

    private void SaveIcons()
    {
        foreach (int size in iconSizes)
        {
            string path = System.IO.Path.Combine(Application.dataPath, $"icon_{size}.png");
            int width = size;
            int height = size;
            System.IO.File.WriteAllBytes(path, ResizeTexture(texture, width, height));
        }
    }

    private byte[] ResizeTexture(Texture2D texture, int width, int height)
    {
        texture.filterMode = FilterMode.Trilinear;
        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        RenderTexture.active = rt;
        Graphics.Blit(texture, rt);
        Texture2D output = new Texture2D(width, height);
        output.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        output.Apply();
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);
        return output.EncodeToPNG();
    }
}