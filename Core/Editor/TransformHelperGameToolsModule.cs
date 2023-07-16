using UnityEditor;
using UnityEditor.U2D.PSD;
using UnityEngine;

public class TimeHelperGameToolsModule : ExpandableGameToolsModule
{
    public override string Name => "Time";
    public override void Init()
    {
    }

    public override bool IsEnable()
    {
        return Application.isPlaying;
    }

    public override void InlineOnGUI()
    {
        base.InlineOnGUI();
        GUILayout.Label($"Current time scale: {Time.timeScale * 100}%");
        TimeScaleButton(0.05f);
        TimeScaleButton(0.25f);
        TimeScaleButton(0.5f);
        TimeScaleButton(1f);
    }

    protected override void RealOnGUI()
    {
        GUILayout.BeginHorizontal();
        TimeScaleButton2(0f);
        TimeScaleButton2(0.01f);
        TimeScaleButton2(0.05f);
        TimeScaleButton2(0.1f);
        TimeScaleButton2(0.25f);
        TimeScaleButton2(0.5f);
        TimeScaleButton2(0.75f);
        TimeScaleButton2(1f);
        GUILayout.EndHorizontal();
    }
    private void TimeScaleButton(float f)
    {
        if (GUILayout.Button($"{f * 100}%", GUILayout.Width(50)))
        {
            Time.timeScale = f;
        }
    }
    private void TimeScaleButton2(float f)
    {
        if (GUILayout.Button($"{f * 100}%", GUILayout.Width(50), GUILayout.Height(30)))
        {
            Time.timeScale = f;
        }
    }

}
public class TransformHelperGameToolsModule : ExpandableGameToolsModule
{
    public override string Name => "Transform";
    public override void Init()
    {
    }

    public override bool IsEnable()
    {
        return true;
    }

    public override void InlineOnGUI()
    {
        base.InlineOnGUI();
        if (GUILayout.Button("Copy"))
        {
            Transform current = Selection.activeTransform;
            if (current != null)
            {
                UnityEditorInternal.ComponentUtility.CopyComponent(current);

            }
        }

        if (GUILayout.Button("Past"))
        {
            Transform current = Selection.activeTransform;
            if (current != null)
            {
                UnityEditorInternal.ComponentUtility.PasteComponentValues(current);

            }
        }
        if (GUILayout.Button("Reset"))
        {
            foreach (var transform in Selection.transforms)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
            }
        }
    }


    protected override void RealOnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("ResetPos"))
        {
            foreach (var transform in Selection.transforms)
            {
                transform.localPosition = Vector3.zero;
            }
        }

        if (GUILayout.Button("ResetRot"))
        {
            foreach (var transform in Selection.transforms)
            {
                transform.localRotation = Quaternion.identity;
            }
        }

        if (GUILayout.Button("ResetScale"))
        {
            foreach (var transform in Selection.transforms)
            {
                transform.localScale = Vector3.one;
            }
        }

        if (GUILayout.Button("ResetPivot"))
        {
            foreach (var transform in Selection.transforms)
            {
                var rect = transform.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.pivot = new Vector2(0.5f, 0.5f);
                }
            }
        }

        if (GUILayout.Button("ResetAnchor"))
        {
            foreach (var transform in Selection.transforms)
            {
                var rect = transform.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.anchorMin = new Vector2(0.5f, 0.5f);
                    rect.anchorMax = new Vector2(0.5f, 0.5f);
                }
            }
        }
        GUILayout.EndHorizontal();
    }
}