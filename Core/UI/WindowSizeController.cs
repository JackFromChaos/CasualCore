using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class WindowSizeController : MonoBehaviour
{
    public RectTransform content;
    public Vector2 padding;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (content == null) return;

        LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        rectTransform.sizeDelta = new Vector2(content.rect.width + padding.x, content.rect.height + padding.y);
    }
}