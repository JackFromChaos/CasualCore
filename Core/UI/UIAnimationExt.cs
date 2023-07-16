using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class UIAnimationExt
{
    public static void SetAlpha(this Image image, float alpha)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }
    public static void SetAlpha(this TMPro.TextMeshProUGUI text, float alpha)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }

    public static void SetSprite2D(this Transform rect, Sprite sprite)
    {
        var image=rect.GetComponent<Image>();
        if (image != null)
        {
            image.sprite = sprite;
            return;
        }
        var rawImage = rect.GetComponent<RawImage>();
        if (rawImage != null)
        {
            rawImage.texture = sprite.texture;
            return;
        }
        var spr = rect.GetComponent<SpriteRenderer>();
        if (spr != null)
        {
            spr.sprite = sprite;
            return;
        }
    }
    public static void UISetAlpha(this Transform rect, float value)
    {
        var images = rect.GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            var color=image.color;
            color.a = value;
            image.color = color;
        }
        var rawImages=rect.GetComponentsInChildren<RawImage>();
        foreach (var rawImage in rawImages)
        {
            var color = rawImage.color;
            color.a = value;
            rawImage.color = color;
        }
        var texts = rect.GetComponentsInChildren<Text>();
        foreach (var text in texts)
        {
            var color = text.color;
            color.a = value;
            text.color = color;
        }
        var tmps=rect.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        foreach (var tmp in tmps)
        {
            var color = tmp.color;
            color.a = value;
            tmp.color = color;
        }
        var spr = rect.GetComponentsInChildren<SpriteRenderer>();
        foreach (var image in spr)
        {
            var color = image.color;
            color.a = value;
            image.color = color;
        }
    }
    public static void UISetAlphaWithotText(this Transform rect, float value)
    {
        var images = rect.GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            var color = image.color;
            color.a = value;
            image.color = color;
        }
        var rawImages = rect.GetComponentsInChildren<RawImage>();
        foreach (var rawImage in rawImages)
        {
            var color = rawImage.color;
            color.a = value;
            rawImage.color = color;
        }

        var spr = rect.GetComponentsInChildren<SpriteRenderer>();
        foreach (var image in spr)
        {
            var color = image.color;
            color.a = value;
            image.color = color;
        }
    }
    public static void UISetColorWithoutText(this Transform rect, Color value)
    {
        var images = rect.GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            image.color = value;
        }
        var rawImages = rect.GetComponentsInChildren<RawImage>();
        foreach (var rawImage in rawImages)
        {
            rawImage.color = value;
        }
        var spr = rect.GetComponentsInChildren<SpriteRenderer>();
        foreach (var image in spr)
        {
            image.color = value;
        }
    }
    public static void UISetColor(this Transform rect, Color value)
    {
        var images = rect.GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            image.color = value;
        }
        var rawImages = rect.GetComponentsInChildren<RawImage>();
        foreach (var rawImage in rawImages)
        {
            rawImage.color = value;
        }
        var texts = rect.GetComponentsInChildren<Text>();
        foreach (var text in texts)
        {
            text.color = value;
        }
        var tmps = rect.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        foreach (var tmp in tmps)
        {
            tmp.color = value;
        }
        var spr = rect.GetComponentsInChildren<SpriteRenderer>();
        foreach (var image in spr)
        {
            image.color = value;
        }
    }


    public static void UIFadeAlpha(this Transform rect, float value, float time = 0.3f, Ease ease = Ease.OutBack)
    {
        var images=rect.GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            image.DOFade(value, time).SetEase(ease).SetUpdate(true).SetTarget(image);
        }
        var rawImages=rect.GetComponentsInChildren<RawImage>();
        foreach (var rawImage in rawImages)
        {
            rawImage.DOFade(value, time).SetEase(ease).SetUpdate(true).SetTarget(rawImage);
        }
        var texts = rect.GetComponentsInChildren<Text>();
        foreach (var text in texts)
        {
            text.DOFade(value, time).SetEase(ease).SetUpdate(true).SetTarget(text);
        }
        var tmps=rect.GetComponentsInChildren<TMPro.TextMeshPro>();
        foreach (var tmp in tmps)
        {
            tmp.DOFade(value, time).SetEase(ease).SetUpdate(true).SetTarget(tmp);
        }
        var spr = rect.GetComponentsInChildren<SpriteRenderer>();
        foreach (var image in spr)
        {
            image.DOFade(value, time).SetEase(ease).SetUpdate(true).SetTarget(image);
        }
    }


    public static Sprite GetSprite(this GameObject o)
    {
        var image=o.GetComponent<Image>();
        if (image != null)
            return image.sprite;
        /*var rawImage=o.GetComponent<RawImage>();
        if (rawImage != null)
            return rawImage.texture.ToSprite();*/
        var spriteRenderer=o.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            return spriteRenderer.sprite;
        return null;
    }
    public static void SprSetAlpha(this Transform rect, float value)
    {
        var spr = rect.GetComponentsInChildren<SpriteRenderer>();
        foreach (var image in spr)
        {
            var color = image.color;
            color.a = value;
            image.color = color;
        }
        /*var rawImages = rect.GetComponentsInChildren<RawImage>();
        foreach (var rawImage in rawImages)
        {
            var color = rawImage.color;
            color.a = value;
            rawImage.color = color;
        }
        var texts = rect.GetComponentsInChildren<Text>();
        foreach (var text in texts)
        {
            var color = text.color;
            color.a = value;
            text.color = color;
        }
        var tmps = rect.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        foreach (var tmp in tmps)
        {
            var color = tmp.color;
            color.a = value;
            tmp.color = color;
        }*/
    }
    public static void SprFadeAlpha(this Transform rect, float value, float time = 0.3f, Ease ease = Ease.OutBack)
    {
        var spr = rect.GetComponentsInChildren<SpriteRenderer>();
        foreach (var image in spr)
        {
            image.DOFade(value, time).SetEase(ease).SetUpdate(true).SetTarget(image);
        }
        /*var rawImages = rect.GetComponentsInChildren<RawImage>();
        foreach (var rawImage in rawImages)
        {
            rawImage.DOFade(value, time).SetEase(ease).SetUpdate(true).SetTarget(rawImage);
        }
        var texts = rect.GetComponentsInChildren<Text>();
        foreach (var text in texts)
        {
            text.DOFade(value, time).SetEase(ease).SetUpdate(true).SetTarget(text);
        }
        var tmps = rect.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        foreach (var tmp in tmps)
        {
            tmp.DOFade(value, time).SetEase(ease).SetUpdate(true).SetTarget(tmp);
        }*/
    }

    public static void ScaleTo(this Button button, float value,float time=0.3f, Ease ease=Ease.OutBack)
    {
        var scaleTween = button.transform.DOScale(value, time)
            .SetEase(ease)
            .SetUpdate(true)
            .SetTarget(button);
    }
}