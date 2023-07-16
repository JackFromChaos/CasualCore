using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopUpAnimatedController : PopUpController
{
    public RectTransform contentPopUp;
    public Image blackout;
    protected Vector3 _savePopUpPosition;
    protected Color _saveBlackoutColor;
    protected Vector3 _savePopUpScale;
    protected const float showAnimationTime = 0.3f;
    protected const float hideAnimationTime = 0.2f;
    
    public enum AnimationType
    {
        Scale, ToLeft, ToRight, Top, Bottom
    }
    public AnimationType animationType;
    protected virtual void Awake()
    {
        if (blackout != null)
            _saveBlackoutColor = blackout.color;
        if (contentPopUp != null)
        {
            _savePopUpPosition = contentPopUp.localPosition;
            _savePopUpScale = contentPopUp.localScale;
        }

    }

    protected Vector3 GetHidePosition()
    {
        Vector3 posHide = _savePopUpPosition;
        switch (animationType)
        {
            case AnimationType.ToLeft:
                posHide.x += contentPopUp.rect.width + 20;
                break;
            case AnimationType.ToRight:
                posHide.x -= contentPopUp.rect.width + 20;
                break;
            case AnimationType.Top:
                posHide.y += contentPopUp.rect.height + 20;
                break;
            case AnimationType.Bottom:
                posHide.y -= contentPopUp.rect.height + 20;
                break;
        }

        return posHide;


    }
    public override void StartShow()
    {
        base.StartShow();

        if (blackout != null)
        {
            Color a = _saveBlackoutColor;
            a.a = 0;
            blackout.color = a;
            blackout.DOColor(_saveBlackoutColor, 0.2f);
        }

        if (contentPopUp != null)
        {
            if (animationType == AnimationType.Scale)
            {
                contentPopUp.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                contentPopUp.DOScale(_savePopUpScale, showAnimationTime).SetEase(Ease.OutBack);
            }
            else
            {
                Vector3 posHide = GetHidePosition();
                contentPopUp.localPosition = posHide;
                contentPopUp.DOLocalMove(_savePopUpPosition, showAnimationTime).SetEase(Ease.OutQuad);
            }

        }
    }

    public override void StartHide()
    {
        base.StartHide();
        if (contentPopUp != null)
        {
            if (animationType == AnimationType.Scale)
            {
                contentPopUp.DOScale(new Vector3(0.01f, 0.01f, 0.01f), hideAnimationTime).SetEase(Ease.OutBack);
            }
            else
            {
                Vector3 posHide = GetHidePosition();
                contentPopUp.DOLocalMove(posHide, hideAnimationTime).SetEase(Ease.OutQuad);
            }
        }

        if (blackout != null)
        {
            Color a = _saveBlackoutColor;
            a.a = 0;
            blackout.DOColor(a, 0.2f);
        }
    }

    public override float GetHideInterval()
    {
        return 0.3f;
    }
}