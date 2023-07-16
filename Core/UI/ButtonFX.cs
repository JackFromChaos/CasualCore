using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFX : Button
{
    public string buttonId;
    private Tween _scaleTween;
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        ScaleTo(1f, Ease.OutBack);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        try
        {
            Transmitter.Send(new LogButtonClickMsg() { buttonId = GetButtonId() });
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
        base.OnPointerClick(eventData);
        ScaleTo(1f, Ease.OutBack);
    }

    private string GetButtonId()
    {
        if (string.IsNullOrEmpty(buttonId))
        {
            string id = gameObject.name;
            var com=GetComponentInParent<PopUpController>();
            if (com != null)
            {
                if (com.initialShowRequest != null)
                    id = com.initialShowRequest.PopUpType.ToString() + "_" + id;
                else
                {
                    id = com.gameObject.name + "_" + id;
                }
            }
            return id;
        }
        return buttonId;

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        ScaleTo(0.8f, Ease.OutCubic);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        ScaleTo(1f, Ease.OutBack);
    }

    private void OnApplicationFocus(bool focus)
    {
        _scaleTween?.Kill();
        transform.localScale = Vector3.one;
    }

    private void ScaleTo(float value, Ease ease)
    {
        _scaleTween?.Kill();
        if (this != null)
        {
            _scaleTween = transform.DOScale(value, 0.3f)
                .SetEase(ease)
                .SetUpdate(true)
                .SetTarget(this);
        }
    }

    protected override void OnDestroy()
    {
        _scaleTween?.Kill();
        this.DOKill();
    }

}